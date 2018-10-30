using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using SygmaFramework;

/// <summary>
/// Summary description for SystemStatus
/// </summary>
public class SystemStatus {
	private string gFileName;
	private string gUserName = "Auto";
	private XmlDocument gDocument = new XmlDocument();
	private XmlNode gActiveStatusNode;
	//private string gID;
	//private string gName;
	//private string gColor;
	//private string gDescription;
	//private string gPrevStatusID;
	//private bool gIsSelectable;

	public SystemStatus(string appRoot) {
		SetFileName(appRoot);
		SetDocument();
		SetCurrentStatus();
	}
	public SystemStatus(string appRoot, string userName) {
		SetFileName(appRoot);
		SetDocument();
		SetCurrentStatus();
		gUserName = userName;
	}

	private void SetFileName(string appRoot) {
		XMLConfig config = new XMLConfig();
		string pathOffRoot = config.GetAppSetting("systemstatus", "config", "pathoffroot");
		gFileName = System.IO.Path.Combine(appRoot, pathOffRoot);
	}
	private void SetDocument() {
		Document.Load(gFileName);
	}
	private void SetCurrentStatus() {
		XmlNode xmlNode = Document.SelectSingleNode("//SysStatus/Levels/Level[@active = 1]");
		if (xmlNode != null) { //if there is no active, then set "ok" status
			SetCurrentStatus(xmlNode);
		} else {
			SetStatus("ok");
		}
	}
	private void SetCurrentStatus(XmlNode node) {
		try {
			gActiveStatusNode = node;
		} catch (Exception exp) {
			string xml = (node != null) ? node.OuterXml : "Current Node Not Set";
			throw new Exception(string.Format("Getting Current Node values - {0}", xml), exp);
		}
	}

	#region Public Members

	#region Methods

	public XmlNode SetStatus(string id) {
		return SetStatus(id, null);
	}
	public XmlNode SetStatus(string id, bool retainPrevStatus) {
		return SetStatus(id, null, retainPrevStatus);
	}
	public XmlNode SetStatus(string id, string description) {
		return SetStatus(id, description, false);
	}
	public XmlNode SetStatus(string id, string description, bool retainPrevStatus) {
		Document.SelectSingleNode("//LastUpdated").InnerText = DateTime.Now.ToString();
		Document.SelectSingleNode("//LastUpdatedBy").InnerText = gUserName;

		XmlNodeList nodes = Document.SelectNodes("//SysStatus/Levels/Level");
		foreach (XmlNode node in nodes) {
			UpdateStatusActive(node, false);
		}
		XmlNode selectedNode = Document.SelectSingleNode(string.Format("//SysStatus/Levels/Level[@id = '{0}']", id.ToLower()));
		if (selectedNode != null) {
			UpdateStatusActive(selectedNode, true);
			UpdateStatusDescription(selectedNode, description);
			if (retainPrevStatus)
				UpdatePreviousStatusId(selectedNode, ID);
		} else {
			//add code to add new status, but not right now
		}
		Document.Save(FileName);
		SetCurrentStatus(selectedNode);
		return selectedNode;
	}

	public void UpdateStatusDescription(string id, string description) {
		XmlNode node = Document.SelectSingleNode(string.Format("//SysStatus/Levels/Level[@id = '{0}']", id.ToLower()));
		UpdateStatusDescription(node, description);
	}
	private void UpdateStatusDescription(XmlNode node, string description) {
		if (description != null) {
			node["description"].InnerText = description;
			node.OwnerDocument.Save(FileName);
		}
	}
	public void UpdateStatusActive(string id, bool active) {
		XmlNode node = Document.SelectSingleNode(string.Format("//SysStatus/Levels/Level[@id = '{0}']", id.ToLower()));
		UpdateStatusActive(node, active);
	}
	private void UpdateStatusActive(XmlNode node, bool active) {
		node.Attributes["active"].Value = (active) ? "1" : "0";
		node.OwnerDocument.Save(FileName);
	}
	public void UpdatePreviousStatusId(string id, string previousId) {
		XmlNode node = Document.SelectSingleNode(string.Format("//SysStatus/Levels/Level[@id = '{0}']", id.ToLower()));
		UpdatePreviousStatusId(node, previousId);
	}
	private void UpdatePreviousStatusId(XmlNode node, string previousId) {
		if (node.Attributes["previousStatusId"] == null)
			node.Attributes.Append(Document.CreateAttribute("previousStatusId"));
		node.Attributes["previousStatusId"].Value = previousId;
		node.OwnerDocument.Save(FileName);
	}
	public string[] GetStatusIds() {
		XmlNodeList nodes = this.StatusNodes;
		string[] nodeIds = new string[nodes.Count];
		int i = 0;
		foreach (XmlNode node in nodes) {
			nodeIds[i++] = node.Attributes["id"].Value;
		}
		return nodeIds;
	}
	public string[] GetStatusNames() {
		XmlNodeList nodes = this.StatusNodes;
		string[] nodeNames = new string[nodes.Count];
		int i = 0;
		foreach (XmlNode node in nodes) {
			nodeNames[i++] = node["name"].InnerText;
		}
		return nodeNames;
	}
	public string GetStatusDescription(string id) {
		string retVal = "";
		XmlNode xmlNode = Document.SelectSingleNode(string.Format("//SysStatus/Levels/Level[@id='{0}']/description", id));
		if (xmlNode != null)
			retVal = xmlNode.InnerText;
		return retVal;
	}

	#endregion

	#region Properties

	public XmlNodeList StatusNodes {
		get { return Document.SelectNodes("//SysStatus/Levels/Level[@selectable = 'true']"); }
	}
	public string FileName {
		get { return gFileName; }
	}
	public XmlDocument Document {
		get { return gDocument; }
	}
	public XmlNode ActiveStatusNode {
		get { return gActiveStatusNode; }
	}
	public DateTime LastUpdated {
		get { return Convert.ToDateTime(Document.SelectSingleNode("//LastUpdated").InnerText); }
	}
	public string LastUpdatedBy {
		get { return Document.SelectSingleNode("//LastUpdatedBy").InnerText; }
	}


	#region Active Node

	public string ID {
		get { return ActiveStatusNode.Attributes["id"].Value; }
	}
	public string Name {
		get { return ActiveStatusNode["name"].InnerText; }
	}
	public string Color {
		get { return ActiveStatusNode["color"].InnerText; }
	}
	public string Description {
		get { return ActiveStatusNode["description"].InnerText; }
	}
	public string PreviousStatusID {
		get {
			string prevStatusId = null;
			if (ActiveStatusNode.Attributes["previousStatusId"] != null)
				prevStatusId = ActiveStatusNode.Attributes["previousStatusId"].Value;
			return prevStatusId;
		}
	}
	public bool IsSelectable {
		get { return Convert.ToBoolean(gActiveStatusNode.Attributes["selectable"].Value); }
	}

	#endregion

	#endregion

	#endregion

}
