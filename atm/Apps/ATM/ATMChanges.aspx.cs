using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_ATMChanges : BasePage
{
    protected override void LoadBasePage()
    {
        //PageId = 39;
        xmlChanges.DataFile = AppSettings.GetAppSetting("intranet", "atmchanges", "file");
    }
}