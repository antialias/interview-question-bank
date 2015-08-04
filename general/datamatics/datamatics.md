# Datamatic integration

As of August 2015, the below flow diagram describes our understanding of Datamatics's workflow.

[image tk]

## front-end URLs

Below is a list of URLs they hit on the front-end to scrape our site in order to download data and images as well as upload data and images.

**IMPORTANT** any change to the schemes, hosts, paths OR HTML in any of these files could cause their automated systems to fail.

### download

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

See `datamatics_scraping_source.cs` for the full source code of their download scraping tool (it's a doozy).

### upload
