
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;
using SygmaFramework;
using System.Collections.Generic;
using System.Text;
using atm;
using Sygmanet;

/// <summary>
///		Summary description for ADUserDropDown.
/// </summary>
//public delegate void NameChangedEventHandler(object sender, EventArgs e);
public partial class UserControls_ADUserDropDown : System.Web.UI.UserControl
{
    private string gADGroupPath = "";
    private bool gInsertChooseItem = false;
    private string gDisplayName = "";
    private bool gRequired = false;
    private UserDisplayFormat gDisplayFormat = UserDisplayFormat.LastFirstName;
    //private System.Web.UI.WebControls.SortDirection gSortDir = System.Web.UI.WebControls.SortDirection.Ascending;

    private const string CHOOSE_ITEM_TEXT = "Choose...";
    protected void Page_Load(object sender, System.EventArgs e)
    {

        if (!IsPostBack)
        {
            if (this.Visible && AutoLoadUsers)
                LoadUsers();
        }
        else
        {
            if (RemoveChooseItemOnPostBack)
                RemoveChooseItem();
        }
        SetValidation();

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (gHideDisplayName)
            lblDDName.Visible = false;
        if (ReadOnly)
        {
            ddUsers.Visible = false;
            if (ddUsers.SelectedItem != null)
            {
                lblDisplayName.Visible = true;
                lblDisplayName.Text = ddUsers.SelectedItem.Text;
            }
        }
        else
        {
            ddUsers.Visible = true;
            lblDisplayName.Visible = false;
        }
    }

    public override void DataBind()
    {
        LoadUsers();
    }

    private void RemoveChooseItem()
    {
        ListItem li = ddUsers.Items.FindByText(ChooseItemText);
        if (li != null)
            ddUsers.Items.Remove(li);
    }
    public void LoadUsers()
    {
        ADHelper ad = new ADHelper();

        string cacheName = "";
        if (gADGroupPath != "")
        {
            cacheName = string.Format("{0}_ADUsers", gADGroupPath.Replace(" ", "_"));
        }
        else if (OUPath != "")
        {
            //cacheName = string.Format("{0}_ADUsers", OUPath);
        }

        ADUser[] adUsers = null;
        if (cacheName == "" || Cache[cacheName] == null || !string.IsNullOrEmpty(Request.QueryString.Get("cache")))
        { //use this b/c AD is so slow over VPN.  should make any pages slightly faster as well
            if (Cache[cacheName] != null)
                Cache.Remove(cacheName);
            if (gADGroupPath != "")
            {
                adUsers = ad.GetUsers(gADGroupPath);
            }
            else if (OUPath != "")
            {
                adUsers = ad.GetUsersFromOU(OUPath);
            }
            else
            {
                throw new Exception("You must specify a group or OU to pull users from");
            }
            Cache.Add(cacheName, adUsers, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
        }
        else
        {
            adUsers = (ADUser[])Cache[cacheName];
        }
        ddUsers.Items.Clear();
        if (gInsertChooseItem)
            ddUsers.Items.Add(new ListItem(ChooseItemText, ChooseItemValue));

        if (adUsers.Length > 0)
            AddUsers(adUsers);

        if (gDisplayName != "")
        {
            lblDDName.InnerText = gDisplayName;
            lblDDName.Visible = true;
        }
    }
    private void AddUsers(ADUser[] users)
    {
        switch (gDisplayFormat)
        {
            case UserDisplayFormat.FirstLastName:
                Array.Sort<ADUser>(users, SortByFirstName);
                break;
            case UserDisplayFormat.LastFirstName:
                Array.Sort<ADUser>(users, SortByLastName);
                break;
            case UserDisplayFormat.UserName:
                Array.Sort<ADUser>(users, SortByUserName);
                break;
            case UserDisplayFormat.UserFLName:
                Array.Sort<ADUser>(users, SortByLastName);
                break;
        }

        foreach (SygmaFramework.ADUser user in users)
        {
            switch (gDisplayFormat)
            {
                case (UserDisplayFormat.FirstLastName):
                    ddUsers.Items.Add(new ListItem(string.Concat(user.FirstName, " ", user.LastName), user.UserName));
                    break;
                case (UserDisplayFormat.UserName):
                    ddUsers.Items.Add(new ListItem(user.Name, user.UserName));
                    break;
                case (UserDisplayFormat.LastFirstName):
                    string firstName = user.FirstName;
                    string lastName = user.LastName;

                    StringBuilder sb = new StringBuilder(lastName);
                    if (firstName != "")
                    {
                        if (lastName != "")
                            sb.Append(", ");
                        sb.Append(firstName);
                    }
                    ddUsers.Items.Add(new ListItem(sb.ToString(), user.UserName));
                    break;
                case (UserDisplayFormat.UserFLName):
                    string fName = user.FirstName;
                    string lName = user.LastName;

                    StringBuilder sb1 = new StringBuilder(lName);
                    if (fName != "")
                    {
                        if (lName != "")
                            sb1.Append(", ");
                        sb1.Append(fName);
                        sb1.Append(" (");
                        sb1.Append(user.UserName);
                        sb1.Append(")");
                    }
                    ddUsers.Items.Add(new ListItem(sb1.ToString(), user.UserName));
                    break;
            }
        }
    }

    private static int SortByFirstName(ADUser x, ADUser y)
    {
        int result = x.FirstName.CompareTo(y.FirstName);
        if (result == 0)
            result = x.LastName.CompareTo(y.LastName);
        return result;
    }

    private static int SortByLastName(ADUser x, ADUser y)
    {
        int result = x.LastName.CompareTo(y.LastName);
        if (result == 0)
            result = x.FirstName.CompareTo(y.FirstName);
        return result;
    }

    private static int SortByUserName(ADUser x, ADUser y)
    {
        return x.UserName.CompareTo(y.UserName);
    }

    private void SetValidation()
    {
        if (gRequired)
        {
            rfvUsers.Visible = true;
            //set error message.  default to "Name is Required" if DisplayName is not set
            if (gDisplayName != "")
            {
                rfvUsers.ErrorMessage = string.Format("'{0}' is required.", gDisplayName);
            }

            bool summaryInUse = SummaryInUse();
            //set whether or not to display the message or to display the *
            if (summaryInUse)
            {
                rfvUsers.Text = "*";
            }
            else
            {
                rfvUsers.Text = "";
            }
        }
        else
        {
            rfvUsers.Visible = false;
        }

    }
    private bool SummaryInUse()
    {
        bool retVal;
        if (ViewState["SummaryInUse"] == null)
        {
            retVal = WebCommon.ControlTypeExists(this.Parent.Page, typeof(System.Web.UI.WebControls.ValidationSummary));
            ViewState["SummaryInUse"] = retVal;
        }
        else
        {
            retVal = (bool)ViewState["SummaryInUse"];
        }
        return retVal;
    }
    protected void ddUsers_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (NameChange != null)
        {
            NameChange(this, e);
        }
    }
    public void SelectUser(string userName)
    {
        //WebCommon.SelectByValue(ddUsers, userName);
        WebCommon.SelectListValue<string>(ddUsers, userName);
    }
    public string ADGroup
    {
        get { return gADGroupPath; }
        set { gADGroupPath = value; }
    }
    private string gOUPath;

    public string OUPath
    {
        get { return gOUPath; }
        set { gOUPath = value; }
    }

    public string SelectedUserName
    {
        get
        {
            string retVal = "";
            if (ddUsers.SelectedItem != null)
                retVal = ddUsers.SelectedValue;
            return retVal;
        }
    }
    public string SelectedDisplayName
    {
        get
        {
            string retVal = "";
            if (ddUsers.SelectedItem != null)
                retVal = ddUsers.SelectedItem.Text;
            return retVal;
        }
    }
    public ADUser SelectedUser
    {
        get
        {
            return BasePage.AD.GetUser(SelectedUserName);
        }
    }
    private BasePage gBasePage;
    public BasePage BasePage
    {
        get
        {
            try
            {
                if (gBasePage == null)
                    gBasePage = (BasePage)this.Page;
                return gBasePage;
            }
            catch (Exception exp)
            {
                throw new InvalidBasePageException(exp);
            }
        }
    }

    public int SelectedIndex
    {
        get { return ddUsers.SelectedIndex; }
        set { ddUsers.SelectedIndex = value; }
    }
    public ListItem SelectedItem
    {
        get { return ddUsers.SelectedItem; }
    }
    public ListItemCollection Users
    {
        get { return ddUsers.Items; }
    }
    public bool AutoPostBack
    {
        get { return ddUsers.AutoPostBack; }
        set { ddUsers.AutoPostBack = value; }
    }
    public bool InsertChooseItem
    {
        get { return gInsertChooseItem; }
        set { gInsertChooseItem = value; }
    }
    public string DisplayName
    {
        set { gDisplayName = value; }
        get { return gDisplayName; }
    }
    public bool Required
    {
        set { gRequired = value; }
        get { return gRequired; }
    }
    public bool NoNameSelected
    {
        get
        {
            bool retVal = false;
            if (ddUsers.SelectedValue == "" || ddUsers.SelectedIndex == -1)
            {
                retVal = true;
            }
            return retVal;
        }
    }
    public int Count
    {
        get { return ddUsers.Items.Count; }
    }
    public string CssClass
    {
        get { return lblDDName.Attributes["Class"]; }
        set { lblDDName.Attributes["Class"] = value; }
    }
    public UserDisplayFormat DisplayFormat
    {
        get { return gDisplayFormat; }
        set { gDisplayFormat = value; }
    }
    //not implemented and not used.  do it later if needed
    //public System.Web.UI.WebControls.SortDirection SortDir {
    //  get { return gSortDir; }
    //  set { gSortDir = value; }
    //}
    public string ChooseItemText
    {
        get
        {
            if (ViewState["ChooseItemText"] == null)
                ViewState.Add("ChooseItemText", CHOOSE_ITEM_TEXT);
            return ViewState["ChooseItemText"].ToString();
        }
        set { ViewState["ChooseItemText"] = value; }
    }
    public string ChooseItemValue
    {
        get
        {
            if (ViewState["ChooseItemValue"] == null)
                ViewState.Add("ChooseItemValue", "");
            return ViewState["ChooseItemValue"].ToString();
        }
        set { ViewState["ChooseItemValue"] = value; }
    }
    public bool RemoveChooseItemOnPostBack
    {
        get
        {
            if (ViewState["RemoveChooseItemOnPostBack"] == null)
                ViewState.Add("RemoveChooseItemOnPostBack", true);
            return Convert.ToBoolean(ViewState["RemoveChooseItemOnPostBack"]);
        }
        set { ViewState["RemoveChooseItemOnPostBack"] = value; }
    }
    private bool gAutoLoadUsers = true;

    public bool AutoLoadUsers
    {
        get { return gAutoLoadUsers; }
        set { gAutoLoadUsers = value; }
    }

    public bool Enabled
    {
        get { return ddUsers.Enabled; }
        set { ddUsers.Enabled = value; }
    }

    private bool gHideDisplayName;

    public bool HideDisplayName
    {
        get { return gHideDisplayName; }
        set { gHideDisplayName = value; }
    }

    public bool IsUserSelected
    {
        get { return (SelectedItem.Value != ChooseItemValue); }
    }
    public ListItemCollection Items
    {
        get { return ddUsers.Items; }
    }

    public string BoxClientID
    {
        get { return ddUsers.ClientID; }
    }

    public Unit Width
    {
        get { return ddUsers.Width; }
        set { ddUsers.Width = value; }
    }

    public override void Focus()
    {
        ddUsers.Focus();
    }

    public string ValidationGroup
    {
        get { return rfvUsers.ValidationGroup; }
        set { rfvUsers.ValidationGroup = value; }
    }

    public bool ReadOnly
    {
        get
        {
            if (ViewState["ReadOnly"] == null)
                ViewState.Add("ReadOnly", false);
            return (bool)ViewState["ReadOnly"];
        }
        set { ViewState["ReadOnly"] = value; }
    }

    public short TabIndex
    {
        get { return ddUsers.TabIndex; }
        set { ddUsers.TabIndex = value; }
    }


    public event System.EventHandler NameChange;
}
