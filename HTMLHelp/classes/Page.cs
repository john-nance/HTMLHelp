using System.Web.UI;

namespace pureHelp.classes
{
    public class Page : System.Web.UI.Page
    {

        /* Overridden page class to force session and viewstate are saved on the server and not the client */
        PageStatePersister _pers;
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                if (_pers == null)
                    _pers = new SessionPageStatePersister(this);
                return _pers;
            }
        }
    }
}