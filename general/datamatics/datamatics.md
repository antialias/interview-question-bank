# Datamatics integration

As of August 2015, the below flow diagram describes our understanding of Datamatics's workflow.

![flow diagram][flow-diagram]

#### resources
* [download scraper source code (C#)](datamatics_scraping_source.cs)
* [upload source code (C#)](datamatics_upload_source.cs)
* [CQ class](https://github.com/jamietre/CsQuery) (used extensively in abov)
* [Uploading / Approving Items (QA test) doc](https://docs.google.com/presentation/d/1zSVeVcoPI-JFZjyA8qG4cnTNvDJr3ZbQI6sHwAEz6rA/edit#slide=id.g6cc6cafbb_0106)

Below is a list of URLs they hit the front-end to scrape our site in order to download data and images as well as upload data and images.

**IMPORTANT** any change to the schemes, hosts, paths OR HTML in any of these files could cause their automated systems to fail.

## download process

This is the data that they scrape from `/citysearch-administration/photo_processing/i_view.php`:
* 1stdibs reference number
* item ID
* item title
* item URL
* photo processor notes
* dealer notes
* number of items
* upload URL (this is derived from the 'original' URL link on the i_view page; see below)

This diagram from datamatics attempts to make clear from where the data is being scraped.

![i_view page][iview-diagram]

Here are URLs datamatics uses (with schemes, hosts, paths):

* https://admin.1stdibs.com/citysearch-administration/photo_processing/returnlogin.php
	* set as the referrer on their client for some reason

* https://adminv2.1stdibs.com/login/internal 
	* POST data (see below)

* https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php
	* the URL that they then load and start scraping.

* https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php?status=-32&start=0&rows= + _pageSize + &proc_user_view= + UserName
	* `_pageSize` is derived from scaping the previous i_view.php page and `Username` is the email login name (sans the @ and the host).

* https://www.admin.1stdibs.com/archivesE/{0}
	* {0} – Image physical path depends on item ID & Seller ID

See [datamatics_scraping_source.cs](datamatics_scraping_source.cs) for the full source code of their download scraping tool.

https://adminv2.1stdibs.com/login/internal login POST data:

| field    | value                         |
|----------|-------------------------------|
| email    | e.g.: india1@1stdibsindia.com |
| password | e.g.: india123                |
| do-login | 1                             |

++


## upload process

##### front-end URLs
* https://adminv2.1stdibs.com/internal/image-upload/
* https://adminv2.1stdibs.com/image/ajax/dealer_image_upload?seller_id={0}
* https://adminv2.1stdibs.com/image/ajax/ajax_dealer_resize_images
* https://admin.1stdibs.com/citysearch-administration/photo_processing/ajax/i_update_imgstatus.php

##### back-end endpoints
* https://adminv2.1stdibs.com/soa/inventory/3.1/{0}/item/{1}?userToken={2}
* https://adminv2.1stdibs.com/soa/inventory/3.1/{0}/item/{1}?fields=images&userToken={2}

{0} – Vertical, {1} = Item PK, {2} = userToken cookie value

See [upload source code (C#)](datamatics_upload_source.cs) for the full source code of their upload tool.

## Service call documentation

Datamatics provided the documenation below (July 31, 2015).

### 1stDibs Item review Service calls, Status code and schedule details

##### Production calls
User Token = 1698644_a096a87e846763ebbde657ee8589ac2ae6d741348f27517721aa19481a78aea6

* **GET** 
	* http://www.1stdibs.com/soa/inventory/3.1/items?verticals=furniture&csStatus=QUEUED_FOR_POSTING_NL&fields=basic,categoryTree,classification,images,pricing,status,material,condition&orderBy=CREATED_DATE

* **PATCH**  
	* http://www.1stdibs.com/soa/inventory/3.1/furniture/item/{0}?ecomUpdate=N&statusTransition=Y&csStatus=NEW_LISTING&userUpdate=Y&userToken={1}&fields=basic,classification,status,images,material

* Save for Later **PATCH** 
	* http://www.1stdibs.com/soa/inventory/3.1/furniture/item/{0}?ecomUpdate=N&statusTransition=N&saveReleaseLater=Y&userUpdate=Y&userToken={1}&fields=basic,classification,status,images,material

* Item **GET** 
	* http://www.1stdibs.com/soa/inventory/3.1/item/{0}.json?fields=basic,categoryTree,classification,images,pricing,status,material,condition&userToken={1}

* Category **GET**
	* http://www.1stdibs.com/soa/inventory/3.2/category.json?fields=full&userToken={1}

* Creator **GET** 
	* http://www.1stdibs.com/soa/inventory/3.2/creators?vertical=FURNITURE&creatorStatus=APPROVED&startWith&pageStart=1&pageSize=1000&returnTotalResults=Y


##### QA calls

User Token = 1698644_a096a87e846763ebbde657ee8589ac2afac1138b2c148c863aa69487b53c981b

* **GET** 
	* http://www.qa.1stdibs.com/soa/inventory/3.1/items?verticals=furniture&csStatus=QUEUED_FOR_POSTING_NL&fields=basic,categoryTree,classification,images,pricing,status,material,condition&orderBy=CREATED_DATE

* **PATCH** 
	* http://www.qa.1stdibs.com/soa/inventory/3.1/furniture/item/{0}?ecomUpdate=N&statusTransition=Y&csStatus=NEW_LISTING&userUpdate=Y&userToken={1}&fields=basic,classification,status,images,material

* Save for Later **PATCH** 
	* http://www.qa.1stdibs.com/soa/inventory/3.1/furniture/item/{0}?ecomUpdate=N&statusTransition=N&saveReleaseLater=Y&userUpdate=Y&userToken={1}&fields=basic,classification,status,images,material

* Item **GET** 
	* http://www.qa.1stdibs.com/soa/inventory/3.1/item/{0}.json?fields=basic,categoryTree,classification,images,pricing,status,material,condition&userToken={1}

* Category **GET**
	* http://www.qa.1stdibs.com/soa/inventory/3.2/category.json?fields=full&userToken={1}

* Creator **GET**
	* http://www.qa.1stdibs.com/soa/inventory/3.2/creators?vertical=FURNITURE&creatorStatus=APPROVED&startWith&pageStart=1&pageSize=1000&returnTotalResults=Y


### 1stdibs Service Calls

Below, also provided by datamatics:

1. Auto login to 1stdibs workflow with particular accounts user credentials  
2. To get current image sequence & extract seller id (GET) 
	* https://adminv2.1stdibs.com/soa/inventory/3.1/{vertical}/item/{ItemID}?userToken={userToken}

3. Upload image to server (POST)
	* https://adminv2.1stdibs.com/image/ajax/dealer_image_upload?seller_id={sellerId}
	* https://adminv2.1stdibs.com/internal/image-upload/ **In case of Fashion & Jewelry** <- unclear what this means

4.	Get small and medium images (POST)
	* https://adminv2.1stdibs.com/image/ajax/ajax_dealer_resize_images

5.	Save the newly updated image sequences (PUT)
	* https://adminv2.1stdibs.com/soa/inventory/3.1/{Vertical}/item/{ItemID}??fields=images&userToken={userToken}

6.	Update status of the item (POST)
	* https://admin.1stdibs.com/citysearch-administration/photo_processing/ajax/i_update_imgstatus.php



[flow-diagram]: https://github.com/1stdibs/necrodibsicon/blob/master/general/datamatics/datamatics-flow.jpg?raw=true "datamatics flow"
[iview-diagram]: https://github.com/1stdibs/necrodibsicon/blob/master/general/datamatics/i-view-data.png?raw=true "i_view.php"
