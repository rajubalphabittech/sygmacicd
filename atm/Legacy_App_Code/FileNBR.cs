using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SygmaFramework;


/// <summary>
/// Summary description for FileNBR
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FileNBR : System.Web.Services.WebService {

    public FileNBR () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        
    }

    [WebMethod]
    public string[] GetFileNBR(string prefixText, int count) {
        DataView dv = WebCommon.RunSP(new Database("ATM"), "up_searchFileNBR", prefixText);
        List<string> empFileNBR = new List<string>(dv.Count);
        int i = 0;
        foreach (DataRowView row in dv)
        {
            if (i++ < count)
                empFileNBR.Add(row["File_NBR"].ToString());
            else
                break;
        }
        
        return empFileNBR.ToArray();
    }
}
