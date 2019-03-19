# pureHelp

This is a tiny ASP.Net/C# Project which is embarrassingly simple - but it was useful for me so why not share it?

I needed a simple Help system for an application.  No problem writing content I just needed a menu system wrapped around it.

The idea is simple, Markdown and/or HTML help in a folder structure which is mapped to a help index.

![pureHelp](https://github.com/john-nance/HTMLHelp/blob/master/HTMLHelp/Docs/-images/pureHelp/IntroPage.png)

## Help Structure

In the Docs folder, build the folder structure you want to see in the menu.  

Couple of simple rules:
* Folders beginning with a minus are hidden (I always have images in a -images folder)
* Folders can begin with nn_ to set their display order, otherwise alphabetical
* Content can be HTML or Markdown with an htm, html or md extensions
* Default.md (or default.html) is the default content when clicking on the folder name, so not shown on the menu
* AnyThingElse.xxx is shown as a menu item without the extension
* Any underscores are shown as spaces in the menu so a folder called "The_Topic" will be seen as "The Topic"

Sample content is included in the code under Docs giving details on using and writing content and the configuration options.

## Help Content

Just plain HTML or Markdown files.  The pages are rendered inside the main page so make sure images are relative to the top of the web structure.

Links within the help system can use a linkTo javascript function which finds the current link to a named page.  This makes it easier to re-organise content without changing all the links.

### HTML

For images use

```
    <img src= "Docs/-images/image_name.jpg" ..
```

or to link to another page in the help system use

```
   <a href="javascript:linkTo('Menu_Name')">Click Here</a>
```

### MarkDown

For images in the help system use

```
    ![the image](Docs/-images/image_name.png "The Image")
```

To link to another page in the help system use

```
   [Click Here](javascript:linkTo("Menu Name"))
```

## Security and Warnings

To get the above links working, we are disabling some security checks.  Help content is not checked or verified so you must be 100% certain that the content is safe and under your control.

## Styling

The default styling HTML 5 grids.  The stylesheet is forms.css.  It uses css variables to make it easier to change colours.  This means it will work in Chrome, Firefox, Edge and Safari but not in IE.  The only issue for IE is the styling so you'll need to remove grids and variables.

Configuration settins are used for the logo and help title.

## Link Checking

Add **?Action=CheckLinks** to the url to run a link checking utility which check links inside the help system using *linkTo*.

## Cache Control

Configuration settings control if the folder structure is cached for all users.  If cached, then use **?Action=ClearCache** to re-read and cache the folder structure after making changes.  Pages aren't cached so any changes take effect immediately. 

## Url for context sensitive help

Add **?Action=Topic_Name** to the url to link to a specific help page to build context sensitive help into an application.


## MIT License

Feel free to use it any way you like, no restrictions.

If you come up with improvements, then I'd love to hear about them.

