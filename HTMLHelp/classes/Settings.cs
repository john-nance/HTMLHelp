using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HTMLHelp.classes
{
    public class Settings
    {
        public static string HelpName //Name shown at the top of the Help Pages
        {
            get
            {
                string value = WebConfigurationManager.AppSettings["HelpName"];
                if (string.IsNullOrEmpty(value))
                    value = "pureHelp";
                return value;
            }
        }

        public static string HelpIcon //Image shown at the top left of the Help Pages, use ~ for Help Root
        {
            get
            {
                string value = WebConfigurationManager.AppSettings["HelpIcon"];
                if (string.IsNullOrEmpty(value))
                    value = "~/images/icons/folderlogo75.png";
                return value;
            }
        }

        public static string HelpFolder //Image shown at the top left of the Help Pages
        {
            get
            {
                string value = WebConfigurationManager.AppSettings["HelpFolder"];
                if (string.IsNullOrEmpty(value))
                    value = "~/Docs";
                return value;
            }
        }

        public static string DefaultPageName //Name of the default pages without any extension
        {
            get
            {
                string value = WebConfigurationManager.AppSettings["DefaultPageName"];
                if (string.IsNullOrEmpty(value))
                    value = "default";
                return value.ToLower();
            }
        }
    }
}