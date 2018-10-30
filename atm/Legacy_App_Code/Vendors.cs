using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SygmaFramework;
using System.Data;

/// <summary>
/// Summary description for Vendors
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Vendors : System.Web.Services.WebService {

    public Vendors () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld(string prefixText, int count, string contextKey) {
      Database db = new Database("Intranet");
			DataView dv = db.GetDataView("up_getVendors", prefixText, count);
			List<string> vendors = new List<string>(dv.Count);
			foreach (DataRowView row in dv) {
				vendors.Add(row["VendorName"].ToString());
			}

			return "Hello World";
    }
    
}
