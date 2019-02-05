# Writing Content

pureHelp content are text files in [MarkDown](javascript:linkTo("Markdown")) or [HTML](javascript:linkTo("HTML")).

Build your help structure as folders under the Docs folder.

Every folder becomes a topic in the help index.  If a folder contains other folders or other file, it can be opened and closed in the help index.

Every folder must have a default.md (or default.html) which is the help  shown when you click on the folder topic.

You add other content files under a folder.

Files can be prefixed with a two digit number and an underscore to control the display order (otherwise its alphabetical).


### Files and Folders

Here is the file structure of this help with folders and files

![Folders and Files](Docs/-images/pureHelp/FileStructure.png)  

Numbers are used to control the display order.  

The default.md is the default content for the folder so they are not seen in the index.

The folder and file names define the help index structure.


### Resulting Help Index

Here is the resulting help index

![Resulting Help Index](Docs/-images/pureHelp/FileStructureDisplayed.png)

