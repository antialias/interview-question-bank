## Kegbot
Kegbot keeps track of our beverages. You can visit kegbot here: http://kegbot.1stdibs.com/

## How does it work?

### components
* raspberry pi - Runs the kegbot server (https://github.com/Kegbot/kegberry, python) and holds the kegbot database (mysql). It serves the kegbot API and web admin
* Flow meters - are hooked up to the beer lines. They measure how much beer is poured out of the lines
* Kegboard Pro Mini - are connected via rj-45 cable to the flow meter and via USB to the raspberry pis. https://www.kegbot.org/docs/kbpm-owners-manual/
* card reader - halp?

## How can we get the card reader to work?
Our office ID badges are not compatible with out of the box kegbot infrastructure. The kegbot server does not have an api call to "assign" a drink to someone, only an admin function. New drinks can be `POST`ed to the kegbot server with a user token from a card reader.

There is a USB card reader on @robrichard's desk that works with our ID cards. It is a HID device that you can read using https://github.com/emilyrose/hidstream.

You can read data from the flow meters using https://github.com/robrichard/node-kegboard

Ideally I think we should move the kegbot server to a real server in the office and build a daemon process to listen to events from both the card reader and the flow meters to create a drink and send it to the kegbot server. This is more scalable then running the server on the raspberry pi since we can then hook the wine kegs up to the same kegbot server.

Looking for volunteers to work on this!
