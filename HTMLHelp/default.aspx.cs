
//using HeyRed.MarkdownSharp;
using Markdig;
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
    public partial class Default : pureHelp.classes.Page
    {


        private bool ResetMenu
        {
            get
            {
                if (Session["ResetMenu"] == null)
                {
                    Session["ResetMenu"] = true;
                }
                return (bool)Session["ResetMenu"];
            }
            set
            {
                Session["ResetMenu"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Settings.NoCache)  // Reload the content from the file folders every time if NoCache set to true in pureHelp.config
                    ContentCache.HelpFolderContent.Clear();

                // Load the cache if needed
                if ((ContentCache.HelpFolderContent is null) || (ContentCache.HelpFolderContent.Count==0))
                {
                    ContentCache.LoadContent(Server.MapPath(Settings.HelpFolder));
                }

                PopulateTreeView();

                SetHelpHeader();
                if (ResetMenu)
                {
                    tvFolders.ExpandDepth = 1;
                    ResetMenu = false;
                }
                
                tvFolders.Nodes[0].Selected = true;
                ShowContent(tvFolders.Nodes[0]);
                GetActionInURL();
            }
        }

        private void PopulateTreeView()
        {
            foreach (ContentClass c in ContentCache.HelpFolderContent)
            {
                if (c.IsVisible == false) continue;

                TreeNode newNode = new TreeNode(c.LinkName, c.NodeID.ToString());

                bool AddedOK = false;

                if (c.ParentNodeID>-1)
                {
                    ContentClass parentFolder = ContentCache.HelpFolderContent.Find(x => x.NodeID == c.ParentNodeID);
                    if (parentFolder!=null)
                    {
                        TreeNode parentTreeNode = tvFolders.FindNode(parentFolder.NodePath);
                        if (parentTreeNode!=null)
                        {
                            parentTreeNode.ChildNodes.Add(newNode);
                            AddedOK = true;
                        }
                    }
                }

                if (!AddedOK)
                {
                    tvFolders.Nodes.Add(newNode);
                }
            }
        }

        private void ShowContent(TreeNode tn)
        {
            ExpandParents(tn);

            int nValue = -1;
            if (int.TryParse(tn.Value, out nValue))
            {
                ContentClass foundContent = ContentCache.HelpFolderContent.Find(x => x.NodeID == nValue);
                if (foundContent!=null)
                {
                    Display(foundContent.FilePath);
                }
            }
        }

        private void Display(string FilePath)
        {
            string extension = Path.GetExtension(FilePath).Replace(".", string.Empty);
            bool IsMarkDown = (extension == "md");
            bool IsHTML = (extension == "htm") || (extension == "html");
            bool IsDoc = IsMarkDown || IsHTML;

            if (!IsDoc)
            {
                FilePath = Path.Combine(FilePath, Settings.DefaultPageName);
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

                        //Markdown md = new Markdown();
                        //content = md.Transform(content) + "<br/>";
                       var newPipe = new  MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                        content = Markdown.ToHtml(content, newPipe);
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
                FilePath = Path.Combine(FilePath, Settings.DefaultPageName + ".md");
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

        private void SetHelpHeader()
        {
            imgMasterHeader.ImageUrl = Settings.HelpIcon;
            lblMasterHeader.Text = Settings.HelpName;
        }

        public void tvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode thisNode = tvFolders.SelectedNode;
            ShowContent(thisNode);
        }

        private void ShowLinks()
        {
            page_HTML.Text = "<h1>Links</h1>";
            string Root = Server.MapPath(Settings.HelpFolder);
            string linkTemplate = "[{0}](javascript:linkTo(\"{1}\")) </br/>";

            foreach (ContentClass c in ContentCache.HelpFolderContent)
            {
                if (c.IsVisible)
                {
                    string pageName = c.LinkName;
                    string cleanName = pageName.Replace(" ", "_");
                    page_HTML.Text += string.Format(linkTemplate, pageName, cleanName);
                }
            }
        }

        private void CheckPages()
        {
            page_HTML.Text = "<h1>Checking</h1>";
            string Root = Server.MapPath(Settings.HelpFolder);

            foreach (ContentClass c in ContentCache.HelpFolderContent)
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
                        page_HTML.Text += " ... no links found, skipping";
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

            foreach (ContentClass c in ContentCache.HelpFolderContent)
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

            if (action == "clearcache")
            {
                ClearCache();
                return;
            }

            if (action == "showlinks")
            {
                ShowLinks();
                return;
            }

            action = action.Replace("_", " ");

            string PageRequest = string.Empty;

            ContentClass foundContent = null;
            foreach (ContentClass c in ContentCache.HelpFolderContent)
            {
                if ((c.LinkName.ToLower()==action)  && (c.IsVisible))
                {
                    foundContent = c;
                    break;
                }
            }
            if (foundContent != null)
            {
                TreeNode thisNode = tvFolders.FindNode(foundContent.NodePath);
                if (thisNode != null)
                {
                    thisNode.Selected = true;
                    ExpandParents(thisNode);
                }
                Display(foundContent.FilePath);
            }

        }

        private void ClearCache()
        {
            ContentCache.LoadContent(Server.MapPath(Settings.HelpFolder));
            tvFolders.Nodes.Clear();

            PopulateTreeView();
            SetHelpHeader();
            tvFolders.ExpandDepth = 1;
            ResetMenu = false;

            tvFolders.Nodes[0].Selected = true;
            ShowContent(tvFolders.Nodes[0]);
        }

        private void ExpandParents(TreeNode thisNode)
        {
            if ((thisNode.Parent != null) && (thisNode.Parent.Expanded != true))
            {
                thisNode.Parent.Expand();
                ExpandParents(thisNode.Parent);
            }
        }

    }
}