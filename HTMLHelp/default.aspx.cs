
using HeyRed.MarkdownSharp;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HTMLHelp
{
    public partial class Default : System.Web.UI.Page
    {
        private const string BasePath = "~/Docs/";
        private const string DefaultPage = "default";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath(BasePath));
                PopulateTreeView(rootInfo, null);

                tvFolders.Nodes[0].Selected = true;
                ShowContent(tvFolders.Nodes[0]);
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
                    if (CleanFileName(file.Name).ToLower() == DefaultPage.ToLower())
                    {
                        directoryNode.Value = file.FullName;
                        continue;
                    }

                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = CleanFileName(file.Name),
                        Value = file.FullName
                    };
                    directoryNode.ChildNodes.Add(fileNode);
                }

                PopulateTreeView(directory, directoryNode);
                
            }
        }

        private string CleanFileName(string filename)
        {
            string CleanName = filename.Replace(".htm", string.Empty).Replace(".md",string.Empty).Replace("_", " ");
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
            Display(Server.MapPath(url));
        }

        private void ShowContent(TreeNode tn)
        {
            Display(tn.Value);
        }

        private void Display(string FilePath)
        {
            string extension = Path.GetExtension(FilePath).Replace(".", string.Empty);
            bool IsMarkDown = (extension == "md");
            bool IsHTML = (extension == "htm") || (extension == "html");
            bool IsDoc = IsMarkDown || IsHTML;

            if (!IsDoc)
            {
                FilePath = Path.Combine(FilePath, DefaultPage);
            }

            using (StreamReader sr = new StreamReader(FilePath))
            {
                string content = sr.ReadToEnd();
                if (IsMarkDown)
                {
                    Markdown md = new Markdown();
                    content = md.Transform(content);
                }
                page_HTML.Text = content;
            }
        }

        public void tvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode thisNode = tvFolders.SelectedNode;
            ShowContent(thisNode);
        }
    }
}