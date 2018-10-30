using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Departments_Transportation_ATM_Index : ATMPage
{
    protected override void LoadATMPage()
    {
        Response.Redirect("/Apps/ATM/Payroll/Forms/Index.aspx");
    }
}