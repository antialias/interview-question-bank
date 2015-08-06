// Image Upload Process
// Procedure for uploading images from local to 1stdibs
//FolderPath – directory path from where source images will be picked up
//Uploaded Type – Regular or White background image
//Upload images for furniture vertical
public int UploadImages(string FolderPath, UploadType uploadType, System.Windows.Forms.Label lblStatus = null)
{
            string loggedinUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

    int successfullyProcessed = 0;
    CookieContainer cookieJar = new CookieContainer();
    WebClientX client = new WebClientX(cookieJar);

    NameValueCollection postData = new NameValueCollection();
    postData.Add("email", UserName);    //1stdibs account for india
    postData.Add("password", Password); //provide password
    postData.Add("do-login", "1");

    byte[] responseBytes = client.UploadValues(_login_url_topost, postData);//login
    string response = Encoding.UTF8.GetString(responseBytes);


    CQ loggedInDom = response;
    CQ passwordbox = loggedInDom.Find("[type=\"password\"][name=\"password\"]");
    if (passwordbox.Length == 0)
    {
        DirectoryInfo diFolder = new DirectoryInfo(FolderPath);
   //Get files info from directory
        IEnumerable<FileInfo> imageFiles = from p in diFolder.GetFiles("*.*", SearchOption.AllDirectories)
                                           where (p.Extension.ToLower() == ".jpg") || (p.Extension.ToLower() == ".jpeg")
                                           select p;
        int counter = 0;
        int total = imageFiles.Count();
        if (total > 0)
        {
            Dictionary<string, string> UploadUrls = new Dictionary<string, string>();
            List<Product> pending = this.DownloadImages(out UploadUrls).ToList();//same function used  for download - get all records in pending list
            string userToken = null;
            foreach (FileInfo imageFile in imageFiles)
            {
                counter++;
                string ItemID = Path.GetFileNameWithoutExtension(imageFile.FullName).Trim();

                //abort image uploading if it does not exist in pending list
                var exists = (from p in pending
                              where string.Equals(p.Item, ItemID, StringComparison.OrdinalIgnoreCase)
                              select p).FirstOrDefault();
                if (exists != null)
                {
                    try
                    {
                        if (userToken == null)
                        {

    string uploadPage = client.DownloadString(UploadUrls[exists.Item]);
                            CQ uploadPageDom = uploadPage;
                 userToken = (from p in cookieJar.GetCookies(new Uri("https://www.1stdibs.com")).Cast<Cookie>()
                                         where p.Name.Equals("userToken")
                                         select p.Value).FirstOrDefault();
                        }

                     //get seller id
                     //1.	Get current image sequence & extract seller id (GET)
                     // AppSettings.GetSequenceURL - https://adminv2.1stdibs.com/soa/inventory/3.1/{0}/item/{1}?userToken={2}
						string getSequenceURL = string.Format(AppSettings.GetSequenceURL, "furniture", ItemID, userToken);
                        string curSequenceJSON = client.DownloadString(getSequenceURL);
                        CurrentSequence curSequence = new JavaScriptSerializer().Deserialize<CurrentSequence>(curSequenceJSON);
                        //backup current sequence
                        string bkpCurrentSequenceJSON = curSequenceJSON;

                        bool uploadSuccess = false;
                        AcceptedImageList lstUploaded = null;
                        try
                        {
                            //2.	Upload image to server (POST)
                            string uploadURL = "https://adminv2.1stdibs.com/image/ajax/dealer_image_upload?seller_id=" + curSequence.result.furnitureItem.seller.id;

                            byte[] uploadFileResponseBytes = client.UploadFile(uploadURL, "POST", imageFile.FullName);//pick file from same directory
                            string uploadFileResponse = Encoding.UTF8.GetString(uploadFileResponseBytes);


                           lstUploaded = AcceptedImageList.GetItemDetails(uploadFileResponse);

                            if (lstUploaded.status.Equals("success", StringComparison.InvariantCultureIgnoreCase))
                                uploadSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            WriteMessageToFile("File upload failed, " + ex.Message, System.Diagnostics.TraceEventType.Error);
                        }

                        if (uploadSuccess)
                        {

 string original = lstUploaded.accepted[0].original;
                            string thumb = lstUploaded.accepted[0].thumb;
                            string large = lstUploaded.accepted[0].large;

                            //3.	Get small and medium images (POST)
                            string resizeImageURL = "https://adminv2.1stdibs.com/image/ajax/ajax_dealer_resize_images";
                            NameValueCollection resizeParams = new NameValueCollection();
                            resizeParams.Add("images[image1][web_url]", lstUploaded.accepted[0].large);
                            byte[] responseResizeBytes = client.UploadValues(resizeImageURL, resizeParams);//login
                            string responseResize = Encoding.UTF8.GetString(responseResizeBytes);
                            ResizedImage imgResized = ResizedImage.GetResizedImage(responseResize);




                            string medium = imgResized.image1.medium;                                    string small = imgResized.image1.small;
                            //4.	Save the newly updated image sequences (PUT)
                            List<Image> images = null;
                            if (curSequence.result.furnitureItem.images != null)
                            {
                                images = curSequence.result.furnitureItem.images;
                            }
                            else
                            {
                                images = new List<Image>();
                            }

                            var Count = (from p in images
                                         where string.IsNullOrWhiteSpace(p.large) != true
                                         select p).Count();

                            if ((Count == 10) || (uploadType == UploadType.WhiteBackground))
                            {
                                //if all 10 blocks are occupied then replace first image with new one
                                Image FirstImage = (from p in curSequence.result.furnitureItem.images
                                                    where p.position == 1
                                                    select p).FirstOrDefault();
                                if (FirstImage == null)
                                    throw new Exception("First image not found");

                                FirstImage.large = large;
                                FirstImage.thumb = thumb;
                                FirstImage.medium = medium;
                                FirstImage.small = small;
                            }
                            else
                            {
                                //replace last block image with first block image, change sequence of other blocks
                                //if value exists in 10th block then update first image only
                                if (Count < 10 && Count > 0)
                                {
                                    MoveImagesNext(Count, curSequence);

                                    //newly uploaded image must be at first position
                                    Image first = new Image();
                                    first.position = 1;
                                    first.large = large;
                                    first.thumb = thumb;
                                    first.medium = medium;
                                    first.small = small;
                                    curSequence.result.furnitureItem.images[Count] = first;

                                    curSequence.result.furnitureItem.images = (from p in curSequence.result.furnitureItem.images
                                                                               orderby p.position ascending
                                                                               select p).ToList();
                                }

                                switch (Count)
                                {
                                    case 0:
                                        {
                                            Image first = new Image();
                                            first.position = 1;
                                            first.large = large;
                                            first.thumb = thumb;
                                            first.medium = medium;
                                            first.small = small;
                                            curSequence.result.furnitureItem.images.Add(first);
                                        }
                                        break;
                                    case 10:
                                        {
                                            Image first = (from p in curSequence.result.furnitureItem.images
                                                           where p.position == 1
                                                           select p).First();
                                            first.large = large;
                                            first.thumb = thumb;
                                            first.medium = medium;
                                            first.small = small;
                                                                                            }
                                        break;
                                }
                            }

                            try
                            {
                                string saveSequenceURL = string.Format(AppSettings.SaveSequenceURL, "furniture", ItemID, userToken);
                                JsonReader jsonReader = new JsonTextReader(new StringReader(bkpCurrentSequenceJSON)) { DateParseHandling = DateParseHandling.None };
                                dynamic rss = JObject.Load(jsonReader);

                                var imagesX = (from p in curSequence.result.furnitureItem.images
                                               where string.IsNullOrWhiteSpace(p.large) == false
                                               select p).ToArray();

                                if (!string.IsNullOrWhiteSpace(Convert.ToString(curSequence.result.furnitureItem.pricing.initialPriceCurrencies)))
                                    rss.result.furnitureItem.pricing.initialPriceCurrencies = (JObject)JToken.FromObject(curSequence.result.furnitureItem.pricing.initialPriceCurrencies);

                                if (!string.IsNullOrWhiteSpace(Convert.ToString(curSequence.result.furnitureItem.pricing.amountCurrencies)))
                                    rss.result.furnitureItem.pricing.amountCurrencies = (JObject)JToken.FromObject(curSequence.result.furnitureItem.pricing.amountCurrencies);

                                rss.result.furnitureItem.images = (JArray)JToken.FromObject(imagesX);//set the updated sequence
                                rss.result.furnitureItem.images[0].status = "PENDING";
                                var count = rss.result.furnitureItem.images.Count;
                                rss.result.furnitureItem.images[count - 1].original = Convert.ToString(lstUploaded.accepted[0].original);
                                foreach (var img in rss.result.furnitureItem.images)
                                {
                                    if (img.small == null)
                                    {
                                        img.Remove("small");
                                    }
                                    if (img.medium == null)
                                    {
                                        img.Remove("medium");
                                    }
                                }

                                var json = JsonConvert.SerializeObject(rss.result.furnitureItem, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });//new
                                client = new WebClientX(cookieJar, "application/json; charset=UTF-8");

                                //Issue 403 forbidden
                                byte[] changeSequenceByte = client.UploadData(saveSequenceURL, "PATCH", Encoding.UTF8.GetBytes(json));
                                string changeSequenceResponse = Encoding.UTF8.GetString(changeSequenceByte);

                                //backup updated sequence
                                string bkpUpdatedSequence = JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });//new


                                //5.	Update status of the item (POST) to QC
                                client = new WebClientX(cookieJar);
                                string statusUpdateURL = "https://admin.1stdibs.com/citysearch-administration/photo_processing/ajax/i_update_imgstatus.php";
                                NameValueCollection postStatus = new NameValueCollection();
                                postStatus.Add("dstdiv", "imgstat" + ItemID);
                                postStatus.Add("itemID", ItemID);
                                postStatus.Add("imgstatus", "QC");//PENDING/QC
                                postStatus.Add("admin", UserName);

                                byte[] PostStatusBytes = client.UploadValues(statusUpdateURL, postStatus);
                                string responsePostStatus = Encoding.UTF8.GetString(PostStatusBytes);

                                successfullyProcessed++;
                            }
                            catch (Exception ex)
                            {
                                WriteMessageToFile("Error updating image sequence details, " + ex.Message, System.Diagnostics.TraceEventType.Error);
                            }
                        }

                    }
                    catch (Exception e2)
                    {
                        WriteMessageToFile("Unexpected error, Product image upload failed. Item ID: " + ItemID + e2.Message, System.Diagnostics.TraceEventType.Error);
                    }
                }
                else
                {
                    //successfullyProcessed++;
                    WriteMessageToFile(counter + ". Abort uploading " + ItemID + ", Image is not in pending items list.", System.Diagnostics.TraceEventType.Error);
                }
            }
        }
        else
        {
            WriteMessageToFile("No records to upload", System.Diagnostics.TraceEventType.Error);
        }
    }
    else
    {
        WriteMessageToFile("Login failed", System.Diagnostics.TraceEventType.Error);
        throw new Exception("Login failed");
    }

    return successfullyProcessed;
}


//Upload process for fashion & Jewelry
public int UploadImagesFashionJewelry(string FolderPath, string Vertical, System.Windows.Forms.Label lblStatus = null)
        {
            string detectedVertical = Vertical;
            string loggedinUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //login
            int successfullyProcessed = 0;
            CookieContainer cookieJar = new CookieContainer();
            WebClientX client = new WebClientX(cookieJar, false);

            NameValueCollection postData = new NameValueCollection();
            postData.Add("email", UserName);
            postData.Add("password", Password);
            postData.Add("do-login", "1");

            byte[] responseBytes = client.UploadValues(_login_url_topost, postData);//login
            string response = Encoding.UTF8.GetString(responseBytes);

            CQ loggedInDom = response;
            CQ passwordbox = loggedInDom.Find("[type=\"password\"][name=\"password\"]");
            if (passwordbox.Length == 0)
            {
                DirectoryInfo diFolder = new DirectoryInfo(FolderPath);
                IEnumerable<FileInfo> imageFiles = from p in diFolder.GetFiles("*.*", SearchOption.AllDirectories)
                                                   where (p.Extension.ToLower() == ".jpg") || (p.Extension.ToLower() == ".jpeg")
                                                   select p;
                int counter = 0;
                int total = imageFiles.Count();
                if (total > 0) //upload images 1 by 1
                {
                    client = new WebClientX(cookieJar, true);
                    string userToken = null;
                    foreach (FileInfo imageFile in imageFiles)
                    {
                        counter++;
                        string ItemID = Path.GetFileNameWithoutExtension(imageFile.FullName).Trim();
                        string[] ItemDetails = ImageDetailsRepository.GetItemKeyAndIsUploaded(ItemID);
                        if (!string.IsNullOrWhiteSpace(ItemDetails[0]))
                        {
                            string ItemKey = ItemDetails[0];

                            if (Vertical.ToLower() == "fashion")
                            {
                                if (!ItemKey.ToLower().StartsWith("v_"))
                                {
                                    throw new Exception("Please select correct vertical for jewelry item batch");
                                }
                            }

                            if (Vertical.ToLower() == "jewelry")
                            {
                                if (!ItemKey.ToLower().StartsWith("j_"))
                                {
                                    throw new Exception("Please select correct vertical for fashion item batch");
                                }
                            }

                            bool IsUploaded = Convert.ToBoolean(ItemDetails[1]);
                            if (!IsUploaded)
                            {
                                string UploadPageUrl = ConfigurationManager.AppSettings["UploadURL"] + ItemKey;
                                string uploadPage = string.Empty;
                                try
                                {
                                    if (userToken == null)
                                    {
                                        uploadPage = client.DownloadString(UploadPageUrl);

            							userToken = (from p in
                                             cookieJar.GetCookies(new Uri("https://www.1stdibs.com")).Cast<Cookie>()
                                             where p.Name.Equals("userToken")
                                             select p.Value).FirstOrDefault();
                                    }

                                    if (string.IsNullOrWhiteSpace(uploadPage))
                                    {
                                        uploadPage = client.DownloadString(UploadPageUrl);
                                    }

                                    CQ uploadPageDom = uploadPage;

                                    //get seller id
                                    //1.	Get current image sequence & extract seller id (GET)
                                    string getSequenceURL = string.Format(AppSettings.GetSequenceURL, Vertical.ToLower(),
                                        ItemID, userToken);

                                    string curSequenceJSON = client.DownloadString(getSequenceURL);

                                    JsonReader jsonReaderX = new JsonTextReader(new StringReader(curSequenceJSON)) { DateParseHandling = DateParseHandling.None };
                                    dynamic curSequence = JObject.Load(jsonReaderX);

                                    dynamic curItem = null;
                                    switch (Vertical)
                                    {
                                        case "fashion":
                                            curItem = curSequence.result.fashionItem;
                                            if (curItem == null)
                                            {
                                                detectedVertical = "jewelry";
                                                curItem = curSequence.result.jewelryItem;
                                            }
                                            break;
                                        case "jewelry":
                                            curItem = curSequence.result.jewelryItem;
                                            break;
                                    }

                                    bool uploadSuccess = false;
                                    AcceptedImageList lstUploaded = null;
                                    try
                                    {
                                        //2.	Upload image to server (POST)
                                        string ImageuploadURL = string.Format(AppSettings.ImageUploadURL, curItem.seller.id);

       									byte[] uploadFileResponseBytes = client.UploadFile(ImageuploadURL, "POST", imageFile.FullName); //pick file from same directory
                                        string uploadFileResponse = Encoding.UTF8.GetString(uploadFileResponseBytes);


                                        lstUploaded = AcceptedImageList.GetItemDetails(uploadFileResponse);

                                        if (lstUploaded.status.Equals("success",
                                            StringComparison.InvariantCultureIgnoreCase))
                                            uploadSuccess = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteMessageToFile(ItemID + "-" + ex.Message);
                                        WriteMessageToFile("File upload failed, " + ex.Message,
                                            System.Diagnostics.TraceEventType.Error);
                                    }

                                    if (uploadSuccess)
                                    {
                                        string original = lstUploaded.accepted[0].original;
                                        string thumb = lstUploaded.accepted[0].thumb;
                                        string large = lstUploaded.accepted[0].large;

                                        //3.	Get small and medium images (POST)
                                        string resizeImageURL =
                                            "https://adminv2.1stdibs.com/image/ajax/ajax_dealer_resize_images";
                                        NameValueCollection resizeParams = new NameValueCollection();
                                        resizeParams.Add("images[image1][web_url]", lstUploaded.accepted[0].large);
                                        byte[] responseResizeBytes = client.UploadValues(resizeImageURL, resizeParams);
                                        //login
                                        string responseResize = Encoding.UTF8.GetString(responseResizeBytes);
                                        ResizedImage imgResized = ResizedImage.GetResizedImage(responseResize);

                                        string medium = imgResized.image1.medium;
                                        string small = imgResized.image1.small;
                                        //4.	Save the newly updated image sequences (PUT)
                                        dynamic images = null;
                                        if (curItem.images != null)
                                        {
                                            images = curItem.images;
                                        }
                                        else
                                        {
                                            images = new List<Image>();
                                        }


                                        dynamic FirstImage = curItem.images[0];

                                        if (FirstImage == null)
                                            throw new Exception("First image not found");

                                        FirstImage.large = large;
                                        FirstImage.thumb = thumb;
                                        FirstImage.medium = medium;
                                        FirstImage.small = small;

                                        try
                                        {
                                            string saveSequenceURL = string.Format(AppSettings.SaveSequenceURL, detectedVertical, ItemID,
                                                userToken);

                                            JsonReader jsonReader =
                                                new JsonTextReader(new StringReader(bkpCurrentSequenceJSON))
                                                {
                                                    DateParseHandling = DateParseHandling.None
                                                };
                                            dynamic rss = JObject.Load(jsonReader);
                                            var imagesX = (from p in (JArray)curItem.images
												where string.IsNullOrWhiteSpace((string)p["large"]) == false 
												select p).ToArray();

                                            dynamic resultItem = null;
                                            switch (Vertical)
                                            {
                                                case "fashion":
                                                    dynamic tmp = rss.result.fashionItem ?? rss.result.jewelryItem;
                                                    resultItem = tmp.images;
                                                    break;
                                                case "jewelry":
                                                    resultItem = rss.result.jewelryItem.images;
                                                    break;
                                            }

                                            if (
                                                !string.IsNullOrWhiteSpace(
                                                    Convert.ToString(
                                                        curItem.pricing.initialPriceCurrencies)))
                                                resultItem.pricing.initialPriceCurrencies =
                                                    (JObject)
                                                        JToken.FromObject(
                                                            curItem.pricing
                                                                .initialPriceCurrencies);

                                            if (
                                                !string.IsNullOrWhiteSpace(
                                                    Convert.ToString(
                                                        curItem.pricing.amountCurrencies)))
  resultItem.pricing.amountCurrencies =(JObject)JToken.FromObject(curItem.pricing.amountCurrencies);


        resultItem = (JArray)JToken.FromObject(imagesX);

        var count = resultItem.Count;
 resultItem[count - 1].original = Convert.ToString(lstUploaded.accepted[0].original);
                                     var json = JsonConvert.SerializeObject(curItem,
                                                Newtonsoft.Json.Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                            NullValueHandling = NullValueHandling.Ignore

       client = new WebClientX(cookieJar, "application/json; charset=UTF-8");

                                            //Issue 403 forbidden
       byte[] changeSequenceByte = client.UploadData(saveSequenceURL, "PATCH", Encoding.UTF8.GetBytes(json));
       string changeSequenceResponse = Encoding.UTF8.GetString(changeSequenceByte);

                                            //backup updated sequence
                      string bkpUpdatedSequence = JsonConvert.SerializeObject(rss,
                                                Newtonsoft.Json.Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore
                                                });

                                                                                        successfullyProcessed++;
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteMessageToFile(ItemID + "-" + ex.Message,
                                                System.Diagnostics.TraceEventType.Error);
                                            WriteMessageToFile("Unexpected error, " + ex.Message,
                                                System.Diagnostics.TraceEventType.Error);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteMessageToFile(ItemID + "-" + ex.Message, System.Diagnostics.TraceEventType.Error);
                                    WriteMessageToFile("Unexpected error, " + ex.Message, System.Diagnostics.TraceEventType.Error);
                                }
                            }
                            else
                            {
                                WriteMessageToFile("Abort uploading, item already uploaded", System.Diagnostics.TraceEventType.Error);
                            }
                        }
                        else
                        {
                            WriteMessageToFile("Item Key not found, Please check selected vertical", System.Diagnostics.TraceEventType.Error);
                        }
                    }
                }
                else
                {
                    WriteMessageToFile("No records to upload", System.Diagnostics.TraceEventType.Error);
                }
            }
            else
            {
                WriteMessageToFile("Login failed", System.Diagnostics.TraceEventType.Error);
                throw new Exception("Login failed");
            }
            return successfullyProcessed;
        }

private void MoveImagesNext(int max, CurrentSequence curSequence)
{
    for (int i = max - 1; i >= 0; i--)
    {
        Image obj = curSequence.result.furnitureItem.images[i + 1];
        obj = curSequence.result.furnitureItem.images[i];
        obj.position = i + 2;
    }
}

private void MovePropertiesNext(int max, Item_Details item_Details)
{

    for (int i = max; i > 1; i--)
    {
        Type objType = item_Details.GetType();
        PropertyInfo pi = objType.GetProperty("image" + i + "_original");//image1_original
        PropertyInfo piPrevious = objType.GetProperty("image" + (i - 1) + "_original");
        pi.SetValue(item_Details, piPrevious.GetValue(item_Details, null), null);

        PropertyInfo pi2 = objType.GetProperty("image" + i + "_l");//image1_l
        PropertyInfo pi2Previous = objType.GetProperty("image" + (i - 1) + "_l");
        pi2.SetValue(item_Details, pi2Previous.GetValue(item_Details, null), null);

        PropertyInfo pi3 = objType.GetProperty("image" + i + "_t");//image1_t
        PropertyInfo pi3Previous = objType.GetProperty("image" + (i - 1) + "_t");
        pi3.SetValue(item_Details, pi3Previous.GetValue(item_Details, null), null);
    }
}

// Reference Classes
// Reference Classes
// Reference Classes
public class Status
{
    public string productStatus { get; set; }
    public string contactForPrice { get; set; }
    public string locked { get; set; }
    public string posted { get; set; }
    public string newListing { get; set; }
    public string inSaturdaySale { get; set; }
    public string saveReleaseLater { get; set; }
}

public class Material
{
    public string description { get; set; }
    public string restricted { get; set; }
}

public class CustomMaterial
{
    public string restricted { get; set; }
    public string description { get; set; }
}

public class PublishOptions
{
    public string dibs { get; set; }
    public string featured { get; set; }
    public string nydc { get; set; }
    public string storefront { get; set; }
    public string workInProgress { get; set; }
    public string listedFrenchItem { get; set; }
    public string addText { get; set; }
    public string queuedForPosting { get; set; }
    public string rejected { get; set; }
    public string approved { get; set; }
    public string onHold { get; set; }
    public string deleted { get; set; }
    public string sold { get; set; }
    public string photoRepair { get; set; }
    public string previouslySold { get; set; }
    public string unpublished { get; set; }
}

public class Seller
{
    public string id { get; set; }
    public string status { get; set; }
}

public class Store
{
    public int id { get; set; }
}

public class Style
{
    public string name { get; set; }
    public string attribute { get; set; }
}

public class Classification
{
    public Style style { get; set; }
    public string countryOfOrigin { get; set; }
    public List<string> categories { get; set; }
    public string creationDate { get; set; }
    public string period { get; set; }
}

public class Measurement
{
    public string unit { get; set; }
    public string width { get; set; }
    public string depth { get; set; }
    public string height { get; set; }
    public Weight weight { get; set; }
    public string volume { get; set; }
    public string shape { get; set; }
}

public class Condition
{
    public string state { get; set; }
    public string damageLosses { get; set; }
    public string damageLight { get; set; }
    public string damageSound { get; set; }
    public string damageFading { get; set; }
}

public class Image
{
    public int position { get; set; }
    public string thumb { get; set; }
    public string small { get; set; }
    public string medium { get; set; }
    public string large { get; set; }
}

public class Pricing
{
    public string currency { get; set; }
    public double lowestPrice { get; set; }
    public double amount { get; set; }
    public string pricePerPiece { get; set; }
    public string negotiable { get; set; }
    public string hidePrice { get; set; }
    public string bargain { get; set; }
    public string qualifyForBargain { get; set; }
    public int initialPrice { get; set; }
    public InitPriceCur initialPriceCurrencies { get; set; }
    public AmountCur amountCurrencies { get; set; }
}

public class InitPriceCur
{
    public double PND { get; set; }
    public double EUR { get; set; }
    public double USD { get; set; }
}
public class AmountCur
{
    public double PND { get; set; }
    public double EUR { get; set; }
    public double USD { get; set; }
}
public class ReturnPolicy
{
}

public class FurnitureItem
{
    public string id { get; set; }
    public string vertical { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string dealerReference { get; set; }
    public string dibsReference { get; set; }
    public string dateAdded { get; set; }
    public string dateSold { get; set; }
    public string releaseDate { get; set; }
    public string modifiedDate { get; set; }
    public string uploadType { get; set; }
    public Status status { get; set; }
    public string overrideShipping { get; set; }
    public int pieces { get; set; }
    public Material material { get; set; }
    public CustomMaterial customMaterial { get; set; }
    public PublishOptions publishOptions { get; set; }
    public Seller seller { get; set; }
    public Store store { get; set; }
    public Classification classification { get; set; }
    public Measurement measurement { get; set; }
    public Condition condition { get; set; }
    public List<Image> images { get; set; }
    public Pricing pricing { get; set; }
    public ReturnPolicy returnPolicy { get; set; }
}

public class Result
{
    public FurnitureItem furnitureItem { get; set; }
}

public class CurrentSequence
{
    public int httpCode { get; set; }
    public string message { get; set; }
    public Result result { get; set; }
}
// Variable values -
// <add key="UploadURL" value="https://adminv2.1stdibs.com/internal/image-upload/" />
//         <add key="ImageUploadURL" value="https://adminv2.1stdibs.com/image/ajax/dealer_image_upload?seller_id={0}" />
//         <add key="GetSequenceURL" value="https://adminv2.1stdibs.com/soa/inventory/3.1/{0}/item/{1}?userToken={2}" />
//         <add key="SaveSequenceURL" value="https://adminv2.1stdibs.com/soa/inventory/3.1/{0}/item/{1}?fields=images&amp;userToken={2}" />


