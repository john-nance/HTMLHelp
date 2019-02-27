# Advanced Usage

## Context Sensitive Help

You can link to specific help pages using the URL parameter Action

... pureHelp/default.aspx**?Action=Help__Topic**

Help__Topic is the name of a topic in the help menu - replace spaces with underscores.

The help topic isn't case sensitive so help__topic and Help__TOPIC will also work.

This means you can use pureHelp to add context sensitive help to your applications.

See also [Check for errors](javascript:linkTo("Checking_for_errors")) for the utility to check all links in your content.

## Caching the help folders

By default the help folders are scanned once when the application is started.  You can refresh the cache after changes using the clearcache action.

... pureHelp/default.aspx**?Action=ClearCache**

If you are writing a help system or using it in test mode, change the NoCache setting in pureHelp.config to True.

pureHelp doesn't cache page content so any updates to page content will always be seen immediately.

NoCache should be **False** for any production systems.

## Configuration Settings

The name and icon for the help system are configured in pureHelp.config application settings.

You can also change the name of the Help content folder from Docs to your preferred location.  

The Docs folder must be readable by the web server but could be an external file share linked as a virtual folder.

	<appSettings>

	<!-- Name shown on the header -->
	<add key="HelpName" value="pureHelp" />

	<!-- Image shown on the header, use ~ for web application root -->
	<add key="HelpIcon" value="~/images/icons/folderlogo75.png" />

	<!-- web root location of help documents, use ~ for web application root -->
	<add key="HelpFolder" value="~/Docs" />

	<!-- Name of the default pages with no extension (system checks for .md, .htm, .html extensions -->
	<add key="DefaultPageName" value="default" />

	<!-- Set NoCache to true to reload the help Content every time - useful while writing content -->
	<add key="NoCache" value="false" />
  
	</appSettings>

## Styling

pureHelp has one small css file in styles/form.css.

It uses css variables for the colours and css grids so it will not work on Internet Explorer or old browsers.

##  Javascript, tracking and performance

pureHelp uses Javascript to interact with the treeview.  

There are no external javascript libraries, no tracking codes, no advertising.

The page sizes are tiny apart from your images.

On a busy system you can improve performance by turning on IIS caching for the Docs folder and sub folders.

