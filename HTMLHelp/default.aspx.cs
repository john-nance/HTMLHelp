
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HTMLHelp
{
    public partial class Default : System.Web.UI.Page
    {
        private const string BasePath = "~/Docs/";
        private const string DefaultPage = "default.htm";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath(BasePath));
                PopulateTreeView(rootInfo, null);
                ShowContent(BasePath + DefaultPage);
            }
        }

        private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
        {
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {

                if (directory.Name.Substring(0, 1) == "-")
                    continue;

                TreeNode directoryNode = new TreeNode
                {
                    Text = CleanFileName(directory.Name),
                    Value = directory.FullName
                    //NavigateUrl= BasePath + directory.ToString()
                };

                if (treeNode == null)
                {
                    //If Root Node, add to TreeView.
                    tvFolders.Nodes.Add(directoryNode);
                }
                else
                {
                    //If Child Node, add to Parent Node.
                    treeNode.ChildNodes.Add(directoryNode);
                }

                //Get all files in the Directory.
                foreach (FileInfo file in directory.GetFiles())
                {
                    if (file.Name == DefaultPage)
                        continue;


                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = CleanFileName(file.Name),
                        Value = file.FullName,
                        Target = "_blank"
                        
                        //NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()
                    };
                    directoryNode.ChildNodes.Add(fileNode);
                }

                PopulateTreeView(directory, directoryNode);
                
            }
        }

        private string CleanFileName(string filename)
        {
            string CleanName = filename.Replace(".htm", string.Empty).Replace("_", " ");
            string first3 = CleanName.Substring(0, 3);
            Regex rx = new Regex(@"[0-9][0-9] ");
            MatchCollection matches = rx.Matches(first3);
            if ((matches.Count==1) && (CleanName.Length>4))
            {
                CleanName = CleanName.Substring(3);
            }
            return CleanName;
        }

        private void ShowContent(string url)
        {
            string FilePath = Server.MapPath(url);
            using (StreamReader sr = new StreamReader(FilePath))
            {
                page_HTML.Text = sr.ReadToEnd();
            }
        }

        private void ShowContent(TreeNode tn)
        {
            string FilePath = tn.Value;
            if (!FilePath.Contains(".htm"))
            {
                FilePath = Path.Combine(FilePath, DefaultPage);
            }

            using (StreamReader sr = new StreamReader(FilePath))
            {
                page_HTML.Text = sr.ReadToEnd();
            }
        }

        public void tvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode thisNode = tvFolders.SelectedNode;
            ShowContent(thisNode);
        }
    }
}