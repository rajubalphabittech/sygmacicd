using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SygmaFramework;


/// <summary>
/// Summary description for Employees
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Employees : System.Web.Services.WebService {

	public Employees() {

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	[WebMethod]
	public string[] SearchEmployees(string prefixText, int count, string contextKey) {
		DataView dv = WebCommon.RunSP(new Database("ATM"), "up_p_searchEmployees", prefixText);
		List<string> employees = new List<string>(dv.Count);
		int i = 0;
		foreach (DataRowView row in dv) {
			if (i++ < count)
				employees.Add(row["EmployeeFullName"].ToString());
			else
				break;
		}
		return employees.ToArray();
	}

}
