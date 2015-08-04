# Datamatic integration

As of August 2015, the below flow diagram describes our understanding of Datamatics's workflow.

![flow diagram][flow-diagram]

## front-end URLs

Below is a list of URLs they hit on the front-end to scrape our site in order to download data and images as well as upload data and images.

**IMPORTANT** any change to the schemes, hosts, paths OR HTML in any of these files could cause their automated systems to fail.

### download

This is the data that they scrape from `/citysearch-administration/photo_processing/i_view.php`:
* 1stdibs reference number
* item ID
* item title
* item URL
* photo processor notes
* dealer notes
* number of items
* upload URL (this is derived from the 'original' URL link on the i_view page; see below)

See this diagram for more details on where each of those data points comes from.

Here are the URLs (with schemes, hosts, paths):

##### `https://admin.1stdibs.com/citysearch-administration/photo_processing/returnlogin.php`

Note: this is set as the referrer on their client for some reason.

##### `https://adminv2.1stdibs.com/login/internal`

There is a POST of this data to this URL in order to get logged in:

| field    | value                         |
|----------|-------------------------------|
| email    | e.g.: india1@1stdibsindia.com |
| password | e.g.: india123                |
| do-login | 1                             |

##### `https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php`

This is the URL that they then load and start scraping.

##### `https://admin.1stdibs.com/citysearch-administration/photo_processing/i_view.php?status=-32&start=0&rows= + _pageSize + &proc_user_view= + UserName`

`_pageSize` is derived from scaping the previous i_view.php page and `Username` is the email login name (sans the @ and the host).

See [datamatics_scraping_source.cs](datamatics_scraping_source.cs) for the full source code of their download scraping tool (it's a doozy).

### upload tk




[flow-diagram]: https://github.com/1stdibs/necrodibsicon/blob/master/general/datamatics/datamatics-flow.jpg?raw=true "datamatics flow"
