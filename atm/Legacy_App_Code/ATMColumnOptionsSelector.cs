using ATM.Payroll;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using atm;

/// <summary>
/// Summary description for ATMColumnOptionsSelector
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ATMColumnOptionsSelector : System.Web.Services.WebService
{
    public ATMColumnOptionsSelector()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SaveColumnOptionsData(string userName, string columnsData, string pageName)
    {
        BasePage.RunNonQueryFromStatic("ATM", "up_p_saveProfileCustom", userName, pageName, columnsData);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetColumnOptionsData(string userName, string pageName)
    {
        string[,] columns = null;
        SetAvailableColumnValues(ref columns, pageName);

        // Start Selected Columns
        string selectedColumnsJson = string.Empty;
        DataTable dtSelectedCols = new DataTable();
        dtSelectedCols.Columns.Add("ID");
        dtSelectedCols.Columns.Add("Width");
        dtSelectedCols.Columns.Add("ColumnIdentifier");
        dtSelectedCols.Columns.Add("Visible");
        dtSelectedCols.Columns.Add("DisplayOrder");
        DataSet dsSelectedColumns = BasePage.GetDataSetFromStatic("ATM", "up_p_getProfileCustom", userName, pageName);
        if ((dsSelectedColumns.Tables[0].Rows != null) && (dsSelectedColumns.Tables[0].Rows.Count > 0))
        {
            dtSelectedCols = JsonConvert.DeserializeObject<DataSet>(dsSelectedColumns.Tables[0].Rows[0]["Value"].ToString()).Tables[0];

            foreach (DataRowView row in dtSelectedCols.DefaultView)
            {
                for (int i = 0; i < columns.GetLength(0); i++)
                {
                    if (columns[i, 0] == Convert.ToString(row["ID"]))
                    {
                        row["ColumnIdentifier"] = columns[i, 1];
                    }
                }
            }
            selectedColumnsJson = "{\"Columns\":" + JsonConvert.SerializeObject(dtSelectedCols) + "}";
        }
        else
        {
            for (int i = 0; i < columns.GetLength(0); i++)
            {
                dtSelectedCols.Rows.Add(new object[] { columns[i, 0], "0", columns[i, 1], "true", i + 1 });
            }
            selectedColumnsJson = "{\"Columns\":" + JsonConvert.SerializeObject(dtSelectedCols) + "}";
        }
        // End Selected Columns

        // Start Available Columns
        string availableColumnsJson = string.Empty;
        DataTable dtAvailableCols = new DataTable();
        dtAvailableCols.Columns.Add("ID");
        dtAvailableCols.Columns.Add("ColumnIdentifier");

        if ((dtSelectedCols.Rows.Count > 0))
        {
            for (int i = 0; i < columns.GetLength(0); i++)
            {
                dtAvailableCols.Rows.Add(new object[] { columns[i, 0], columns[i, 1] });
            }
        }

        var temp = dtAvailableCols.Copy();
        foreach (DataRow row in temp.Rows)
        {
            if ((dtSelectedCols.Rows.Count > 0) && (dtAvailableCols.Rows.Count > 0))
            {
                DataRow selectedDataTableRow = dtSelectedCols.Select("ID=" + row["ID"]).FirstOrDefault();
                DataRow availableDataTableRow = dtAvailableCols.Select("ID=" + row["ID"]).FirstOrDefault();

                if (selectedDataTableRow != null && availableDataTableRow != null)
                {
                    availableDataTableRow.Delete();
                }
            }
        }
        availableColumnsJson = "{\"AvailableColumns\":" + JsonConvert.SerializeObject(dtAvailableCols) + "}";
        // End Available Columns

        string finalColumnsJson = JsonConvert.SerializeObject(new object[] { JsonConvert.DeserializeObject(selectedColumnsJson), JsonConvert.DeserializeObject(availableColumnsJson) });

        return finalColumnsJson;
    }

    private void SetAvailableColumnValues(ref string[,] columns, string pageName)
    {
        switch (pageName)
        {
            case "PROFILEFORMGRID":
                columns = PayrollCommon.FormPageGridColumns;
                break;
            case "MANAGEROUTE":
                columns = PayrollCommon.ManageRouteGridColumns;
                break;
            case "MANAGEVEHICLESTRAILERS":
                columns = PayrollCommon.ManageVehiclesTrailersColumns;
                break;
            case "MANAGEEMPLOYEES":
                columns = PayrollCommon.ManageEmployeesColumns;
                break;
        }
    }
}
