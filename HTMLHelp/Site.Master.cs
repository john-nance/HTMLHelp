using ISOManager.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ISOManager
{
    public partial class Site : MasterPage
    {

        public string SearchText
        {
            get
            {
                return tbSearch.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((FormsHelper.CurrentUserGUID == null) || (FormsHelper.CurrentUserGUID == Guid.Empty))
            {
                LogOut();
            }

            if (!Page.IsPostBack)
            {
                SetSecurity();
                HighlightMenu();
                SetConfigValues();
            }
        }

        private void SetSecurity()
        {
            UserClass thisUser = StaticLookups.GetUserFromGUID(FormsHelper.CurrentUserGUID);

            foreach (MenuItem mi in menuMain.Items)
            {
                string securityRule = mi.Value.ToLower();
                if (securityRule == "all")
                    continue;

                string[] AccessList = securityRule.Split(',');
                bool allowedAccess = false;
                foreach (string roleAccess in AccessList)
                {
                    if (thisUser.HasRoleCode(roleAccess))
                    {
                        allowedAccess = true;
                        break;
                    }
                }

                if (!allowedAccess)
                {
                    mi.Enabled = false;
                    mi.Selectable = false;
                }

            }
        }

        private void SetConfigValues()
        {
            if (string.IsNullOrEmpty(Settings.AppRoot))
            {
                Settings.AppRoot=Server.MapPath("~/");
            }
            if (string.IsNullOrEmpty(Settings.SharedDrive))
            {
                Settings.SharedDrive = Server.MapPath("~/ISOSharedDrive/");
            }
        }

        public void LogOut()
        {
            Uri OrigPath = Request.Url;
            Session.Abandon();
            Session.Contents.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));


            string LoginPath = ResolveClientUrl("~/Login.aspx?ReturnURL=" + OrigPath.ToString());
            Response.Redirect(LoginPath, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void HighlightMenu()
        {
            string thisPage = GetASPXName(Context.Request.Path);
            foreach (MenuItem m in menuMain.Items)
            {
                string menuPage = GetASPXName(m.NavigateUrl);
                if ((menuPage!=string.Empty) && (menuPage==thisPage))
                {
                    if (m.Enabled)
                        m.Selected = true;
                }
            }
        }

        private string GetASPXName(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string ASPXName = path;
            int posSlash = path.LastIndexOf('/');

            if ((posSlash >= 0) && (path.Length > posSlash + 1))
                ASPXName = path.Substring(posSlash + 1);

            return ASPXName;
        }

        public void ClearMessages()
        {
            errorMessage.Text = string.Empty;
            statusMessage.Text = string.Empty;
            pnlMessages.CssClass = "MessagePanel Hidden";
            tbMessageShown.Text = "1";
            upMessages.Update();
        }

        public void ShowMessage(string Message)
        {
            errorMessage.Text = string.Empty;
            statusMessage.Text = Message;
            pnlMessages.CssClass = "MessagePanel Shown";
            tbMessageShown.Text = "0";
            upMessages.Update();
        }

        public void ShowError(string ErrorMessage)
        {
            
            errorMessage.Text = ErrorMessage;
            statusMessage.Text = string.Empty;
            pnlMessages.CssClass = "MessagePanel Shown";
            tbMessageShown.Text = "0";
            upMessages.Update();
        }

        public bool IsMessageShown()
        {
            bool isShown = true;

            if (tbMessageShown.Text == "0")
                isShown = false;

            return isShown;
        }

        

        
    }
}