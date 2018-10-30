using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_RowCountBar : System.Web.UI.UserControl {

	#region EventHandlers

	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void Page_PreRender(object sender, EventArgs e) {
		if (IncludeAllSelection)
			if (ddPageSize.Items.FindByValue("-1") == null)
				ddPageSize.Items.Add(new ListItem("All", "-1"));
	}

	protected void ddPageSize_SelectedIndexChanged(object sender, EventArgs e) {
		if (PageSizeChanged != null)
			PageSizeChanged(sender, e);
	}

	#endregion EventHandlers

	#region Public Properties

	public bool IncludeAllSelection {
		get {
			if (ViewState["IncludeAllSelection"] == null)
				ViewState.Add("IncludeAllSelection", false);
			return (bool)ViewState["IncludeAllSelection"];
		}
		set { ViewState["IncludeAllSelection"] = value; }
	}

	public int PageSize {
		get {
			int ps = Convert.ToInt32(ddPageSize.SelectedValue);
			return (ps != -1) ? ps : ItemCount;
		}
		set { WebCommon.SelectListValue<int>(ddPageSize, value); }
	}

	public int ItemCount {
		get { return Convert.ToInt32(lblItemCount.Text); }
		set { lblItemCount.Text = value.ToString(); }
	}

	public string Width {
		get { return tblMain.Width; }
		set { tblMain.Width = value; }
	}


	#endregion Public Properties


	#region Events

	public event EventHandler PageSizeChanged;

	#endregion Events


}
