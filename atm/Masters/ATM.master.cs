using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using SygmaFramework;
using System.Xml;
using atm;

public partial class Masters_ATM : System.Web.UI.MasterPage
{
    private string gUserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            //litJQueryScript.Text = string.Format("<script src='{0}Scripts/jquery-1.8.2.min.js' type='text/javascript'></script>", ATMPage.AppPath);
            hlSiteChanges.NavigateUrl = string.Format("javascript: OpenWindow('{0}Apps/ATM/ATMChanges.aspx', 475, 700, 1,0,0)", ATMPage.AppPath); // last 2 params should be 0 for new window and disable url
        }
    }
    private void SetPageVariables()
    {
        gUserName = WebCommon.GetUserName(Context);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GetFileFullPath());
        XmlNode node = xmlDoc.SelectSingleNode("//Changes/Change/date");
        string lastUpdateDate = node.InnerText;
        litLastUpdateDate.Text = lastUpdateDate;  //this is the last updated date
    }

    private string GetFileFullPath()
    {
        XMLConfig config = new XMLConfig();
        return config.GetAppSetting("intranet", "atmchanges", "file");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.Page.Title != "")
            this.Page.Title = string.Concat("ATM - ", this.Page.Title);
        else
            this.Page.Title = "ATM";
    }

    private ATMPage gATMPage;
    public ATMPage ATMPage
    {
        get
        {
            try
            {
                if (gATMPage == null)
                    gATMPage = (ATMPage)this.Page;
                return gATMPage;
            }
            catch (Exception exp)
            {
                throw new InvalidATMPageException(exp);
            }
        }
    }

    public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
    {
        if (Request.Url.IsDefaultPort)
        {
            return string.Format("http{0}://{1}{2}",
                        (Request.IsSecureConnection) ? "s" : "",
                        Request.Url.Host,
                        Page.ResolveUrl(relativeUrl)
                    );
        }

        return string.Format("http{0}://{1}:{3}{2}",
            (Request.IsSecureConnection) ? "s" : "",
            Request.Url.Host,
            Page.ResolveUrl(relativeUrl),
            Request.Url.Port
        );
    }


    //private Database gATMDB;

    //public Database ATMDB {
    //  get {
    //    if (gATMDB == null)
    //      gATMDB = new Database("ATM");
    //    return gATMDB;
    //  }
    //}
}
