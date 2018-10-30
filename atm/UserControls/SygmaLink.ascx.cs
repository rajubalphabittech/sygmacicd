
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Sygmanet;

/// <summary>
///		Summary description for SygmaLink.
/// </summary>
public partial class UserControls_SygmaLink : System.Web.UI.UserControl {
	private const string DOC_IMAGE = "~/images/link_doc.gif";
	private const string FOLDER_IMAGE = "~/images/link_folder.gif";
	private const string FORM_IMAGE = "~/images/sygma_small.gif";
	private const string URL_IMAGE = "~/images/link_url.gif";

	private string gNavUrl = "";
	private string gImgUrl = "";
	private string gText = "";
	private string gToolTip = "";
	private bool gViewImage = true;
	private bool gUsePopUp = false;
	private string gPUWidth = "-1";  //default to max
	private string gPUHeight = "-1"; //default to max
	private bool gPUMenus = true;
	private bool gPUResizeable = true;
	private bool gPUScrollBars = true;
	private bool gPUToolBar = true;
	private LinkType gType = LinkType.Form;
	private bool gJSRequested = false;

	protected void Page_Load(object sender, System.EventArgs e) {
		string url = BuildLinkUrl();
		hlLink.NavigateUrl = url;
		hlLink.Text = Text;
		hlLink.ToolTip = ToolTip;
		if (UsePopUp && !gJSRequested) {
			hlLink.Target = "_blank";
			hlSygmaImg.Target = "_blank";
		}
		if (DisplayImage) {
			hlSygmaImg.NavigateUrl = url;
			//hlSygmaImg.ImageUrl = GetImage();
			hlSygmaImg.ToolTip = ToolTip;
			SetImage();
		}
	}
	private string SetImage() {
		string retVal = ImageUrl;
		if (ImageUrl == "") {
			switch (Type) {
				case LinkType.Url:
					int i = NavigateUrl.IndexOf('?');
					diIcon.FileName = (i > -1) ? NavigateUrl.Substring(0, i) : NavigateUrl;
					break;
				case LinkType.Document:
					diIcon.Visible = true;
					diIcon.FileName = NavigateUrl;
					break;
				case LinkType.Folder:
					//hlSygmaImg.ImageUrl = FOLDER_IMAGE;
					diIcon.Visible = true;
					diIcon.Extension = "folder";
					break;
				default:
					//hlSygmaImg.ImageUrl = FORM_IMAGE;
					diIcon.Visible = true;
					diIcon.Extension = "form";
					break;
			}
		}
		return retVal;
	}
	private string BuildLinkUrl() {
		string url = "";
		if (gUsePopUp && gJSRequested) {
			string popUpUrl = (NavigateUrl.StartsWith("~/")) ? MakeRelativePath(NavigateUrl) : NavigateUrl;
			string width = (Width.ToLower() == "max") ? "-1" : Width;
			string height = (Height.ToLower() == "max") ? "-1" : Height;
			string resizable = (Resizeable) ? "1" : "0";
			string menus = (Menus) ? "1" : "0";
			string scrollBars = (ScrollBars) ? "1" : "0";
			string toolBar = (ToolBar) ? "1" : "0";
			url = string.Format("javascript:OpenWindow('{0}', {1}, {2}, {3}, {4}, {5}, {6});", popUpUrl.Replace("'", @"\'"), width, height, resizable, toolBar, scrollBars, menus);
		} else {
			if (WebCommon.IsWebUrl(gNavUrl)) {
				url = gNavUrl;
			} else {
				url = string.Format("{0}/{1}", GetParentRelativePath(), gNavUrl);
			}
		}

		hlSygmaImg.Visible = DisplayImage;
		litSpace.Visible = DisplayImage;
		return url;
	}

	public string MakeRelativePath(string url) {
		return url.Substring(1);
	}

	public string GetParentRelativePath() {
		string parent = this.Parent.Page.Request.AppRelativeCurrentExecutionFilePath;
		return parent.Substring(0, parent.LastIndexOf('/'));
	}

	public string Text {
		get { return gText; }
		set { gText = value; }
	}
	public string NavigateUrl {
		get { return gNavUrl; }
		set { gNavUrl = value; }
	}

	public string ImageUrl {
		get { return gImgUrl; }
		set { gImgUrl = value; }
	}
	public string ToolTip {
		get { return gToolTip; }
		set { gToolTip = value; }
	}
	public bool DisplayImage {
		get { return gViewImage; }
		set { gViewImage = value; }
	}
	public bool UsePopUp {
		get { return gUsePopUp; }
		set { gUsePopUp = value; }
	}
	public string Width {
		get { return gPUWidth; }
		set {
			gJSRequested = true;
			gPUWidth = value;
		}
	}
	public string Height {
		get { return gPUHeight; }
		set {
			gJSRequested = true;
			gPUHeight = value;
		}
	}
	public bool ToolBar {
		get { return gPUToolBar; }
		set {
			if (!value)
				gJSRequested = true;
			gPUToolBar = value;
		}
	}
	public bool Menus {
		get { return gPUMenus; }
		set {
			if (!value)
				gJSRequested = true;
			gPUMenus = value;
		}
	}
	public bool Resizeable {
		get { return gPUResizeable; }
		set {
			if (!value)
				gJSRequested = true;
			gPUResizeable = value;
		}
	}
	public bool ScrollBars {
		get { return gPUScrollBars; }
		set {
			if (!value)
				gJSRequested = true;
			gPUScrollBars = value;
		}
	}
	public LinkType Type {
		get { return gType; }
		set { gType = value; }
	}
}
