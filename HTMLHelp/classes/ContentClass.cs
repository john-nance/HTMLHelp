using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace pureHelp.classes
{

    public class ContentClass
    {
        public string FilePath { get; set; }
        public string LinkName { get; set; }
        public int NodeID { get; set; }
        public int ParentNodeID { get; set; } 
        public string NodePath { get; set; }

        public bool IsVisible { get; set; }
        public bool IsFolder { get; set; }
        public TreeNode Node { get; set; }

        public ContentClass(string _FilePath, string _LinkName, bool _IsVisible, bool _IsFolder, int _nodeID, int _parentNodeID)
        {
            FilePath = _FilePath;
            LinkName = _LinkName;
            Node = null;
            IsVisible = _IsVisible;
            IsFolder = _IsFolder;
            NodeID = _nodeID;
            ParentNodeID = _parentNodeID;
            NodePath = string.Empty;
        }

        public ContentClass(TreeNode tn, bool _IsFolder)
        {
            FilePath = tn.Value;
            LinkName = tn.Text;
            Node = tn;
            IsVisible = true;
            IsFolder = _IsFolder;
        }
    }
}