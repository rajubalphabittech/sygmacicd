using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Data.Sql;
using System.Data.OleDb;
using atm;
using SygmaIntranet;

/// <summary>
/// Summary description for ExcelDataObject
/// </summary>
[DataObject(true)]
public class ExcelDataObject : BasePage
{
    /// <summary>
    /// 
    /// </summary>
    private DataSet _storeNumberLookup;

    protected override void LoadBasePage()
    {
        // No implementation - not neccesary
    }

    public ExcelDataObject()
    {
        // Default Constructor - not used
    }

    /// <summary>
    /// 
    /// </summary>
    public ExcelDataObject(string sp, List<object> listOfParameters)
	{
        this._storeNumberLookup = HttpContext.Current.Session["StoreNumberLookup"] as DataSet;

        if (this._storeNumberLookup == null)
        {
            this._storeNumberLookup = new DataSet();
            _storeNumberLookup = GetData(sp, listOfParameters) as DataSet;
            HttpContext.Current.Session["StoreNumberLookup"] = this._storeNumberLookup;
        }
	}

    public DataSet GetData(string sp, List<object> listOfParameters)
    {
        DataSet ds = new DataSet();
        ds = IntranetDB.GetDataSet(sp, listOfParameters);
        return ds;
    }

    /// <summary>
    /// 
    /// </summary>
    private DataTable StoreNumberLookupTable
    {
        get 
        {
            if (this._storeNumberLookup == null)
            {
                this._storeNumberLookup = HttpContext.Current.Session["StoreNumberLookup"] as DataSet;
            }
            return this._storeNumberLookup.Tables[0];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [DataObjectMethod(DataObjectMethodType.Select)]
    public DataView Select()
    {
        //this.StoreNumberLookupTable.DefaultView.Sort = "Billto ASC";
        return this.StoreNumberLookupTable.DefaultView;
    }
}
