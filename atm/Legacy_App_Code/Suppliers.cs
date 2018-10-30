using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using SygmaFramework;
using System.Data;

/// <summary>
/// Summary description for Suppliers
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Suppliers : System.Web.Services.WebService {
	Database IntranetDB = new Database("intranet");

	public Suppliers() {

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	private DataView GetSuppliers(string searchText) {
		return IntranetDB.GetDataView("up_sd_searchSuppliers", searchText.Trim());
	}

	private string[] GetSupplierArray(string searchText, int column) {
		DataView dv = GetSuppliers(searchText);
		string[] list = new string[dv.Count];
		for (int i = 0; i < dv.Count; i++) {
			list[i] = dv[i][column].ToString();
		}
		return list;
	}

	[WebMethod]
	public string[] GetSupplierIds(string prefixText, int count) {
		return GetSupplierArray(prefixText, 0);
	}

	[WebMethod]
	public string[] GetSupplierNames(string prefixText, int count) {
		return GetSupplierArray(prefixText, 1);
	}

	[WebMethod]
	public string[] GetSuppliers(string prefixText, int count) {
		return GetSupplierArray(prefixText, 2);
	}


	
}


