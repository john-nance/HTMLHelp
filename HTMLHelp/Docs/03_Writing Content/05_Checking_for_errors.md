# Checking for Link Errors

It's fairly easy to spot broken images, but links are harder to test - unless you click on every link.

There is a utility called Check Links to help.

Run the CheckLinks command on your pages using the special URL

http://*{normal help site}*/default.aspx**?action=CheckLinks**


The utility scans every page and seaches for the "linkTo" function.  It then checks if it can find the linked page.

If it doesn't find a page it shows the message  * * MISSING * *

![Check Links Screen Results](Docs/-images/pureHelp/CheckLinks.png)

