public class FirstDibsRecords : DataTable
{
	#region Columns
	private string _id = "S_No";
	private string _ref = "Reference";
	private string _item = "ItemID";
	private string _url = "URL";
	private string _notes = "Notes";
	private string _title = "Title";
	private string _numberofitems = "NumberOfItems";
	private string _dealernotes = "DealerNotes";
	#endregion
	public FirstDibsRecords()
	{
		this.Columns.Add(_id, typeof(string));
		this.Columns.Add(_ref, typeof(string));
		this.Columns.Add(_item, typeof(string));
		this.Columns.Add(_title, typeof(string));
		this.Columns.Add(_url, typeof(string));
		this.Columns.Add(_notes, typeof(string));
		this.Columns.Add(_dealernotes, typeof(string));
		this.Columns.Add(_numberofitems,typeof(string));
	}

	public void Add(Product record)
	{
		DataRow curDataRow = this.NewRow();
		curDataRow[_id] = this.Rows.Count + 1;
		curDataRow[_ref] = record.Ref;
		curDataRow[_item] = record.Item;
		curDataRow[_title] = record.Title;
		curDataRow[_url] = record.Url;
		curDataRow[_notes] = record.Notes;
		curDataRow[_dealernotes] = record.DealerNotes;
		curDataRow[_numberofitems] = record.NumberOfRecords;
		this.Rows.Add(curDataRow);
	}


public FirstDibsRecords DownloadImages(out Dictionary<string, string> UploadUrls)
        {
            UploadUrls = new Dictionary<string, string>();
            try
            {
                CookieContainer cookieJar = new CookieContainer();
                WebClientX client = new WebClientX(cookieJar);

//_login_url = "https://admin.1stdibs.com/citysearch-administration/photo_processing/returnlogin.php" 
                client.Headers["Referer"] = _login_url;

                NameValueCollection postData = new NameValueCollection();
                postData.Add("email", UserName); //example – india1@1stdibsindia.com
                postData.Add("password", Password); //provided password for india1 account
                postData.Add("do-login", "1");

//_login_url_topost = "https://adminv2.1stdibs.com/login/internal"
				byte[] responseBytes = client.UploadValues(_login_url_topost, postData); 
                string response = Encoding.UTF8.GetString(responseBytes);

                CQ loggedInDom = response;
                CQ passwordbox = loggedInDom.Find("[type=\"password\"][name=\"password\"]");
                if (passwordbox.Length == 0)
                {

//_post_login_url = "https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php"
                    //new response
					responseBytes = client.DownloadData(_post_login_url); 
                    response = Encoding.UTF8.GetString(responseBytes);

                    loggedInDom = response;
                    //login success
                    FirstDibsRecords collectedData = new FirstDibsRecords();

                    //make total rows equal to pagesize
                    CQ pages = loggedInDom.Find("td[valign=\"bottom\"][height=\"30px\"][align=\"center\"][colspan=\"3\"]");
                    if (!string.IsNullOrWhiteSpace(pages.Html()))//blank = no record exists
                    {
                        _pageSize = pages.Html().Substring(0, pages.Html().IndexOf("<br>")).Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

// Photoshop_Pending = "https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php?status=-32&start=0&rows=" + _pageSize + "&proc_user_view=" + UserName

                        CQ dom = client.DownloadString(Photoshop_Pending);
                        CQ dataContainer = dom["table"].Find("[width=\"90%\"]")[0].Cq();
                        CQ rows = dataContainer.Find(">tbody>tr");

                        foreach (var row in rows.Elements)
                        {
                            CQ Hr = row.Cq().Find("hr[style=\"border:1px solid black\"]");
                            if (Hr.Length == 0)
                            {
                                CQ curRow = row.Cq().Find(">td>table>tbody>tr");
                                if (curRow.Length == 3)
                                {
                                    CQ data = curRow[0].Cq();//~REF, ~TITLE
                                    string REF_HTML = data.Find(">td")[0].Cq().Html();
                                    string REF_ID = REF_HTML.Substring(0, REF_HTML.IndexOf("<br>"));

                                    CQ title_span = data.Find(">td")[1].Cq().Find("span[id]");
                                    string ItemID = "";
                                    string TITLE = "";
                                    string number_of_items = "";
                                    foreach (var prod_detail in title_span.Elements)
                                    {
                                        if (prod_detail.Attributes["id"].StartsWith("dibs_I_item#"))
                                        {
                                            ItemID = prod_detail.Attributes["id"].Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                                            CQ title_span_elm = prod_detail.Cq().Find(">b")[0].Cq();
                                            TITLE = title_span_elm.Html();
                                        }

                                        if (prod_detail.Attributes["id"].StartsWith("dibs_I_num_item#"))
                                        {
                                            number_of_items = prod_detail.InnerHTML.Trim();
                                        }
                                    }

                                    string siteName = System.Configuration.ConfigurationManager.AppSettings["SiteName"]; 
//SitemName = https://admin.1stdibs.com
                                    if (string.IsNullOrWhiteSpace(siteName))
                                        siteName = "https://www.1stdibs.com";

                                    string UploadURL = "";//image upload url
                                    var URLDom2 = data.Find(">td")[2].Cq().Find("a");
                                    if (URLDom2.Length > 1)
                                    {
                                        UploadURL = URLDom2[0].Cq().Attr("href");
                                    }

                                    string URL = "";//~URL
                                    var URLDom = data.Find(">td")[2].Cq().Find("a");
                                    if (URLDom.Length > 1)
                                    {
                                        URL = URLDom[1].Cq().Attr("href");
                                        if (!URL.StartsWith("https://"))
                                            URL = siteName + URL;

                                    }

                                    CQ notes = curRow[2].Cq().Find("textarea");//~NOTES
                                    string Photo_notes = "";
                                    string DealerNotes = "";
                                    foreach (var note in notes.Elements)
                                    {
                                        if (note.Attributes["id"].StartsWith("dealer_photo_note_"))
                                        {
                                            //CQ notes_textarea_elm = note.Cq().Html();
                                            Photo_notes = note.InnerHTML;
                                        }
                                        if (note.Attributes["id"].StartsWith("dealer_notes_"))
                                        {
                                            //CQ notes_textarea_elm = note.Cq().Html();
                                            DealerNotes = note.InnerHTML;
                                        }
                                    }

                                    UploadUrls.Add(ItemID, UploadURL);
                                    Product record = new Product(UploadURL);
                                    record.Ref = REF_ID;
                                    record.Item = ItemID;
                                    record.Url = URL;
                                    record.Notes = Photo_notes;

                                    record.Title = TITLE;
                                    record.NumberOfRecords = number_of_items;
                                    record.DealerNotes = DealerNotes;
                                    collectedData.Add(record);
                                }
                            }
                        }
                    }
                    else
                    {
                        WriteMessageToFile("No records found.", System.Diagnostics.TraceEventType.Error);
                    }

                    return collectedData;
                }
                else
                {
                    throw new Exception("Login failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	

public class Product
    {
        [CsvField(Name = "S_No")]
        public int Id
        {
            get;
            set;
        }

        [CsvField(Name = "Reference")]
        public string Ref
        {
            get;
            set;
        }

        [CsvField(Name = "Itemid")]
        public string Item
        {
            get;
            set;
        }

        [CsvField(Name = "Url")]
        public string Url
        {
            get;
            set;
        }

        [CsvField(Name = "Notes")]
        public string Notes
        {
            get;
            set;
        }

        //additional fields to extract
        public string Title
        {
            get;
            set;
        }

        public string NumberOfRecords
        {
            get;
            set;
        }

        public string DealerNotes
        {
            get;
            set;
        }

        private string _UploadURL = "";
        public override string ToString()
        {
            return _UploadURL;
        }

        public Product() { 
        
        }

        public Product(string url)
        {
            _UploadURL = url;
        }
    }