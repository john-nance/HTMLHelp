using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace pureHelp.classes
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

        public static bool NoCache //Name of the default pages without any extension
        {
            get
            {
                string value = WebConfigurationManager.AppSettings["NoCache"];
                bool nCache = false;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.Trim().ToLower().Substring(1,1);
                    if ((value == "y") || (value == "t") || (value == "1"))
                        nCache = true;
                }
                
                return nCache;
            }
        }
    }
}