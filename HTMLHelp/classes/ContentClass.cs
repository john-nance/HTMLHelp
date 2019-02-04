using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTMLHelp.classes
{
    public class ContentClass
    {
        public string DisplayName { get; set; }
        public string LinkName { get; set; }
        public string ActualName { get; set; }
        public bool IsVisible { get; set; }
        public bool IsFolder { get; set; }

        public ContentClass(string _DisplayName, string _LinkName, bool _IsVisible, bool _IsFolder)
        {
            DisplayName = _DisplayName;
            LinkName = _LinkName;
            IsVisible = _IsVisible;
            IsFolder = _IsFolder;
        }


    }
}