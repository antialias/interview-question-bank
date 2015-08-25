# CMS

The CMS stores data that make up the content of pages on 1stdibs.com that's interfaced with via a variety of HTTP calls.  v2 and v3 have been proposed, but only v1 has been built and is used in production (as of 8/25/2015).

This document attempts to clean up [the old wiki's documentation](https://sites.google.com/a/1stdibs.com/wiki/home/engineering/services-documentation/cms-service?pli=1).

Version 2 of the CMS is currently on Draft 3.  The draft is available [in a Google Doc](https://docs.google.com/document/d/11WP-0Mn8GV5Wm0xoOk0VuLX8YGjq6C0CCnUVsT8pZUE/edit).

## Endpoints

| environment | URL |
| ----------- | --- |
| dev | [http://hearteater:8080/cms-service/v1/](http://hearteater:8080/cms-service/v1/) |
| qa | http://qa.1stdibs.com/soa/cms-service/v1/ |
| stage | http://stage.1stdibs.com/soa/cms-service/v1/ |
| prod | http://1stdibs.com/soa/cms-service/v1/ |

Note that the `dev` endpoint does not include `/soa/` in it's path.  All paths will be shown with the `/soa/` present in their specifications, but this should be omitted for `dev` paths.  This can be handled in 1stdibs-admin-v2 via the `serviceEnds` module and in 1stdibs.com through `Dibs_Cms_Model.php`.

## Browser Tools

- **HTTP Rest client** for manually firing HTTP requests.
  - [Chrome](https://chrome.google.com/webstore/detail/advanced-rest-client/hgmloofddffdnphfgcellkdfbfbjeloo?hl=en-US&utm_source=ARC)
  - [Firefox](https://addons.mozilla.org/en-us/firefox/addon/rest-easy/)
- **JSON formatter** to be able to read the responses from the CMS.
  - [Chrome](https://chrome.google.com/webstore/detail/jsonview/chklaanhfefbnpoihckbnefhakgolnmc?hl=en)
  - [Safari](https://github.com/acrogenesis/jsonview-safari)
  - [Firefox](https://addons.mozilla.org/en-Us/firefox/addon/jsonview/)

## Page Types

A page type follows a similar JSON structure for all pages of that type.  It is namespaced using colons (`:`).

To create a new page type, fire an action_newpage on the desired endpoint.

For each page type, there can be one live page.  This can be accessed via the action_getlive action.

### Lifecycle

A page type has a typical life cycle:

- [Create the page type](#create-a-new-page-type)
- [Create a new page of the page type](#create-a-new-page)
- [Set that page live](#set-live-page)
- [Duplicate the live page](#duplicate-a-page)
- [Make edits to the duplicate page](#update-a-page)
- [Set the duplicate page as live](#set-live-page)

### Directories

Directory calls return a list of pages or page types, depending on the data included in the call.  These calls have no body unless specified.

#### Return all page types

Call the root of the CMS to return a listing of all page types for a given `endpoint`.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/
```

Sample Request:
```
http://qa/soa/cms-service/v1/
```

Sample Response:
```JSON
[
    {
        "id":1,
        "page_name":"homepage"
    },
    {
        "id":2,
        "page_name":"collections"
    }
]
```

#### Return headers for all pages of a given page type

Call the CMS with a `pageName` on a given `endpoint` but no arguments or actions to receive a listing of all the pages of a given type.  Will not include the modules present on each page.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageName]/
```

Sample Request:
```
http://qa/soa/cms-service/v1/homepage/
```

Sample Response:
```JSON
[
    {
        "id": 14666,
        "type": "homepage",
        "created_datetime": "Thu Jan 09 2014 09:46:52 -0500",
        "modified_datetime": "Wed Jun 17 2015 15:19:59 -0400",
        "release_datetime": "Thu Jan 09 2014 09:46:52 -0500",
        "note": "01082014 | Lighting emergency! JK",
        "status": "scheduled",
        "version": ""
    },
    {
        "id": 16638,
        "type": "homepage",
        "created_datetime": "Mon Feb 03 2014 09:21:07 -0500",
        "modified_datetime": "Fri Jan 31 2014 17:41:06 -0500",
        "release_datetime": "Fri Jan 31 2014 17:42:10 -0500",
        "note": "02012014 | Saturday Shopping with Tommy Clements and Sat Sale - SR",
        "status": "ended",
        "version": null
    }
]
```

## Return the full information on a page ID

Pass the `pageName` and the `rootId` of the page to receive the headers and modules for that page ID on a given `endpoint`.  This is useful for reviewing changes to a page or to preview changes without pushing them live.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageName]?rootId=[rootId]
```

Sample Request:
```
http://qa/soa/cms-service/v1/homepage?rootId=14666
```

Sample Response:
```JSON
{
    "id": 14666,
    "type": "homepage",
    "created_datetime": "Thu Jan 09 2014 09:46:52 -0500",
    "modified_datetime": "Wed Jun 17 2015 15:19:59 -0400",
    "release_datetime": "Thu Jan 09 2014 09:46:52 -0500",
    "note": "01082014 | Lighting emergency! JK",
    "status": "scheduled",
    "version": "",
    "modules": [
        {
            "visible": "true",
            "id": 11896072,
            "type": "locationsList",
            "title": "Shop By Country",
            "items": [
                {
                    "visible": "true",
                    "id": 11896092,
                    "title": "USA",
                    "link_url": null,
                    "dibs_L_id": "1",
                    "items": [
                        {
                            "visible": "true",
                            "id": 11896112,
                            "link_url": "/furniture_search.php?location=new_york_city",
                            "title": "New York City",
                            "items": {
                                "visible": true,
                                "id": 11896122
                            }
                        },
                            {
                            "visible": "true",
                            "id": 11896132,
                            "title": "Los Angeles",
                            "link_url": "/furniture_search.php?location=los_angeles",
                            "items": {
                                "visible": true,
                                "id": 11896142
                            }
                        }
                    ]
                }
            ]
        }
    ]
}
```

## Actions

#### Get Live Page

Get the current live page for a given `pageName` on a given `endpoint`.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageName]/action_getlive
```

Sample Request:
```
http://qa/soa/cms-service/v1/homepage/action_getlive
```

Sample Response:
```JSON
{
    "id": 66692,
    "type": "homepage",
    "created_datetime": "Wed Jun 17 2015 07:48:47 -0400",
    "modified_datetime": "Wed Jun 17 2015 07:48:40 -0400",
    "release_datetime": "Wed Jun 17 2015 07:48:47 -0400",
    "note": "0617115 | Art Basel, Watches in the circles - OE",
    "status": "live",
    "version": "",
    "modules": [
        {
            "visible": "true",
            "id": 11879662,
            "type": "locationsList",
            "title": "Shop By Country",
            "items": [
                {
                    "visible": "true",
                    "id": 11879682,
                    "title": "USA",
                    "dibs_L_id": "1",
                    "link_url": null,
                    "items": [
                        {
                            "visible": "true",
                            "id": 11879702,
                            "title": "New York City",
                            "link_url": "/furniture_search.php?location=new_york_city",
                            "items": {
                                "visible": true,
                                "id": 11879712
                            }
                        }
                    ]
                }
            ]
        }
    ]
}
```


#### Set Live Page

Set the given `rootId` as the live page for a given `pageName` on a given `endpoint`.  The current live page will be assigned an `ended` status.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:

```
http://[endpoint]/soa/cms-service/v1/[pageName]/action_setlive?rootId=[rootId]
```

Sample Request:

```
http://qa/soa/cms-service/v1/homepage/action_setlive?rootId=66692
```

Sample Response:

```JSON
{
    "id": 66692,
    "type": "homepage",
    "created_datetime": "Tue Aug 25 2015 13:39:55 -0400",
    "modified_datetime": "Wed Jun 17 2015 07:48:40 -0400",
    "release_datetime": "Tue Aug 25 2015 13:39:55 -0400",
    "note": "0617115 | Art Basel, Watches in the circles - OE",
    "status": "live",
    "version": "",
    "action_status": "success",
    "action_message": "page set to live",
    "action": "setlive"
}
```

#### Create a New Page

Create a new page of the given `pageName` on a given `endpoint`.  A JSON payload is required, with a top-level `modules` array that contains all page data.  The CMS will automatically fill in all other top-level header elements.

The keys `id` and `visible` are reserved for use by the CMS.

HTTP Verb:
```
POST
```

CRUD Verb:
```
create
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageName]/action_create
```

Sample Request:
```
http://qa/soa/cms-service/v1/homepage/action_create
```

Sample Body:
```JSON
{
    "modules": [
        {
            "data" : "test"
        }
    ]
}
```

#### Update a Page

Update a given `pageId` of the given `pageType` on a given `endpoint` with body content.  Note that the body needs to contain an `id` element whose value matches the `pageId` provided in the HTTP request as well an array of `modules`.

The current `live` page for any page type cannot be updated.

HTTP VERB:
```
PUT
```

CRUD Verb:
```
update
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_update?rootId=[pageId]
```

Sample Request:
```
http://qa/soa/cms-service/v1/global:navigation/action_update?rootId=63693
```

#### Delete a Page

Delete a given `pageId` for a given `pageType` on the given `endpoint`.

The current `live` page for any page type cannot be deleted.

HTTP VERB
```
DELETE
```

CRUD Verb:
```
delete
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_delete?rootId=[pageId]
```

Sample Request:
```
http://qa/soa/cms-service/v1/homepage/action_delete?rootId=82233
```

Sample Response:
```JSON
{
    "id": 82233,
    "action_status": "success",
    "action_message": "delete successful. Full page deleted",
    "action": "delete"
}
```

You can also use `action_delete` to delete a `childId` from a given `pageType` on a given `endpoint`:

```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_delete?childId=[childId]
```

#### Duplicate a Page

Duplicate a given `pageId` for a given `pageType` on a given `endpoint`.  Duplicating a page and setting it live is useful for trying to edit a live page.

HTTP VERB:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_duplicate?rootId=[pageId]
```

Sample Request:
```
http://qa/soa/cms-service/v1/mobile:promo:gal/action_duplicate?rootId=82103
```

Sample Response:
```JSON
{
    "id": 82253,
    "type": "mobile:promo:gal",
    "created_datetime": "Tue Aug 25 2015 14:25:05 -0400",
    "modified_datetime": "Tue Aug 25 2015 14:25:05 -0400",
    "release_datetime": "Tue Aug 25 2015 14:25:05 -0400",
    "note": "test data",
    "status": "scheduled",
    "version": null,
    "action_status": "success",
    "action_message": "duplicate successful",
    "action": "duplicate"
}
```
#### Set Child Element Visibility

Set the `visiblity` property of a given `childId` to a given `visbilityFlag` for a given `pageName` on a given `endpoint`.

`visibilityFlag` must be `true` or `false`.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageName]/action_setvisible?childId=[childId]&visible=[visibilityFlag]
```

Sample Request:
```
http://qa/soa/cms-service/v1/mobile:promo:gal/action_setvisible?childId=14559183&visible=true
```

Sample Response:
```JSON
{
    "id": 82103,
    "type": "mobile:promo:gal",
    "created_datetime": "Tue Aug 25 2015 10:45:02 -0400",
    "modified_datetime": "Tue Aug 25 2015 14:10:06 -0400",
    "release_datetime": "Tue Aug 25 2015 10:45:02 -0400",
    "note": "test data",
    "status": "scheduled",
    "version": "",
    "action_status": "success",
    "action_message": "visible set to true",
    "action": "setvisible",
    "boolean": "true"
}
```

#### Get Headers

Get the headers for a given `pageId` of a given `pageType` on a given `endpoint`.  This data is included in the directory of the `pageType` and when retrieving the full data of the `pageId`.

HTTP Verb:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_getheaders?rootId=[pageId]
```

Sample Request:
```
http://qa/soa/cms-service/v1/mobile:promo:gal/action_getheaders?rootId=82103
```

Sample Response:
```JSON
{
    "id": 82103,
    "type": "mobile:promo:gal",
    "created_datetime": "Tue Aug 25 2015 10:45:02 -0400",
    "modified_datetime": "Tue Aug 25 2015 14:10:06 -0400",
    "release_datetime": "Tue Aug 25 2015 10:45:02 -0400",
    "note": "test data",
    "status": "scheduled",
    "version": ""
}
```
#### Create a New Page Type

Create a new `pageType` on a given `endpoint`.  The page type will have no live page and no pages.

HTTP VERB:
```
GET
```

CRUD Verb:
```
read
```

Specification:
```
http://[endpoint]/soa/cms-service/v1/[pageType]/action_newpage/
```

Sample Request:
```
http://qa/soa/cms-service/v1/mobile:promo:gal/action_newpage
```

Sample Response:
```JSON
{
    "action_status": "success",
    "action_message": "Created new page type - sample_page",
    "action": "newpage"
}
```

#### Other Parameters, Reqeusts, and Actions

[Available in a Google Spreadsheet](https://docs.google.com/a/1stdibs.com/spreadsheet/ccc?key=0Ali6S6rrBfWgdEtfWnBxcXZKQWxMdkFnTjljWlNZcGc)

## Errors

If the CMS service encouters an error, it will return an object containing the status, an error message, and the action that was attempted.

Examples:

```JSON
{
    "action_status": "error",
    "errormessage": "500 - Internal Server Error - Expected a ',' or '}' at character 13 of {"id":60tu31"}]}]}",
    "action": "update"
}
```

```JSON
{
    "action_status": "error",
    "errormessage": "page not found - null",
    "action": "update"
}
```
