
using HeyRed.MarkdownSharp;
using pureHelp.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace pureHelp
{
    public partial class Default : System.Web.UI.Page
    {
        private const string BasePath = "~/Docs/";
        private const string DefaultPage = "default";

        private List<ContentClass> HelpContent
        {
            get
            {
                if (Session["Content"]==null)
                {
                    Session["Content"] = new List<ContentClass>();
                }
                return (List<ContentClass>)Session["Content"];
            }
            set
            {
                Session["Content"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath(BasePath));
                HelpContent = new List<ContentClass>();
                PopulateIndex(rootInfo, null);

                tvFolders.Nodes[0].Selected = true;
                ShowContent(tvFolders.Nodes[0]);
                GetActionInURL();
            }
        }

        private void PopulateIndex(DirectoryInfo dirInfo, TreeNode treeNode)
        {
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                // Hidden Folder, add for refernce but don't show
                if (directory.Name.Substring(0, 1) == "-")
                {
                    HelpContent.Add(new ContentClass(directory.Name, directory.Name, false, true));
                    continue;
                }

                TreeNode directoryNode = new TreeNode
                {
                    Text = CleanFileName(directory.Name),
                    Value = directory.FullName,
                     
                };

                ContentClass newContent = new ContentClass(directoryNode, true);
                HelpContent.Add(newContent);

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
                        newContent.FilePath = directoryNode.Value;
                        continue;
                    }

                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = CleanFileName(file.Name),
                        Value = file.FullName
                    };
                    directoryNode.ChildNodes.Add(fileNode);
                    HelpContent.Add(new ContentClass(fileNode, false));
                }

                PopulateIndex(directory, directoryNode);
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
                if (File.Exists(FilePath+".md"))
                {
                    FilePath = FilePath + ".md";
                    IsMarkDown = true;
                }
                else
                {
                    if (File.Exists(FilePath + ".htm"))
                    {
                        FilePath = FilePath + ".htm";
                        IsHTML = true;
                    }
                    else
                    {
                        if (File.Exists(FilePath + ".html"))
                        {
                            FilePath = FilePath + ".html";
                            IsHTML = true;
                        }
                    }
                }
            }

            if (File.Exists(FilePath))
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    string content = sr.ReadToEnd();
                    if (IsMarkDown)
                    {

                        Markdown md = new Markdown();
                        content = md.Transform(content) + "<br/>";
                    }
                    page_HTML.Text = content;
                }
            }
        }

        private string ReadContent(string FilePath)
        {
            string content = string.Empty;

            string extension = Path.GetExtension(FilePath).Replace(".", string.Empty);
            bool IsMarkDown = (extension == "md");
            bool IsHTML = (extension == "htm") || (extension == "html");
            bool IsDoc = IsMarkDown || IsHTML;

            if (!IsDoc)
            {
                FilePath = Path.Combine(FilePath, DefaultPage + ".md");
            }

            
            if (File.Exists(FilePath))
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    content = sr.ReadToEnd();
                }
            }
            return content;
        }

        public void tvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode thisNode = tvFolders.SelectedNode;
            ShowContent(thisNode);
        }

        private void CheckPages()
        {
            page_HTML.Text = "<h1>Checking</h1>";
            string Root = Server.MapPath(BasePath);

            foreach (ContentClass c in HelpContent)
            {
                page_HTML.Text += "<br/>Checking " + c.FilePath.Replace(Root, string.Empty);
                if (c.IsVisible)
                {
                    string content = ReadContent(c.FilePath);
                    if (content != string.Empty)
                    {
                        evaluateContent(content);
                    }
                    else
                    {
                        page_HTML.Text += " ... no content, skipping";
                    }
                }
                else
                {
                    page_HTML.Text += " ... not visible, skipping";
                }
            }
        }

        private void evaluateContent(string content)
        {
            string linkSearch = @"linkTo\('([^\']*)'\)";

            char singleQuote = (char) 39;
            char doubleQuote = (char) 34; 
            linkSearch=linkSearch.Replace(singleQuote, doubleQuote);
            Regex link = new Regex(linkSearch);
            foreach (Match m in Regex.Matches(content,linkSearch))
            {
                page_HTML.Text += "<br/>---- Match " + m.Captures[0].ToString();
                if (m.Groups.Count==2)
                {
                    Group g = m.Groups[1];
                    string PageName = g.ToString();
                    PageName = PageName.Replace("_", " ").Replace("%20", " ");

                    page_HTML.Text += " >> " + PageName;
                    if (foundPage(PageName))
                    {
                        page_HTML.Text += "  -- OK ";
                    }
                    else
                    {
                        page_HTML.Text += "<span class='error'>****** MISSING *****</span>";
                    }
                }


                string s = m.Result("\\1");
            }
        }

        private bool foundPage(string pageName)
        {
            bool result = false;

            foreach (ContentClass c in HelpContent)
            {
                if (c.LinkName == pageName)
                {
                    result = true;
                }
            }
            return result;
        }

        private void GetActionInURL()
        {
            HttpContext myContext = HttpContext.Current;
            string action = myContext.Request.Params["action"];
            if (!string.IsNullOrWhiteSpace(action))
            {
                DoAction(action);
            }
        }

        private void DoAction(string action)
        {
            action = action.ToLower().Trim();
            if (action == string.Empty)
                return;

            if (action=="checklinks")
            {
                CheckPages();
                return;
            }

            action = action.Replace("_", " ");

            string PageRequest = string.Empty;

            ContentClass foundContent = null;
            foreach (ContentClass c in HelpContent)
            {
                if ((c.LinkName.ToLower()==action)  && (c.IsVisible))
                {
                    foundContent = c;
                    break;
                }
            }
            if (foundContent != null)
            {
                TreeNode thisNode = tvFolders.FindNode(foundContent.Node.ValuePath);
                if (thisNode!=null)
                    thisNode.Selected = true;
                Display(foundContent.FilePath);
            }

        }


    }
}