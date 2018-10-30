using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SygmaFramework;

/// <summary>
/// Summary description for SSN
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SSN : System.Web.Services.WebService {

    public SSN () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetSSN(string prefixText, int count)
    {
        DataView dv = WebCommon.RunSP(new Database("ATM"), "up_searchSSN", prefixText);
        List<string> empSSN = new List<string>(dv.Count);
        int i = 0;
        foreach (DataRowView row in dv)
        {
            if (i++ < count)
                empSSN.Add(row["Social Security"].ToString());
            else
                break;
        }

        return empSSN.ToArray();
    }
    
}
