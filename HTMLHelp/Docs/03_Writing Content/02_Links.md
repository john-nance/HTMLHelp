# Linking to other pages

You can link using standard Markdown but there is a small script called "linkTo" which makes it easier.

### Markdown

In Markdown use the following code for a link to anther page.

```
   [Link Description](javascript:linkTo("Help_Page_Name"))
```

Where "Help Page Name" is the name of the pages as seen in the help menu on the left. Use underscores instead of spaces (Markdown doesn't like spaces in urls).

To link to exterm web pages, simple use normal Markdown links with the full url instead of the javascript function.

### Benefits of LinkTo

The LinkTo script finds help pages by their name.  This means you can safely re-organise your content, and change the sort order, without breaking links.

If you use LinkTo, then make sure every page has a unique name - pureHelp will always return the first matching page.

See also [Check Links](javascript:linkTo("Links")) for the utility to check all links in your help content.