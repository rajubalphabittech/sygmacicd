using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Sygma.Framework.ClosedXML.Excel;
using SygmaFramework;
using Sygmanet;
using Image = System.Web.UI.WebControls.Image;

namespace Sygmanet
{
	public enum LinkType
	{
		Folder,
		Form,
		Document,
		Url
	}
	public enum LabelPosition
	{
		Top,
		Left,
		InBox
	}
	public enum LocationType
	{
		All,
		RegionalOffice,
		DistributionCenter,
		Domicile
	}

	//public enum SortDirection {
	//  Asc,
	//  Desc
	//}
	public enum UserDisplayFormat
	{
		FirstLastName,
		LastFirstName,
		UserName,
		UserFLName
	}
	public enum Department
	{
		Transportation = 3001,
		Warehouse = 5001
	}
	public enum UserInfo : short
	{
		UserId,
		DisplayName,
		IsAdmin,
		EditableWeeks
	}
	public enum ChangeChecksDisplayType
	{
		Links,
		CheckBox
	}
}
namespace Forms
{
	public enum ChangeLogActions
	{
		CreateForm,
		SavedForm,
		SubmittedForm,
		ApprovedForm,
		RejectedForm,
		ExpiredForm,
		EnteredFormOn400,
		ExpireUpdatedOn400,
		CommentsUpdatedOn400,
		SubmittedExpiredForm,
		InvalidAgreementOn400,
		RemovedBulkItem,
		ReviewedForm
	}

	namespace DeviatedBilling
	{

		public enum Status
		{
			Unsaved = -1,
			Unsent,
			PendingEIReview,
			PendingPricingReview,
			PendingPAReview,
			Rejected = 5,
			Approved,
			Processed,
			Invalid
		}
		public enum UserType
		{
			Invalid,
			Submitter,
			EIReviewer,
			PricingReviewer,
			PAReviewer,
			Approver,
		}
		public enum SearchBy
		{
			Concept = 1,
			Item
		}

		public class Common
		{

			//just do static methods for now
			private Common()
			{

			}

			public static UserType GetUserType(ADHelper ad, string userName)
			{
				if (ad.IsValidUser(userName, "084-Deviated Billing EI Reviewers"))
				{
					return UserType.EIReviewer;
				}
				else if (ad.IsValidUser(userName, "084-Deviated Billing Pricing Reviewers"))
				{
					return UserType.PricingReviewer;
				}
				else if (ad.IsValidUser(userName, "084-Deviated Billing PA Reviewers"))
				{
					return UserType.PAReviewer;
				}
				else if (ad.IsValidUser(userName, "084-Deviated Billing Submitters"))
				{
					return UserType.Submitter;
				}
				return UserType.Invalid;
			}
		}
	}

	namespace EarnedIncome
	{

		public enum Status
		{
			Unsaved = -1,
			Unsent,
			Pending,
			Rejected,
			Approved,
			Processed,
			Invalid
		}

		public enum UserType
		{
			Invalid,
			Submitter,
			Approver
		}
		public enum EIType
		{
			Local = 1,
			SYSCO_Procurement,
			Corporate
		}
		public enum EIMethod
		{
			PerPound = 1,
			PerCase = 2,
			Percentage = 4
		}
		public enum FormType
		{
			Add,
			Expire,
			AddBulk,
			ExpireBulk
		}
	}
}

namespace InvoicePayment
{

	public enum PaymentType
	{
		GLAccount,
		CIP,
		Prepaid
	}

	[Serializable()]
	public struct IPUser
	{
		public decimal RankAmount { get; set; }
		public string RankDisplay { get; set; }
		public string FavoriteDepartment { get; set; }
		public int? FavoriteCenter { get; set; }
		public bool Exists { get; set; }
		public bool FormIsApprover { get; set; }
		public bool FormIsCreator { get; set; }
		public bool FormInQueue { get; set; }
		public bool IsAdmin { get; set; }
		public bool CanPostNow { get; set; }
		public bool CanAssignCIPGL { get; set; }
		public bool CanApproveDups { get; set; }
		public bool ViewAllSubmitted { get; set; }
		public string ScanFolderName { get; set; }
		public int ID { get; set; }
	}

	public class IPCommon
	{
		public static IPUser GetUser(Database db, string userName)
		{
			return GetUser(db, userName, null);
		}
		public static IPUser GetUser(Database db, string userName, int? formId)
		{
			DataSet ds = db.GetDataSet("up_ip_getUser", userName, formId);
			IPUser user = new IPUser();
			DataView dvUser = ds.Tables[0].DefaultView;
			if (dvUser.Count > 0)
			{
				DataRowView row = dvUser[0];

				user.Exists = true;
				user.RankAmount = Convert.ToDecimal(row["RankAmount"]);
				user.RankDisplay = row["RankWebDisplay"].ToString();
				user.FavoriteDepartment = row["FavoriteDepartment"].ToString();
				user.ViewAllSubmitted = (bool)row["ViewAllSubmitted"];
				user.IsAdmin = (bool)row["IsUserAdmin"];
				user.CanPostNow = (bool)row["CanPostNow"];
				user.CanAssignCIPGL = (bool)row["CanAssignCIPGL"];
				user.CanApproveDups = (bool)row["CanApproveDups"];
				user.ScanFolderName = row["ScanFolderName"].ToString();
				user.ID = (int)row["UserId"];
				if (!Convert.IsDBNull(row["FavoriteCenter"]))
					user.FavoriteCenter = Convert.ToInt32(row["FavoriteCenter"]);
				if (ds.Tables.Count > 1)
				{
					row = ds.Tables[1].DefaultView[0];
					user.FormIsApprover = (Convert.ToBoolean(row["IsApprover"]));
					user.FormIsCreator = Convert.ToBoolean(row["IsCreator"]);
					user.FormInQueue = Convert.ToBoolean(row["InUserQueue"]);
				}
			}
			else
			{
				user.Exists = false;
			}
			return user;
		}
	}


	[Serializable]
	public struct PaymentAccount
	{

		public PaymentAccount(short prepaidTerms, double paymentAmount, string notes)
			: this(prepaidTerms, paymentAmount, notes, 0)
		{
		}

		public PaymentAccount(short prepaidTerms, double paymentAmount, string notes, int formAccountId)
		{
			gPaymentType = PaymentType.Prepaid;
			gPaymentAmount = paymentAmount;
			gPrepaidTerms = prepaidTerms;
			gNotes = notes.Replace(Environment.NewLine, " ").Replace("  ", " ");
			gCIPNumber = 0;
			gCenter = 0;
			gDepartment = "";
			gAccount = 0;
			gSubAccount = "0";
			gGLAccountNo = null;
			gAccountDescription = null;
			gFormAccountId = formAccountId;

		}

		public PaymentAccount(int cipNumber, double paymentAmount, string notes)
			: this(cipNumber, paymentAmount, notes, 0)
		{
		}
		public PaymentAccount(int cipNumber, double paymentAmount, string notes, int formAccountId)
		{
			gPaymentType = PaymentType.CIP;
			gCIPNumber = cipNumber;
			gPaymentAmount = paymentAmount;
			gNotes = notes.Replace(Environment.NewLine, " ").Replace("  ", " ");
			gCenter = 0;
			gDepartment = "";
			gAccount = 0;
			gSubAccount = "0";
			gGLAccountNo = null;
			gAccountDescription = null;
			gFormAccountId = formAccountId;
			gPrepaidTerms = 0;
		}
		public PaymentAccount(short center, string department, int account, string subAccount, string accountDescription, double paymentAmount, string notes)
			: this(center, department, account, subAccount, accountDescription, paymentAmount, notes, 0)
		{
		}
		public PaymentAccount(short center, string department, int account, string subAccount, string accountDescription, double paymentAmount, string notes, int formAccountId)
		{
			gPaymentType = PaymentType.GLAccount;
			gCenter = center;
			gDepartment = department;
			gAccount = account;
			gSubAccount = subAccount.PadLeft(4, '0');
			gGLAccountNo = null;
			gPaymentAmount = paymentAmount;
			gNotes = notes.Replace(Environment.NewLine, " ").Replace("  ", " ");
			gAccountDescription = accountDescription;
			gCIPNumber = 0;
			gFormAccountId = formAccountId;
			gPrepaidTerms = 0;
			SetGLAccountNo();
		}

		public PaymentAccount(PaymentType paymentType, short center, string department, int account, string subAccount, string accountDescription, int cipNumber, double paymentAmount, string notes, short prepaidTerms)
			: this(paymentType, center, department, account, subAccount, accountDescription, cipNumber, paymentAmount, notes, prepaidTerms, 0)
		{
		}
		public PaymentAccount(PaymentType paymentType, short center, string department, int account, string subAccount, string accountDescription, int cipNumber, double paymentAmount, string notes, short prepaidTerms, int formAccountId)
		{
			gPaymentType = paymentType;
			gCenter = center;
			gDepartment = department;
			gAccount = account;
			gSubAccount = subAccount.PadLeft(4, '0');
			gGLAccountNo = null;
			gPaymentAmount = paymentAmount;
			gNotes = notes.Replace(Environment.NewLine, " ").Replace("  ", " ");
			gAccountDescription = accountDescription;
			gCIPNumber = cipNumber;
			gFormAccountId = formAccountId;
			gPrepaidTerms = prepaidTerms;
			SetGLAccountNo();
		}

		private short gCenter;

		public short Center
		{
			get { return gCenter; }
			set
			{
				gCenter = value;
				SetGLAccountNo();
			}
		}

		private string gDepartment;

		public string Department
		{
			get { return gDepartment; }
			set
			{
				gDepartment = value;
				SetGLAccountNo();
			}
		}

		private int gAccount;

		public int Account
		{
			get { return gAccount; }
			set
			{
				gAccount = value;
				SetGLAccountNo();
			}
		}

		private string gSubAccount;

		public string SubAccount
		{
			get { return gSubAccount; }
			set
			{
				gSubAccount = value;
				SetGLAccountNo();
			}
		}

		private void SetGLAccountNo()
		{
			gGLAccountNo = string.Format("{0}-{1}-{2}-{3}", Center.ToString().PadLeft(3, '0'), Department, Account, SubAccount);
		}

		private int gCIPNumber;

		public int CIPNumber
		{
			get { return gCIPNumber; }
			set { gCIPNumber = value; }
		}


		private string gGLAccountNo;

		public string GLAccountNo
		{
			get { return gGLAccountNo; }
		}

		private double gPaymentAmount;

		public double PaymentAmount
		{
			get { return gPaymentAmount; }
			set { gPaymentAmount = value; }
		}

		private string gNotes;

		public string Notes
		{
			get { return gNotes; }
			set { gNotes = value; }
		}

		private string gAccountDescription;

		public string AccountDescription
		{
			get { return gAccountDescription; }
			set { gAccountDescription = value; }
		}

		private PaymentType gPaymentType;

		public PaymentType PaymentType
		{
			get { return gPaymentType; }
			set { gPaymentType = value; }
		}

		private int gFormAccountId;

		public int FormAccountId
		{
			get { return gFormAccountId; }
			set { gFormAccountId = value; }
		}

		private short gPrepaidTerms;

		public short PrepaidTerms
		{
			get { return gPrepaidTerms; }
			set { gPrepaidTerms = value; }
		}


		//private short gAmortizationMonths;

		//public short AmortizationMonths {
		//  get { return gAmortizationMonths; }
		//  set { gAmortizationMonths = value; }
		//}
	}

}

namespace ATM
{
	public class Dates
	{
		public Dates() { }
		public static DateTime GetWeekending(DateTime dt)
		{
			if (dt.DayOfWeek != DayOfWeek.Saturday)
				return dt.AddDays(6 - (int)dt.DayOfWeek);
			return dt;
		}

		public static DateTime GetWeekending()
		{
			return GetWeekending(DateTime.Today);
		}
	}

	namespace Payroll
	{
		public enum FormStatus
		{
			Out,
			In,
			Submitted,
			Approve,
			Reject
		}
		public enum FormType
		{
			Regular,
			Special,
			Miscellaneous,
			Line
		}

		public static class PayrollCommon
		{
			public static readonly string[,] FormPageGridColumns = {
				{"1001", "Status"},
				{"1002", "ID"},
				{"1003", "Type"},
				{"1004", "Center"},
				{"1005", "Route #"},
				{"1006", "Weekending"},
				{"1007", "Depart Date"},
				{"1008", "Employee 1"},
				{"1009", "Employee 2"},
				{"1010", "Tractor 1"},
				{"1011", "Trailer 1"},
				{"1012", "Assigned Driver"},
				{"1013", "Assigned Team Driver/Helper"},
				{"1014", "AU"},
				{"1015", "Cases"},
				{"1016", "LBs"},
				{"1017", "Cubes"},
				{"1018", "Miles"},
				{"1019", "Stops"}
			};

			public static readonly string[,] ManageRouteGridColumns = {
				{"2001", "Route"},
				{"2002", "Route Name"},
				{"2003", "Origin (Location)*"},
				{"2004", "Classification"},
				{"2005", "Miles"},
				{"2006", "Default Driver"},
				{"2007", "Default Team Driver/Helper"},
				{"2008", "Driver Pay Scale*"},
				{"2009", "Driver Helper Pay Scale*"},
				{"2010", "Zip Code"},
				{"2011", "City/State"},
				{"2012", "Depart Day"},
				{"2013", "Depart Time hh:mm"},
				{"2014", "Duration"},
				{"2015", "Is Holiday Route"},
				{"2016", "Alt Route No"},
				{"2017", "Alt Depart Day"},
				{"2018", "Alt Depart Time hh:mm"},
				{"2019", "Alt Duration"},
				{"2020", "Is Active "},
				{"2021", "Remove"}
			};

			public static readonly string[,] ManageVehiclesTrailersColumns = {
				{"3001", "VehicleName"},
				{"3002", "Unit#"},
				{"3003", "Center"},
                {"3019", "Center #"},
                {"3004", "Unit Attached #"},
				{"3005", "Make"},
				{"3006", "VIN"},
				{"3007", "Replaced"},
				{"3008", "Model"},
				{"3009", "Cab Type"},
				{"3010", "Status"},
				{"3011", "Year"},
				{"3012", "Description"},
				{"3013", "Object Type"},
				{"3014", "Odometer/Hours"},
				{"3015", "Odometer Last Updated"},
				{"3016", "Active"},
				{"3017", "Depreciation"},
				{"3018", "Additional Info"}
			};

            public static readonly string[,] ManageEmployeesColumns = {
                {"4001", "Employee"},
                {"4002", "Q. Driver"},
                {"4003", "Hire Date"},
                {"4004", "Eff Hire Date"},
                {"4005", "Tenure"},
                {"4006", "Rate (%)"},
                {"4007", "APS EXCPTN"},
                {"4008", "Guaranteed Pay ($)"},
                {"4009", "Classification"},
                {"4010", "RTP"}
            };
        }
	}
}
namespace SygmaIntranet.Reports.Cadec.Fix_Lat_Longs
{
	public enum SearchType
	{
		Center = 1,
		CustomerNo,
		BillTo
	}
}

public class WebCommon
{
	public const string REGEXP_VALID_EMAIL = @"(\s*([\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})\s*)";
	public const int CURRENT_NEWS_COUNT = 5;
	public const string DIR_SEPERATOR = "\\";
	public const string INVALID_USER_PAGE = "~/Apps/ATM/CustomErrors/401.aspx";
	public const string OBJECT_NOT_EXIST_PAGE = "~/Apps/ATM/CustomErrors/404.aspx";
	public const string GROUP_LDAP_FORMAT = "CN={0},OU=Groups,OU=SYGMA";
	public const int FEET_IN_MILES = 5280;

	public WebCommon() { }
	public static string GetUserName(HttpContext context)
	{
		HttpRequest Request = context.Request;
		HttpSessionState Session = context.Session;

		string fullUserName = Request.ServerVariables["LOGON_USER"].ToString();
		string userName = GetUserName(fullUserName);
		if (IsSiteAdmin(Request, userName))
		{
			string sessionName = "ImpersonateUser";
			string userQS = Request.QueryString.Get("user");
			if (userQS != null)
			{
				if (userQS == "clear")
				{
					Session.Remove(sessionName);
				}
				else
				{
					userName = userQS;
					Session[sessionName] = userQS;
				}
				context.Response.Redirect(Web.RemoveQSValue(Request.RawUrl, "user"));
			}
			else if (Session[sessionName] != null)
			{
				userName = Session[sessionName].ToString();
			}
		}
		return userName;
	}
	public static bool IsSiteAdmin(HttpRequest Request)
	{
		string fullUserName = Request.ServerVariables["LOGON_USER"].ToString();
		return IsSiteAdmin(Request, fullUserName);
	}
	public static bool IsSiteAdmin(HttpRequest Request, string userName)
	{
		return (userName.EndsWith("bspa6589") || userName.EndsWith("msiv2896") || userName.EndsWith("mbai5105") || userName.EndsWith("skru7538"));
	}

	public static string GetUserName(string fullUserName)
	{
		int seperatorIndex = fullUserName.LastIndexOf(DIR_SEPERATOR);
		return fullUserName.Substring(seperatorIndex + 1);
	}

	public static bool ControlTypeExists(Control control, Type findType)
	{
		bool controlExists = false;
		if (control.HasControls())
		{
			foreach (Control childControl in control.Controls)
			{
				if (childControl.GetType() != findType)
				{
					controlExists = ControlTypeExists(childControl, findType);
					if (controlExists)
						break;
				}
				else
				{
					controlExists = true;
					break;
				}
			}
		}
		return controlExists;
	}
	public static DataSet GetDataSetFromXML(string xmlString)
	{
		XmlReader xr = null;
		try
		{
			DataSet ds = new DataSet();
			StringReader sr = new StringReader(xmlString);
			xr = new XmlTextReader(sr);
			ds.ReadXml(xr);
			return ds;
		}
		catch (Exception exp)
		{
			throw new Exception(string.Format("Loading XML into a DataSet: XML: {0}", xmlString), exp);
		}
		finally
		{
			if (xr != null)
				xr.Close();
		}
	}
	public static string BuildWebParentPath(string path, string rootDir)
	{
		try
		{
			DirectoryInfo parent = Directory.GetParent(path);
			string parentPath = "";
			while (parent.Name != rootDir)
			{
				parentPath = string.Format("{0}/{1}", parent.Name, parentPath);
				parent = Directory.GetParent(parent.FullName);
			}
			return parentPath;
		}
		catch (Exception exp)
		{
			throw new Exception(string.Format("Getting Web Parent Path. Path: {0} RootDir: {1}", path, rootDir), exp);
		}
	}
	public static string GetPageName(string[] urlSegments)
	{
		int count = urlSegments.Length;
		return urlSegments[count - 1];
	}
	public static string GetLDAPGroupPath(string groupName)
	{
		return string.Format(GROUP_LDAP_FORMAT, groupName);
	}
	public static void ViewPageAsExcel(Page page, HttpResponse response, string name)
	{
		page.EnableViewState = false;
		string fileName = string.Format("{0}_{1}.xls", name, DateTime.Now.ToString("yyyyMMddHHmmss"));
		response.ContentType = "application/vnd.ms-excel";
		response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
	}
	public static void SetAsProcessingButton(Button button)
	{
		button.Attributes.Add("onClick", "this.value='Processing...';");
	}

    /**
     * @Author Rene
     * @Date 3/30/2018
     * If no access to desire page, force user to home controller
     * 
    **/
    public static void routeToHomeController(Page page)
    {
        string newPath = "~/";
        try
        {
            string appPath = page.Application["AppPath"].ToString();
            page.Response.Redirect(newPath, true);
        }
        catch (ThreadAbortException)
        {
        }
    }


    public static void GotoInvalidUserPage(Page page)
	{
		try
		{
			string appPath = page.Application["AppPath"].ToString();
			page.Response.Redirect(INVALID_USER_PAGE, true);
		}
		catch (ThreadAbortException)
		{
			//do nothing, b/c there isn't an error
		}
	}
	public static string GetUniqueFilePath(string path, string fileName)
	{
		return GetUniqueFilePath(path, fileName, 0);
	}
	public static string GetUniqueFilePath(string path, string fileName, int index)
	{
		string newFileName = fileName;
		if (index != 0)
		{
			string ext = Path.GetExtension(fileName);
			string fileNameWOE = Path.GetFileNameWithoutExtension(fileName);
			newFileName = string.Concat(fileNameWOE, "_", index, ext);
		}

		string retVal = Path.Combine(path, newFileName);
		if (File.Exists(retVal))
			retVal = GetUniqueFilePath(path, fileName, ++index);

		return retVal;
	}
	public static void SetListControlFromEnum<T>(ListControl lc)
	{
		Type enumType = typeof(T);
		string[] ses = Enum.GetNames(enumType);
		foreach (string se in ses)
		{
			lc.Items.Add(new ListItem(se.Replace("_", " "), ((int)Enum.Parse(enumType, se)).ToString()));
		}
	}



	#region Data Methods

	public static DataView GetCenterList(Database DB)
	{
		return GetCenterList(DB, LocationType.All);
	}

	public static DataView GetCenterList(Database DB, LocationType type)
	{
		try
		{
			object[] parms = new object[1];
			if (type != LocationType.All)
				parms[0] = type;

			return DB.GetDataView("up_GetLocationsList", parms);

		}
		catch (Exception exp)
		{
			throw new Exception("Getting list of Centers.", exp);
		}
	}

	public static DataRowView GetCenterFromOU(Database DB, string userName)
	{
		ADHelper adh = new ADHelper();
		ADUser user = adh.GetUser(userName);
		DataView dv = DB.GetDataView("up_GetCenterFromOUName", user.ParentOU);
		if (dv.Count > 0)
			return dv[0];
		return null;
	}

	public static bool SelectListValue<T>(ListControl lc, T value)
	{
		return SelectListValue<T>(lc, value, false);
	}
	public static bool SelectListValue<T>(ListControl lc, T value, bool allowMultiple)
	{
		if (value != null)
		{
			ListItem li = lc.Items.FindByValue(value.ToString());
			if (li != null)
			{
				if (!allowMultiple)
					lc.ClearSelection();
				li.Selected = true;
				return true;
			}
		}
		return false;
	}
	public static void SelectListText(ListControl lc, string value)
	{
		SelectListText(lc, value, false);
	}
	public static void SelectListText(ListControl lc, string value, bool allowMultiple)
	{
		ListItem li = lc.Items.FindByText(value);
		if (li != null)
		{
			if (!allowMultiple)
				lc.ClearSelection();
			li.Selected = true;
		}
	}

	public static string GetSelectedValues(ListBox lb)
	{
		if (lb.SelectedIndex > -1)
		{
			StringBuilder sb = new StringBuilder();
			foreach (int i in lb.GetSelectedIndices())
			{
				sb.Append(",");
				sb.Append(lb.Items[i].Value);
			}

			//bool isFirst = true;
			//foreach (ListItem item in lb.Items) {
			//  if (item.Selected && (item.Value != "")) {
			//    if (!isFirst)
			//      sb.Append(",");
			//    sb.Append(item.Value);
			//    isFirst = false;
			//  }
			//}
			return sb.ToString().Substring(1);
		}
		else
		{
			return "";
		}
	}



	/// <summary>
	/// Returns a comma delimited string of item values
	/// </summary>
	/// <param name="lb">Teh ListBox to look for selected items</param>
	/// <param name="returnAllItemsIfSelected">If the "All" option is specified and selected, then return all the items except for the "All" option</param>
	/// <returns></returns>
	public static string GetSelectedValues(ListBox lb, bool returnAllItemsIfSelected)
	{
		if (returnAllItemsIfSelected && lb.SelectedItem.Text.ToLower() == "all")
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 1; i < lb.Items.Count; i++)
			{
				sb.Append(",");
				sb.Append(lb.Items[i].Value);
			}
			return sb.ToString().Substring(1);
		}
		else
		{
			return GetSelectedValues(lb);
		}
	}
	[Obsolete("Use SelectListValue")]
	public static void SelectByValue(DropDownList ddl, string value)
	{
		ddl.ClearSelection();
		ListItem li = ddl.Items.FindByValue(value);
		if (li != null)
			li.Selected = true;
	}
	public static void SetSortColumn(GridView gv, GridViewRow row)
	{
		int sortIndex = GetSortColumnIndex(gv);
		if (sortIndex != -1)
			AddSortImage(sortIndex, row, gv);
	}
	public static int GetSortColumnIndex(GridView gv)
	{
		int retVal = -1;
		if (gv.SortExpression != "")
			foreach (DataControlField field in gv.Columns)
			{
				if (field.SortExpression == gv.SortExpression)
				{
					retVal = gv.Columns.IndexOf(field);
					break;
				}
			}
		return retVal;
	}
	public static void AddSortImage(int columnIndex, GridViewRow headerRow, GridView gv)
	{
		Image sortImage = new Image();
		sortImage.ImageUrl =
			(gv.SortDirection == SortDirection.Ascending) ? "~/Images/sort_up_1.gif" : "~/Images/sort_dn_1.gif";
		headerRow.Cells[columnIndex].Controls.Add(sortImage);
	}
	public static bool IsWebUrl(string url)
	{
		return url.StartsWith("~/") ||
		       url.StartsWith("/") ||
		       url.StartsWith("http") ||
		       url.StartsWith("www") ||
		       url.StartsWith("ftp") ||
		       url.StartsWith("file:");
	}

	public static string[] GetItems(string item, int count)
	{
		try
		{
			Database db = new Database("Intranet");
			DataView dv = db.GetDataView("up_getItems", item, count);
			//dv.RowFilter = string.Format("SUPCNumber like '%{0}%' or SUPCDescription like '%{0}%'", item.Replace(" ", "%"));
			dv.Sort = "SUPCNumber asc";
			List<string> items = new List<string>(dv.Count);
			for (int i = 0; i < dv.Count; i++)
			{
				if (i == count)
					break;
				DataRowView row = dv[i];
				items.Add(string.Format("{0} - {1}", row["SUPCNumber"], row["SUPCDescription"]));
			}
			return items.ToArray();
		}
		catch (Exception exp)
		{
			throw new Exception("Getting list of items", exp);
		}
	}

	public static string ParseItemNo(string itemText)
	{
		if (itemText.IndexOf('-') > -1)
			return itemText.Split('-')[0].Trim();
		return itemText.Trim();
	}

	/// <summary>
	/// parses the item from the item selector box and set the parameter
	/// </summary>
	/// <param name="itemParam">Item No Parameter</param>
	/// <returns>whether or not the time number was valid</returns>
	public static bool SetItemNoParameter(DbParameter itemParam)
	{
		if (itemParam.Value != null)
		{
			string itemNo = ParseItemNo(itemParam.Value.ToString());
			int i;
			if (Int32.TryParse(itemNo, out i))
			{
				itemParam.Value = i;
				return true;
			}
			else
			{
				itemParam.Value = null;
				return false;
			}
		}
		return true;
	}

	public static string GetDBSortDirection(string direction)
	{
		try
		{
			return GetDBSortDirection((SortDirection)Enum.Parse(typeof(SortDirection), direction));
		}
		catch (Exception exp)
		{
			throw new Exception(string.Format("Error parsing '{0}' to Type SortDirection", direction), exp);
		}
	}
	public static string GetDBSortDirection(SortDirection direction)
	{
		return (direction == SortDirection.Ascending) ? "asc" : "desc";
	}

	public static SortDirection FlipSortDirection(SortDirection direction)
	{
		return (direction == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
	}

	public static void SetCheckBoxReadOnly(CheckBox cb, bool isReadOnly)
	{
		cb.Attributes.Add("onClick", (isReadOnly) ? string.Format("this.checked = {0};", cb.Checked.ToString().ToLower()) : "");
	}

	public static void SetDropDownReadOnly(DropDownList dd, Label lbl, bool readOnly)
	{
		dd.Visible = !readOnly;
		lbl.Visible = readOnly;
		if (readOnly)
		{
			if (dd.SelectedItem != null)
			{
				lbl.Text = dd.SelectedItem.Text;
			}
		}
	}

	public static void RunNonQuery(string dbName, string sp, params object[] parms)
	{
		Database db = new Database(dbName);
		try
		{
			db.RunNonQuery(sp, parms);
		}
		catch (Exception exp)
		{
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Webcommon.RunNonQuery");
		}
		finally
		{
			db.CloseConnection();
		}
	}

	public static DataView RunSP(string sp, params object[] parms)
	{
		Database db = new Database("Intranet");
		return RunSP(db, sp, parms);
	}

	public static DataView RunSP(Database db, string sp, params object[] parms)
	{
		try
		{
			return db.GetDataView(sp, parms);
		}
		catch (Exception exp)
		{
			ErrorHandler eh = new ErrorHandler(exp);
			eh.ReportError("Web Method");
			throw new Exception("An error occured while trying to save data");
		}
		finally
		{
			db.CloseConnection();
		}
	}


	#endregion
	[Obsolete("Do not need to pass in the GridView anymore.  Switch to the overload w/o the GridView")]
	public static void SetRowSelectable(Page page, GridViewRowEventArgs e, GridView gv)
	{
		SetRowSelectable(page, e);
	}
	public static void SetRowSelectable(Page page, GridViewRowEventArgs e)
	{
		LinkButton lb = (LinkButton)e.Row.Cells[0].Controls[0];
		if (lb != null)
		{
			string linkButtonScript = page.ClientScript.GetPostBackClientHyperlink(lb, "");
			SetRowScript(linkButtonScript, e);
		}
	}
	public static void SetRowScript(string rowScript, GridViewRowEventArgs e)
	{
		e.Row.Style.Add("Cursor", "pointer");
		e.Row.Attributes.Add("onclick", rowScript);
		SetRowHighlight(e);
	}

	public static void SetRowHighlight(GridViewRowEventArgs e)
	{
		GridView gv = (GridView)e.Row.NamingContainer;

		Color selectedColor = gv.SelectedRowStyle.BackColor;
		string hoverColor = selectedColor.Name;
		if (!selectedColor.IsNamedColor && hoverColor.StartsWith("ff"))
			hoverColor = string.Concat("#", selectedColor.Name.Substring(2, 6));
		e.Row.Attributes.Add("onmouseover",
			string.Format("this.originalFontWeight=this.style.fontWeight;this.originalColor=this.style.color;this.originalBC=this.style.backgroundColor;this.style.fontWeight='{2}';this.style.color='{0}';this.style.backgroundColor='{1}';this.className='{3}';", gv.SelectedRowStyle.ForeColor.Name, hoverColor, (gv.SelectedRowStyle.Font.Bold) ? "bold" : "normal", gv.SelectedRowStyle.CssClass));
		e.Row.Attributes.Add("onmouseout",
			string.Format("this.style.backgroundColor=this.originalBC;this.style.color=this.originalColor;this.style.fontWeight=this.originalFontWeight;this.className='{0}';", gv.RowStyle.CssClass));
	}



	public static void RegisterSelectableRows(Page page, GridView gv, string buttonId)
	{
		foreach (GridViewRow r in gv.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00");
			}
		}
	}
}

public struct LocationInfo
{
	public LocationInfo(Database db, bool useSygmaCenterNo, string value)
	{
		string sp = (useSygmaCenterNo) ? "dbo.up_GetCenterFromSygmaNo" : "up_GetCenterFromSyscoNo";
		DataView dv = db.GetDataView(sp, value);
		if (dv.Count > 0)
		{
			DataRowView row = dv[0];
			gSygmaCenterNo = Convert.ToInt32(row["SygmaCenterNo"]);
			gSyscoHouseNo = Convert.ToInt32(row["SyscoHouseNo"]);
			gDescription = row["Description"].ToString();
			gLocationType = (LocationType)Enum.Parse(typeof(LocationType), row["LocationTypeID"].ToString());
			gOUName = row["OUName"].ToString();
			gUserOUPath = row["UserOUPath"].ToString();
		}
		else
		{
			gSygmaCenterNo = 0;
			gSyscoHouseNo = 0;
			gDescription = "";
			gLocationType = LocationType.All;
			gOUName = "";
			gUserOUPath = "";
		}
	}
	private int gSygmaCenterNo;
	public int SygmaCenterNo
	{
		get { return gSygmaCenterNo; }
	}
	private int gSyscoHouseNo;
	public int SyscoHouseNo
	{
		get { return gSyscoHouseNo; }
	}
	private string gDescription;
	public string Description
	{
		get { return gDescription; }
	}
	private LocationType gLocationType;
	public LocationType LocationType
	{
		get { return gLocationType; }
	}
	private string gOUName;
	public string OUName
	{
		get { return gOUName; }
	}
	private string gUserOUPath;
	public string UserOUPath
	{
		get { return gUserOUPath; }
	}
}

[Serializable]
public class InvalidBasePageException : Exception
{
	//
	// For guidelines regarding the creation of new exception types, see
	//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
	// and
	//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
	//
	private const string DEFAULT_MESSAGE = "This is NOT a valid BasePage.  ";
	public InvalidBasePageException() : base(DEFAULT_MESSAGE) { }
	public InvalidBasePageException(string message) : base(string.Concat(DEFAULT_MESSAGE, message)) { }
	public InvalidBasePageException(string message, Exception inner) : base(string.Concat(DEFAULT_MESSAGE, message), inner) { }
	public InvalidBasePageException(Exception inner) : base(DEFAULT_MESSAGE, inner) { }
	protected InvalidBasePageException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) { }
}

[Serializable]
public class InvalidATMPageException : Exception
{
	//
	// For guidelines regarding the creation of new exception types, see
	//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
	// and
	//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
	//
	private const string DEFAULT_MESSAGE = "This is NOT a valid ATMPage.  ";
	public InvalidATMPageException() : base(DEFAULT_MESSAGE) { }
	public InvalidATMPageException(string message) : base(string.Concat(DEFAULT_MESSAGE, message)) { }
	public InvalidATMPageException(string message, Exception inner) : base(string.Concat(DEFAULT_MESSAGE, message), inner) { }
	public InvalidATMPageException(Exception inner) : base(DEFAULT_MESSAGE, inner) { }
	protected InvalidATMPageException(
		SerializationInfo info,
		StreamingContext context)
		: base(info, context) { }
}


[Serializable]
public class ParseFileException : Exception
{
	public ParseFileException() { }
	public ParseFileException(string message) : base(message) { }
	public ParseFileException(string message, Exception inner) : base(message, inner) { }

	public ParseFileException(int rowNo, int columnNumber) { RowNumber = rowNo; ColumnNumber = columnNumber; }
	public ParseFileException(string message, int rowNo, int columnNumber) : base(message) { RowNumber = rowNo; ColumnNumber = columnNumber; }
	public ParseFileException(string message, int rowNo, int columnNumber, Exception inner) : base(message, inner) { RowNumber = rowNo; ColumnNumber = columnNumber; }

	public ParseFileException(IXLCell cell) { Cell = cell; }
	public ParseFileException(string message, IXLCell cell) : base(message) { Cell = cell; }
	public ParseFileException(string message, IXLCell cell, Exception inner) : base(message, inner) { Cell = cell; }

	/// <summary>
	/// Line Number to show to the user
	/// </summary>
	public int RowNumber { get; private set; }

	/// <summary>
	/// columnNumber to show to the user
	/// </summary>
	public int ColumnNumber { get; private set; }

	public IXLCell Cell { get; private set; }

	public string GetInvalidExcelValueMessage()
	{
		return string.Format("The value \\'{0}\\' in cell \\'{1}\\' is invalid.", Cell.Value, Cell.FormulaR1C1);
	}

	protected ParseFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

#region Custom Event Handlers

public delegate void RejectButtonOKClickEventHandler(object sender, RejectButtonOKClickEventArgs e);
public class RejectButtonOKClickEventArgs : EventArgs
{
	public RejectButtonOKClickEventArgs(string reasonText)
	{
		gReasonText = reasonText;
	}
	private string gReasonText;
	public string ReasonText
	{
		get { return gReasonText; }
	}
}

#endregion Custom Event Handlers