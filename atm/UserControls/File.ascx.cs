using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SygmaFramework;

public partial class UserControls_File : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void Page_PreRender(object sender, EventArgs e) {
		SetView();
	}

	private void SetView() {
		if (HasLoadedFile) {
			if (SetFileLink()) {
				fuFile.Visible = false;
				hlFile.Visible = true;
				hlRemoveFile.Visible = (IsEditable);
				if (hlRemoveFile.Visible && ConfirmRemoval)
					hlRemoveFile.OnClientClick = "return confirm('Are you sure you want to remove this file?');";
			} else {
				LoadedFileName = null;
				SetView();
			}
			rfvFile.Visible = false;
		} else if (IsEditable) {
			fuFile.Visible = true;
			hlFile.Visible = false;
			hlRemoveFile.Visible = false;
			rfvFile.Visible = IsRequired;
		} else {
			fuFile.Visible = false;
			hlFile.Visible = false;
			hlRemoveFile.Visible = false;
			rfvFile.Visible = false;
		}
		rfvFile.ErrorMessage = string.Format("'{0}' is required!", DisplayName);
	}
	
	private bool SetFileLink() {
		if (PhysicalFullDirectoryPath != null && LoadedFileName != null) {
			string fullFilePath = Path.Combine(PhysicalFullDirectoryPath, LoadedFileName);
			if (File.Exists(fullFilePath)) {
				//string pageFolderPath = Path.GetDirectoryName(Request.Url.AbsolutePath).Replace("\\", "/");
				//fullFilePath = fullFilePath.Replace("\\", "/"); //replace all the filesystem separators w/ web separators

				string fileWebPath = fullFilePath.Replace(Request.PhysicalApplicationPath, "").Replace("\\", "/");   //trim off everything below the web app root
				//need to UrlEncode the
				fileWebPath = fileWebPath.Replace(LoadedFileName, HttpUtility.UrlEncodeUnicode(LoadedFileName).Replace("+", "%20"));

				hlFile.NavigateUrl = string.Concat("~/", fileWebPath);
				hlFile.Text = LoadedFileName;
				return true;
			}
		}
		return false;
	}

	public bool SaveFile() {
		return SaveFile(PhysicalFullDirectoryPath, false);
	}
	public bool SaveFile(bool forceOverwrite) {
		return SaveFile(PhysicalFullDirectoryPath, forceOverwrite);
	}
	public bool SaveFile(string physicalFullDirectoryPath, bool forceOverwrite) {
		PhysicalFullDirectoryPath = physicalFullDirectoryPath;
		if (fuFile.HasFile) {
			string fileFullPath = Path.Combine(physicalFullDirectoryPath, fuFile.FileName);
			if (!File.Exists(fileFullPath) || forceOverwrite) {
				fuFile.SaveAs(fileFullPath);
				LoadedFileName = fuFile.FileName;
				return true;
			}
		}
		return false;
	}

	public void Validate() {
		rfvFile.Validate();
	}

	public string UploadFileName {
		get { return fuFile.FileName; }
	}

	public bool UploadHasFile {
		get { return fuFile.HasFile; }
	}

	public bool HasLoadedFile {
		get { return (LoadedFileName != null); }
	}

	public string LoadedFileName {
		get {
			//if user has set a file then use that as the filename
			if (ViewState["FileName"] == null)
				ViewState.Add("FileName", null);
			return (string)ViewState["FileName"];
		}
		set { ViewState["FileName"] = value; }
	}

	public string PhysicalFullDirectoryPath {
		get {
			if (ViewState["PhysicalFullDirectoryPath"] == null)
				ViewState.Add("PhysicalFullDirectoryPath", null);
			return (string)ViewState["PhysicalFullDirectoryPath"];
		}
		set {
			if (!Directory.Exists(value))
				Directory.CreateDirectory(value);
			ViewState["PhysicalFullDirectoryPath"] = value;
		}
	}

	public string PhysicalFullFilePath {
		get { return Path.Combine(PhysicalFullDirectoryPath, LoadedFileName); }
	}

	public Unit Width {
		get { return fuFile.Width; }
		set {
			fuFile.Width = value;
			fuFile.Attributes.Add("Size", Convert.ToInt16((value.Value / 6)).ToString()); //add this for IE
		}
	}

	public bool IsRequired {
		get { return rfvFile.Visible; }
		set { rfvFile.Visible = value; }
	}

	public string RequiredErrorMessage {
		get { return rfvFile.ErrorMessage; }
		set { rfvFile.ErrorMessage = value; }
	}

	public string ValidationGroup {
		get { return rfvFile.ValidationGroup; }
		set { rfvFile.ValidationGroup = value; }
	}

	public string Name {
		get {
			if (ViewState["Name"] == null)
				ViewState.Add("Name", "File");
			return (string)ViewState["Name"];
		}
		set { ViewState["Name"] = value; }
	}

	public bool IsEditable {
		get {
			if (ViewState["IsEditable"] == null)
				ViewState.Add("IsEditable", true);
			return (bool)ViewState["IsEditable"];
		}
		set { ViewState["IsEditable"] = value; }
	}

	public bool ConfirmRemoval {
		get {
			if (ViewState["ConfirmRemoval"] == null)
				ViewState.Add("ConfirmRemoval", true);
			return (bool)ViewState["ConfirmRemoval"];
		}
		set { ViewState["ConfirmRemoval"] = value; }
	}

	private string gDisplayName;

	public string DisplayName {
		get { return gDisplayName; }
		set { gDisplayName = value; }
	}

	public bool IsValid {
		get { return rfvFile.IsValid; }
	}

	public bool OpenInNewWindow {
		get { return hlFile.Target == "_blank"; }
		set {
			if (value)
				hlFile.Target = "_blank";
			else
				hlFile.Target = "_self";
		}
	}


	public event EventHandler RemoveFile;

	protected void hlRemoveFile_Click(object sender, EventArgs e) {
		try {
			if (File.Exists(PhysicalFullFilePath))
				File.Delete(PhysicalFullFilePath);
			LoadedFileName = null;
			if (RemoveFile != null)
				RemoveFile(sender, e);
		} catch (Exception exp) {
			throw new Exception(string.Format("Error Removing File: {0}", PhysicalFullFilePath), exp);
		}
	}
}
