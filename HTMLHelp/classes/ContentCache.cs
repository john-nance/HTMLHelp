using pureHelp.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace pureHelp.classes
{
    public class ContentCache
    {
        public static List<ContentClass> HelpFolderContent = new List<ContentClass>();
        private static int NodeID = 0;

        public static void LoadContent(string baseFolder)
        {
            HelpFolderContent.Clear();
            NodeID = 0;
            DirectoryInfo BaseDirectory = new DirectoryInfo(baseFolder);
            PopulateFolderContent(BaseDirectory, -1);
            CleanFolderContent();
        }

        private static void PopulateFolderContent(DirectoryInfo dirInfo, int ParentNodeID)
        {
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                // Hidden Folder, add for refernce but don't show
                if (directory.Name.Substring(0, 1) == "-")
                {
                    HelpFolderContent.Add(new ContentClass(directory.Name, directory.Name, false, true, NodeID++, ParentNodeID));
                    continue;
                }

                ContentClass NewContentFolder = new ContentClass(directory.FullName, CleanFileName(directory.Name), true, true, NodeID++, ParentNodeID);
                HelpFolderContent.Add(NewContentFolder);

                //Get all files in the Directory.
                foreach (FileInfo file in directory.GetFiles())
                {
                    if (CleanFileName(file.Name).ToLower() == Settings.DefaultPageName)
                    { 
                        NewContentFolder.FilePath = file.FullName;
                        continue;
                    }

                    //Add each file as Child Node.
                    HelpFolderContent.Add(new ContentClass(file.FullName, CleanFileName(file.Name), true, false,NodeID++, NewContentFolder.NodeID));
                }

                PopulateFolderContent(directory, NewContentFolder.NodeID);
            }
        }

        private static string CleanFileName(string filename)
        {
            string CleanName = filename.Replace(".htm", string.Empty).Replace(".html", string.Empty).Replace(".md", string.Empty).Replace("_", " ");
            string first3 = CleanName.Substring(0, 3);
            Regex rx = new Regex(@"[0-9][0-9] ");
            MatchCollection matches = rx.Matches(first3);
            if ((matches.Count == 1) && (CleanName.Length > 4))
            {
                CleanName = CleanName.Substring(3);
            }
            return CleanName;
        }

        private static void CleanFolderContent()
        {
            HelpFolderContent.Sort((x, y) => string.Compare(x.FilePath.Replace("\\default","\\00_default"), y.FilePath.Replace("\\default", "\\00_default")));
            
            foreach (ContentClass c in HelpFolderContent)
            {
                c.NodePath = c.NodeID.ToString();
                int thisParentID = c.ParentNodeID;
                while (thisParentID > -1)
                {
                    ContentClass foundContent = HelpFolderContent.Find(x => x.NodeID == thisParentID);
                    if (foundContent!=null)
                    {
                        c.NodePath = foundContent.NodeID.ToString() + "/" + c.NodePath;
                        thisParentID = foundContent.ParentNodeID;
                    }
                    else
                    {
                        thisParentID = -1;
                    }
                }
            }
        }

        
    }
}