using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SygmaFramework;
using Sygmanet;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using atm.web;
using atm.web.helpers;
using SygmaFramework.Spreadsheet;


/// <summary>
/// Summary description for BasePage
/// </summary>
public abstract class BasePage : System.Web.UI.Page {
	protected abstract void LoadBasePage();

	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack)
			LogPageHit();
		LoadBasePage();
		if (WebCommon.IsSiteAdmin(Request))
			Javascript.RegisterStartupScript(this, "MYINFO", string.Format("document.write('<a href=\"javascript: void(0);\" style=\\'color: #ffffff\\' class=\\'invisible\\' accesskey=\\'I\\' TabIndex=\\'-1\\' onFocus=\\'ChangeUser(\"{0}\");this.blur();\\' >i</a>');", UserName), false);
		if (!CancelRegisterCommonJS)
			RegisterCommonJS();
	}

	protected virtual void Page_Unload(object sender, EventArgs e) {
		if (gIntranetDB != null)
			gIntranetDB.CloseConnection();
		if (gWebDB != null)
			gWebDB.CloseConnection();
		if (gAbsenteeismDB != null)
			gAbsenteeismDB.CloseConnection();
		if (gOperationsDB != null)
			gOperationsDB.CloseConnection();
		if (gSafetyDB != null)
			gSafetyDB.CloseConnection();
		if (gATMDB != null)
			gATMDB.CloseConnection();
	}

	private bool gRefreshState;
	private bool gIsRefresh;
	public bool IsRefresh {
		get {
			return gIsRefresh;
		}
	}

	protected override object SaveViewState() {
		Session["__ISREFRESH"] = gRefreshState;
		object[] allStates = new object[2];
		allStates[0] = base.SaveViewState();
		allStates[1] = !gRefreshState;
		return allStates;
	}

	//this only fires after the form submits
	protected override void LoadViewState(object savedState) {
		object[] allStates = (object[])savedState;
		base.LoadViewState(allStates[0]);
		gRefreshState = (bool)allStates[1];
		gIsRefresh = (gRefreshState == ((Session["__ISREFRESH"] != null) ? (bool)Session["__ISREFRESH"] : true)); //if the users sits on the page too long the session can expire.  save to assume they hit refresh
	}

	public void RegisterCommonJS() {
		string includeScriptKey = "Script_File_Common";
		if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), includeScriptKey))
			Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), includeScriptKey, string.Format("{0}Scripts/atmLegacy/Common.js", AppPath));
	}
	public void AddClientVariable(string name, string value) {
		AddClientVariable<string>(name, value);
	}
	public void AddClientVariable<T>(string name, T value) {
		Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Concat(name.ToUpper(), "_VAR"), string.Format("var {0} = '{1}';", name, value), true);
	}

	public string GetControlPostBackCall(Control control) {
		return GetControlPostBackCall(control, true);
	}
	public string GetControlPostBackCall(Control control, bool formatForJSCall) {
		string call = Page.ClientScript.GetPostBackClientHyperlink(control, null);
		if (formatForJSCall)
			call = call.Substring(11).Replace("'", "\\'");
		return call;
	}

	public void RedirectToSelf() {
		RedirectToSelf(null, null);
	}

	public void RedirectToSelf(string qsKey, string qsValue) {
		string url = GetSelfURL(qsKey, qsValue);
		Response.Redirect(url);
	}

	private string GetSelfURL(string qsKey, string qsValue) {
		string url = Request.Url.PathAndQuery;
		if (qsKey != null && qsValue != null)
			url = Web.AddQSValue<string>(url, qsKey, qsValue);
		return url;
	}

	public void RedirectToSelf(string message) {
		RedirectToSelf(message, null, null);
	}

	public void RedirectToSelf(string message, string qsKey, string qsValue) {
		string url = GetSelfURL(qsKey, qsValue);
		this.Javascript.NotifyAndMove(message, url);
	}

	public void GotoInvalidUserPage() {
		WebCommon.GotoInvalidUserPage(this);
	}

	protected DataRowView GetUserInfoFromDB(string userName) {
		DataView dv = IntranetDB.GetDataView("up_getUserInfo", userName);
		if (dv.Count > 0) {
			return dv[0];
		}
		return null;
	}

	private void LogPageHit() {
		IntranetDB.RunNonQuery("up_logPageHit", UserName, Request.AppRelativeCurrentExecutionFilePath, Request.QueryString.ToString());
	}

	public void SendFile(FileInfo content, string contentType) {
		SendFile(content, contentType, false);
	}
	public void SendFile(FileInfo content, string contentType, bool suppressResponseEnd) {
		SendFile(File.ReadAllBytes(content.FullName), content.Name, contentType, suppressResponseEnd);
	}
	public void SendFile(MemoryStream content, string fileName, string contentType, bool suppressResponseEnd) {
		var buffer = new byte[content.Length];
		content.Position = 0;
		content.Read(buffer, 0, buffer.Length);
		SendFile(buffer, fileName, contentType, suppressResponseEnd);
	}
	public void SendFile(byte[] content, string fileName, string contentType, bool suppressResponseEnd) {
		try {
			Response.AddHeader("Content-disposition", string.Format("attachment; filename={0}", fileName));
			Response.ContentType = contentType;
			Response.BinaryWrite(content);
			if (!suppressResponseEnd)
				Response.End();
		} catch (System.Threading.ThreadAbortException) {
			//do nothing.  not really an error
			Server.ClearError();
		} catch (Exception exp) {
			//throw new Exception(string.Format("Sending File. FileFullPath: {0}", content.FullName), exp);
			throw new Exception("Sending File.", exp);
		}
	}

	public string SaveExcelFile(FileUpload fu, string fileNameQualifier, string folder, out string message) {
		if (fu.HasFile && IsValidExcelFile(fu)) {
			message = null;
			return SaveFile(fu, fileNameQualifier, folder, ref message);
		} else {
			message = "Please supply a valid Excel file!";
		}
		return "";
	}

	private string SaveFile(FileUpload fu, string fileNameQualifier, string folder, ref string message) {
		string filesFullPath = Path.Combine(Directory.GetParent(Request.PhysicalPath).FullName, folder);
		if (!Directory.Exists(filesFullPath))
			Directory.CreateDirectory(filesFullPath);
		string fileFullName = Path.Combine(filesFullPath, Common.GetUniqueFileName(fileNameQualifier, Path.GetExtension(fu.FileName)));
		fu.SaveAs(fileFullName);
		if (File.Exists(fileFullName)) {
			return fileFullName;
		} else {
			message = "*** There was an error uploading your file.  Please try it again.  It this persists please contact the Helpdesk. ***";
		}
		return "";
	}



	private bool IsValidExcelFile(FileUpload fu) {
		string fileExtension = Path.GetExtension(fu.FileName);
		return (fileExtension == FileExtensions.EXCEL_9703 || fileExtension == FileExtensions.EXCEL_2007);
	}

	/// <summary>
	/// Gets the FileInfo for a filename.  Adds an index until the fileName does not exist.
	/// </summary>
	/// <param name="fileName">Root name of the file</param>
	/// <returns>The FileInfo</returns>
	public FileInfo GetIndexedFileInfo(string fullFilePath) {
		int i = 1;
		string fullFolderPath = Path.GetDirectoryName(fullFilePath);
		string fileNameWOExt = Path.GetFileNameWithoutExtension(fullFilePath);
		string ext = Path.GetExtension(fullFilePath);
		while (File.Exists(fullFilePath)) {
			string fileName = string.Format("{0}_{1}{2}", fileNameWOExt, i++, ext);
			fullFilePath = Path.Combine(fullFolderPath, fileName);
		}
		return new FileInfo(fullFilePath);
	}

	public void RegisterValidationSummaryMessageBox(ValidationSummary vs) {
		System.Reflection.FieldInfo fi = typeof(Page).GetField("_validated", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		if (fi != null) {
			if ((bool)fi.GetValue(Page)) {
				if (!Page.IsValid) {
					if (vs.ShowMessageBox) {
						System.Text.StringBuilder sb = new System.Text.StringBuilder(vs.HeaderText);
						string sep = " ";
						switch (vs.DisplayMode) {
							case ValidationSummaryDisplayMode.BulletList:
								sep = "\\n-";
								break;
							case ValidationSummaryDisplayMode.List:
								sep = "\\n";
								break;
						}
						foreach (IValidator val in Validators) {
							if (!val.IsValid) {
								sb.Append(sep);
								sb.Append(val.ErrorMessage.Replace("\'", "\\'"));
							}
						}
						Javascript.Notify(sb.ToString(), true);
					}
				}
			}
		}
	}

	private XMLConfig gAppConfig;
	public XMLConfig AppConfig {
		get {
			if (gAppConfig == null)
				gAppConfig = new XMLConfig();
			return gAppConfig;
		}
	}
	private Database gIntranetDB;
	public Database IntranetDB {
		get {
			if (gIntranetDB == null)
				gIntranetDB = new Database("Intranet");
			return gIntranetDB;
		}
	}
	private Database gWebDB;
	public Database WebDB {
		get {
			if (gWebDB == null)
				gWebDB = new Database("WebReports");
			return gWebDB;
		}
	}
	private Database gOperationsDB;
	public Database OperationsDB {
		get {
			if (gOperationsDB == null)
				gOperationsDB = new Database("Operations");
			return gOperationsDB;
		}
	}

	private Database gSafetyDB;
	public Database SafetyDB {
		get {
			if (gSafetyDB == null)
				gSafetyDB = new Database("Safety");
			return gSafetyDB;
		}
	}

	private Database gAbsenteeismDB;

	public Database AbsenteeismDB {
		get {
			if (gAbsenteeismDB == null)
				gAbsenteeismDB = new Database("Absenteeism");
			return gAbsenteeismDB;
		}
	}

	private Database gATMDB;

	public Database ATMDB {
		get {
			if (gATMDB == null)
				gATMDB = new Database("ATM");
			return gATMDB;
		}
	}


	private Javascript gJavascript;
	public Javascript Javascript {
		get {
			if (gJavascript == null)
				gJavascript = new Javascript(this.Page);
			return gJavascript;
		}
	}
	private string gUserName;
	public string UserName {
		get {
			if (gUserName == null)
				gUserName = WebCommon.GetUserName(Context).ToLower();
			return gUserName;
		}
	}
	private ADHelper gAD;
	public ADHelper AD {
		get {
			if (gAD == null)
				gAD = new ADHelper();
			return gAD;
		}
	}
	public void ValidateUser(params string[] groups) {
		if (!AD.IsValidUser(UserName, groups))
			WebCommon.GotoInvalidUserPage(this);
	}
	private ADUser gActiveUser;

	public ADUser ActiveUser {
		get {
			if (gActiveUser == null)
				gActiveUser = AD.GetUser(UserName);
			return gActiveUser;
		}
	}

	public string AppPath {
		get { return Application["AppPath"].ToString(); }
	}

	public string PageName {
		get { return Path.GetFileName(Request.PhysicalPath); }
	}

	public bool CancelRegisterCommonJS { get; set; }

	public static object GetScalarFromStatic(string dbConfig, string sp, params object[] parms) {
		Database db = new Database(dbConfig);
		try {
			return db.GetScalar(sp, parms);
		} catch (Exception exp) {
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Intranet");
			throw new Exception("An error occured while trying to run an sp from a static method", exp);
		} finally {
			db.CloseConnection();
		}
	}

	public static DataView GetDataViewFromStatic(string dbConfig, string sp, params object[] parms) {
		Database db = new Database(dbConfig);
		try {
			return db.GetDataView(sp, parms);
		} catch (Exception exp) {
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Intranet");
			throw new Exception("An error occured while trying to run an sp from a static method", exp);
		} finally {
			db.CloseConnection();
		}
	}

	public static DataSet GetDataSetFromStatic(string dbConfig, string sp, params object[] parms) {
		Database db = new Database(dbConfig);
		try {
			return db.GetDataSet(sp, parms);
		} catch (Exception exp) {
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Intranet");
			throw new Exception("An error occured while trying to run an sp from a static method", exp);
		} finally {
			db.CloseConnection();
		}
	}

	public static void RunNonQueryFromStatic(string dbConfig, string sp, params object[] parms) {
		Database db = new Database(dbConfig);
		try {
			db.RunNonQuery(sp, parms);
		} catch (Exception exp) {
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Intranet");
			throw new Exception("An error occured while trying to run an sp from a static method", exp);
		} finally {
			db.CloseConnection();
		}
	}

	//private int gUserSygmaCenterNo;
	//public int UserSygmaCenterNo {
	//    get {
	//        if (gUserSygmaCenterNo == 0) {
	//            if (UserCenterInfo != null)
	//                gUserSygmaCenterNo = Convert.ToInt32(row["SygmaCenterNo"]);
	//        }
	//        return gUserSygmaCenterNo; }
	//}

	//private DataRowView gUserCenterInfo;
	//public DataRowView UserCenterInfo {
	//    get {
	//        if (gUserCenterInfo == null)
	//            gUserCenterInfo = WebCommon.GetCenterFromOU(IntranetDB, UserName);
	//        return gUserCenterInfo; }
	//}
	//private LocationInfo gUserCenterInfo;
	//private bool gCenterSet = false;
	public LocationInfo UserCenterInfo {
		get {
			if (Session["UserCenterInfo"] == null) {
				string parentOU = ActiveUser.ParentOU;
				string designator = parentOU.Split('-')[0];
				string syscoHouseNo = "84";
				short tmp;
				if (Int16.TryParse(designator, out tmp))
					syscoHouseNo = designator.TrimStart('0');
				Session.Add("UserCenterInfo", new LocationInfo(IntranetDB, false, syscoHouseNo));
				//gUserCenterInfo = new CenterInfo(IntranetDB, UserName);
				//gCenterSet = true;
			}
			return (LocationInfo)Session["UserCenterInfo"];
		}
	}


	private XMLConfig gAppSettings;
	public XMLConfig AppSettings {
		get {
			if (gAppSettings == null)
				gAppSettings = new XMLConfig();
			return gAppSettings;
		}
	}

	private List<GridView> gSelectableGridViews = null;

	public void RegisterSelectableGridView(GridView gv) {
		if (gSelectableGridViews == null)
			gSelectableGridViews = new List<GridView>();
		gSelectableGridViews.Add(gv);
	}


	protected override void Render(HtmlTextWriter writer) {
		if (gSelectableGridViews != null) {
			foreach (GridView gv in gSelectableGridViews) {
				WebCommon.RegisterSelectableRows(this, gv, "$ctl00");
			}
		}
		base.Render(writer);
	}

}

public abstract class BasePageView : ViewPage
{
    protected abstract void LoadBasePage();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LogPageHit();
        LoadBasePage();
        if (WebCommon.IsSiteAdmin(Request))
            Javascript.RegisterStartupScript(this, "MYINFO", string.Format("document.write('<a href=\"javascript: void(0);\" style=\\'color: #ffffff\\' class=\\'invisible\\' accesskey=\\'I\\' TabIndex=\\'-1\\' onFocus=\\'ChangeUser(\"{0}\");this.blur();\\' >i</a>');", UserName), false);
        if (!CancelRegisterCommonJS)
            RegisterCommonJS();
    }

    protected virtual void Page_Unload(object sender, EventArgs e)
    {
        if (gIntranetDB != null)
            gIntranetDB.CloseConnection();
        if (gWebDB != null)
            gWebDB.CloseConnection();
        if (gAbsenteeismDB != null)
            gAbsenteeismDB.CloseConnection();
        if (gOperationsDB != null)
            gOperationsDB.CloseConnection();
        if (gSafetyDB != null)
            gSafetyDB.CloseConnection();
        if (gATMDB != null)
            gATMDB.CloseConnection();
    }

    private bool gRefreshState;
    private bool gIsRefresh;
    public bool IsRefresh
    {
        get
        {
            return gIsRefresh;
        }
    }

    protected override object SaveViewState()
    {
        Session["__ISREFRESH"] = gRefreshState;
        object[] allStates = new object[2];
        allStates[0] = base.SaveViewState();
        allStates[1] = !gRefreshState;
        return allStates;
    }

    //this only fires after the form submits
    protected override void LoadViewState(object savedState)
    {
        object[] allStates = (object[])savedState;
        base.LoadViewState(allStates[0]);
        gRefreshState = (bool)allStates[1];
        gIsRefresh = (gRefreshState == ((Session["__ISREFRESH"] != null) ? (bool)Session["__ISREFRESH"] : true)); //if the users sits on the page too long the session can expire.  save to assume they hit refresh
    }

    public void RegisterCommonJS()
    {
        string includeScriptKey = "Script_File_Common";
        if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), includeScriptKey))
            Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), includeScriptKey, string.Format("{0}Scripts/atmLegacy/Common.js", AppPath));
    }
    public void AddClientVariable(string name, string value)
    {
        AddClientVariable<string>(name, value);
    }
    public void AddClientVariable<T>(string name, T value)
    {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Concat(name.ToUpper(), "_VAR"), string.Format("var {0} = '{1}';", name, value), true);
    }

    public string GetControlPostBackCall(Control control)
    {
        return GetControlPostBackCall(control, true);
    }
    public string GetControlPostBackCall(Control control, bool formatForJSCall)
    {
        string call = Page.ClientScript.GetPostBackClientHyperlink(control, null);
        if (formatForJSCall)
            call = call.Substring(11).Replace("'", "\\'");
        return call;
    }

    public void RedirectToSelf()
    {
        RedirectToSelf(null, null);
    }

    public void RedirectToSelf(string qsKey, string qsValue)
    {
        string url = GetSelfURL(qsKey, qsValue);
        Response.Redirect(url);
    }

    private string GetSelfURL(string qsKey, string qsValue)
    {
        string url = Request.Url.PathAndQuery;
        if (qsKey != null && qsValue != null)
            url = Web.AddQSValue<string>(url, qsKey, qsValue);
        return url;
    }

    public void RedirectToSelf(string message)
    {
        RedirectToSelf(message, null, null);
    }

    public void RedirectToSelf(string message, string qsKey, string qsValue)
    {
        string url = GetSelfURL(qsKey, qsValue);
        this.Javascript.NotifyAndMove(message, url);
    }

    public void GotoInvalidUserPage()
    {
        WebCommon.GotoInvalidUserPage(this);
    }

    protected DataRowView GetUserInfoFromDB(string userName)
    {
        DataView dv = IntranetDB.GetDataView("up_getUserInfo", userName);
        if (dv.Count > 0)
        {
            return dv[0];
        }
        return null;
    }

    private void LogPageHit()
    {
        IntranetDB.RunNonQuery("up_logPageHit", UserName, Request.AppRelativeCurrentExecutionFilePath, Request.QueryString.ToString());
    }

    public void SendFile(FileInfo content, string contentType)
    {
        SendFile(content, contentType, false);
    }
    public void SendFile(FileInfo content, string contentType, bool suppressResponseEnd)
    {
        SendFile(File.ReadAllBytes(content.FullName), content.Name, contentType, suppressResponseEnd);
    }
    public void SendFile(MemoryStream content, string fileName, string contentType, bool suppressResponseEnd)
    {
        var buffer = new byte[content.Length];
        content.Position = 0;
        content.Read(buffer, 0, buffer.Length);
        SendFile(buffer, fileName, contentType, suppressResponseEnd);
    }
    public void SendFile(byte[] content, string fileName, string contentType, bool suppressResponseEnd)
    {
        try
        {
            Response.AddHeader("Content-disposition", string.Format("attachment; filename={0}", fileName));
            Response.ContentType = contentType;
            Response.BinaryWrite(content);
            if (!suppressResponseEnd)
                Response.End();
        }
        catch (System.Threading.ThreadAbortException)
        {
            //do nothing.  not really an error
            Server.ClearError();
        }
        catch (Exception exp)
        {
            //throw new Exception(string.Format("Sending File. FileFullPath: {0}", content.FullName), exp);
            throw new Exception("Sending File.", exp);
        }
    }

    public string SaveExcelFile(FileUpload fu, string fileNameQualifier, string folder, out string message)
    {
        if (fu.HasFile && IsValidExcelFile(fu))
        {
            message = null;
            return SaveFile(fu, fileNameQualifier, folder, ref message);
        }
        else
        {
            message = "Please supply a valid Excel file!";
        }
        return "";
    }

    private string SaveFile(FileUpload fu, string fileNameQualifier, string folder, ref string message)
    {
        string filesFullPath = Path.Combine(Directory.GetParent(Request.PhysicalPath).FullName, folder);
        if (!Directory.Exists(filesFullPath))
            Directory.CreateDirectory(filesFullPath);
        string fileFullName = Path.Combine(filesFullPath, Common.GetUniqueFileName(fileNameQualifier, Path.GetExtension(fu.FileName)));
        fu.SaveAs(fileFullName);
        if (File.Exists(fileFullName))
        {
            return fileFullName;
        }
        else
        {
            message = "*** There was an error uploading your file.  Please try it again.  It this persists please contact the Helpdesk. ***";
        }
        return "";
    }



    private bool IsValidExcelFile(FileUpload fu)
    {
        string fileExtension = Path.GetExtension(fu.FileName);
        return (fileExtension == FileExtensions.EXCEL_9703 || fileExtension == FileExtensions.EXCEL_2007);
    }

    /// <summary>
    /// Gets the FileInfo for a filename.  Adds an index until the fileName does not exist.
    /// </summary>
    /// <param name="fileName">Root name of the file</param>
    /// <returns>The FileInfo</returns>
    public FileInfo GetIndexedFileInfo(string fullFilePath)
    {
        int i = 1;
        string fullFolderPath = Path.GetDirectoryName(fullFilePath);
        string fileNameWOExt = Path.GetFileNameWithoutExtension(fullFilePath);
        string ext = Path.GetExtension(fullFilePath);
        while (File.Exists(fullFilePath))
        {
            string fileName = string.Format("{0}_{1}{2}", fileNameWOExt, i++, ext);
            fullFilePath = Path.Combine(fullFolderPath, fileName);
        }
        return new FileInfo(fullFilePath);
    }

    public void RegisterValidationSummaryMessageBox(ValidationSummary vs)
    {
        System.Reflection.FieldInfo fi = typeof(Page).GetField("_validated", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (fi != null)
        {
            if ((bool)fi.GetValue(Page))
            {
                if (!Page.IsValid)
                {
                    if (vs.ShowMessageBox)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(vs.HeaderText);
                        string sep = " ";
                        switch (vs.DisplayMode)
                        {
                            case ValidationSummaryDisplayMode.BulletList:
                                sep = "\\n-";
                                break;
                            case ValidationSummaryDisplayMode.List:
                                sep = "\\n";
                                break;
                        }
                        foreach (IValidator val in Validators)
                        {
                            if (!val.IsValid)
                            {
                                sb.Append(sep);
                                sb.Append(val.ErrorMessage.Replace("\'", "\\'"));
                            }
                        }
                        Javascript.Notify(sb.ToString(), true);
                    }
                }
            }
        }
    }

    private XMLConfig gAppConfig;
    public XMLConfig AppConfig
    {
        get
        {
            if (gAppConfig == null)
                gAppConfig = new XMLConfig();
            return gAppConfig;
        }
    }
    private Database gIntranetDB;
    public Database IntranetDB
    {
        get
        {
            if (gIntranetDB == null)
                gIntranetDB = new Database("Intranet");
            return gIntranetDB;
        }
    }
    private Database gWebDB;
    public Database WebDB
    {
        get
        {
            if (gWebDB == null)
                gWebDB = new Database("WebReports");
            return gWebDB;
        }
    }
    private Database gOperationsDB;
    public Database OperationsDB
    {
        get
        {
            if (gOperationsDB == null)
                gOperationsDB = new Database("Operations");
            return gOperationsDB;
        }
    }

    private Database gSafetyDB;
    public Database SafetyDB
    {
        get
        {
            if (gSafetyDB == null)
                gSafetyDB = new Database("Safety");
            return gSafetyDB;
        }
    }

    private Database gAbsenteeismDB;

    public Database AbsenteeismDB
    {
        get
        {
            if (gAbsenteeismDB == null)
                gAbsenteeismDB = new Database("Absenteeism");
            return gAbsenteeismDB;
        }
    }

    private Database gATMDB;

    public Database ATMDB
    {
        get
        {
            if (gATMDB == null)
                gATMDB = new Database("ATM");
            return gATMDB;
        }
    }


    private Javascript gJavascript;
    public Javascript Javascript
    {
        get
        {
            if (gJavascript == null)
                gJavascript = new Javascript(this.Page);
            return gJavascript;
        }
    }
    private string gUserName;
    public string UserName
    {
        get
        {
            if (gUserName == null)
                gUserName = WebCommon.GetUserName(Context).ToLower();
            return gUserName;
        }
    }
    private ADHelper gAD;
    public ADHelper AD
    {
        get
        {
            if (gAD == null)
                gAD = new ADHelper();
            return gAD;
        }
    }
    public void ValidateUser(params string[] groups)
    {
        if (!AD.IsValidUser(UserName, groups))
            WebCommon.GotoInvalidUserPage(this);
    }
    private ADUser gActiveUser;

    public ADUser ActiveUser
    {
        get
        {
            if (gActiveUser == null)
                gActiveUser = AD.GetUser(UserName);
            return gActiveUser;
        }
    }

    public string AppPath
    {
        get { return Application["AppPath"].ToString(); }
    }

    public string PageName
    {
        get { return Path.GetFileName(Request.PhysicalPath); }
    }

    public bool CancelRegisterCommonJS { get; set; }

    public static object GetScalarFromStatic(string dbConfig, string sp, params object[] parms)
    {
        Database db = new Database(dbConfig);
        try
        {
            return db.GetScalar(sp, parms);
        }
        catch (Exception exp)
        {
            ErrorHandler eh = new ErrorHandler(exp);
            eh.ReportError("Intranet");
            throw new Exception("An error occured while trying to run an sp from a static method", exp);
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public static DataView GetDataViewFromStatic(string dbConfig, string sp, params object[] parms)
    {
        Database db = new Database(dbConfig);
        try
        {
            return db.GetDataView(sp, parms);
        }
        catch (Exception exp)
        {
            ErrorHandler eh = new ErrorHandler(exp);
            eh.ReportError("Intranet");
            throw new Exception("An error occured while trying to run an sp from a static method", exp);
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public static DataSet GetDataSetFromStatic(string dbConfig, string sp, params object[] parms)
    {
        Database db = new Database(dbConfig);
        try
        {
            return db.GetDataSet(sp, parms);
        }
        catch (Exception exp)
        {
            ErrorHandler eh = new ErrorHandler(exp);
            eh.ReportError("Intranet");
            throw new Exception("An error occured while trying to run an sp from a static method", exp);
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public static void RunNonQueryFromStatic(string dbConfig, string sp, params object[] parms)
    {
        Database db = new Database(dbConfig);
        try
        {
            db.RunNonQuery(sp, parms);
        }
        catch (Exception exp)
        {
            ErrorHandler eh = new ErrorHandler(exp);
            eh.ReportError("Intranet");
            throw new Exception("An error occured while trying to run an sp from a static method", exp);
        }
        finally
        {
            db.CloseConnection();
        }
    }

    //private int gUserSygmaCenterNo;
    //public int UserSygmaCenterNo {
    //    get {
    //        if (gUserSygmaCenterNo == 0) {
    //            if (UserCenterInfo != null)
    //                gUserSygmaCenterNo = Convert.ToInt32(row["SygmaCenterNo"]);
    //        }
    //        return gUserSygmaCenterNo; }
    //}

    //private DataRowView gUserCenterInfo;
    //public DataRowView UserCenterInfo {
    //    get {
    //        if (gUserCenterInfo == null)
    //            gUserCenterInfo = WebCommon.GetCenterFromOU(IntranetDB, UserName);
    //        return gUserCenterInfo; }
    //}
    //private LocationInfo gUserCenterInfo;
    //private bool gCenterSet = false;
    public LocationInfo UserCenterInfo
    {
        get
        {
            if (Session["UserCenterInfo"] == null)
            {
                string parentOU = ActiveUser.ParentOU;
                string designator = parentOU.Split('-')[0];
                string syscoHouseNo = "84";
                short tmp;
                if (Int16.TryParse(designator, out tmp))
                    syscoHouseNo = designator.TrimStart('0');
                Session.Add("UserCenterInfo", new LocationInfo(IntranetDB, false, syscoHouseNo));
                //gUserCenterInfo = new CenterInfo(IntranetDB, UserName);
                //gCenterSet = true;
            }
            return (LocationInfo)Session["UserCenterInfo"];
        }
    }


    private XMLConfig gAppSettings;
    public XMLConfig AppSettings
    {
        get
        {
            if (gAppSettings == null)
                gAppSettings = new XMLConfig();
            return gAppSettings;
        }
    }

    private List<GridView> gSelectableGridViews = null;

    public void RegisterSelectableGridView(GridView gv)
    {
        if (gSelectableGridViews == null)
            gSelectableGridViews = new List<GridView>();
        gSelectableGridViews.Add(gv);
    }


    protected override void Render(HtmlTextWriter writer)
    {
        if (gSelectableGridViews != null)
        {
            foreach (GridView gv in gSelectableGridViews)
            {
                WebCommon.RegisterSelectableRows(this, gv, "$ctl00");
            }
        }
        base.Render(writer);
    }

}
