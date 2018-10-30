using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SygmaFramework;
using System.Data;
using System.Web.Script.Services;

/// <summary>
/// Summary description for FormVendors
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FormVendors : System.Web.Services.WebService {

    public FormVendors () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
		[ScriptMethod]
		public string[] GetVendors(string prefixText, int count, string contextKey) {
			Database db = new Database("Intranet");
			DataView dv = db.GetDataView("up_ip_getFormsVendors", prefixText, contextKey);
			List<string> vendors = new List<string>(dv.Count);
			int i = 0;
			foreach (DataRowView row in dv) {
				vendors.Add(row["VendorDisplay"].ToString());
				if (i++ > count)
					break;
			}
			return vendors.ToArray();
    }
    
}
