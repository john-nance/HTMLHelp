# HTMLHelp

This is a tiny ASP.Net/C# Project which is embarrassingly simple - but it was useful for me so why not share it?

I needed a simple way Help system for an application.  No problem writing HTML content I just needed a simple menu system wrapped around it.

The idea is simple, HTML help in a folder structure which is mapped to a menu.

## Help Structure

In the Docs folder build the folder structure you want to see in the menu.  

Couple of simple rules:
* Folders beginning with a minus are hidden (I always have images in a -images folder)
* Folders can begin with nn_ to set their display order, otherwise alphabetical
* Default.htm is the default content when clicking on the folder name, so not shown on the menu
* AnyThingElse.htm is shown as a menu item without the ".htm"
* Any underscores are shown as spaces in the menu so a folder called "The_Topic" will be seen as "The Topic"

## Help Content

Just plain HTML files.  The pages are rendered inside the main page so make sure images are relative to the top of the web structure, 

i.e.
<img src= "Docs/-images/image_name.jpg" ..

To reference another page use

<a href="#" onclick="linkTo('Menu Name')">Click Here</a>

The it finds the right menu and highlights it in the tree correctly




