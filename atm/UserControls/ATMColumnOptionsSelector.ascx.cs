using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ATMColumnOptionsSelector : System.Web.UI.UserControl
{
    public string PageName { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, GetType(), "usercontrolscript", ResolveUrl("/Scripts/UserControls/ATMColumnOptionsSelector.js"));
    }
}