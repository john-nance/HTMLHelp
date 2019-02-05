# pureHelp

This is a tiny ASP.Net/C# Project which is embarrassingly simple - but it was useful for me so why not share it?

I needed a simple Help system for an application.  No problem writing content I just needed a menu system wrapped around it.

The idea is simple, Markdown/HTML help in a folder structure which is mapped to a help menu.

## Help Structure

In the Docs folder build the folder structure you want to see in the menu.  

Couple of simple rules:
* Folders beginning with a minus are hidden (I always have images in a -images folder)
* Folders can begin with nn_ to set their display order, otherwise alphabetical
* Content can be HTML or Markdown with an htm, html or md extensions
* Default.htm is the default content when clicking on the folder name, so not shown on the menu
* AnyThingElse.xxx is shown as a menu item without the extension
* Any underscores are shown as spaces in the menu so a folder called "The_Topic" will be seen as "The Topic"

Sample content is included in the code under Docs.

## Help Content

Just plain HTML or MarkDown files.  The pages are rendered inside the main page so make sure images are relative to the top of the web structure.

Links within the help system use a linkTo javascript function with the name of the help page as shown in the menu.

### HTML

For images use
    <img src= "Docs/-images/image_name.jpg" ..

or to link to another page in the help system use

    <a href="#" onclick="linkTo('Menu Name')">Click Here</a>
or
    <a href="javascript:linkTo('Menu Name')" >Click Here</a>

### MarkDown

For images in the help system use

    ![the image](Docs/-images/image_name.png "The Image")

## Security and Warnings

To get the above links working we are disabling some security checks.  Help content is not checked or verified so you must be 100% certain that the content is safe and under your control.

## Styling

The default styling HTML 5 grids.  The stylesheet is forms.css.  It uses css variables to make it easier to change colours.  This means it will work in Chrome, Firefox, Edge and Safari but not in IE.  The only issue for IE is the styling so you'll need to remove grids and variables.

## MIT License

Feel free to use it any way you like, no restrictions.

If you come up with improvements, then I'd love to hear about them.





