using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UserControls_IndexList : System.Web.UI.UserControl {

	private string gStartingNodeUrl;
	private int gMaxDataBindDepth = 1;
	private bool gLimitedAccess = false;

	protected void Page_Load(object sender, EventArgs e) {
		if (gStartingNodeUrl != null) {
			siteMap.StartingNodeUrl = gStartingNodeUrl;
		} else {
			string qs = (Request.QueryString.Count != 0) ? string.Concat("?", Request.QueryString) : "";
			siteMap.StartingNodeUrl = string.Concat(Page.AppRelativeVirtualPath, qs);
		}
		tvList.MaxDataBindDepth = gMaxDataBindDepth;
		trLimitedAccess.Visible = LimitedAccess;
	}

	protected void Page_PreRender(object sender, EventArgs e) {
		//string val = tvList.Nodes[0].Value;
	}
				
	public string StartingNodeUrl {
		get { return gStartingNodeUrl; }
		set { gStartingNodeUrl = value; }
	}

	public int MaxDataBindDepth {
		get { return gMaxDataBindDepth; }
		set { gMaxDataBindDepth = value; }
	}

	public bool LimitedAccess {
		get { return gLimitedAccess; }
		set { gLimitedAccess = value; }
	}
	private List<int[]> gNodesToRemove;

	public List<int[]> NodesToRemove {
		get { return gNodesToRemove; }
		set { gNodesToRemove = value; }
	}

	protected void tvList_DataBound(object sender, System.EventArgs e) {
		if (gNodesToRemove != null) {
			gNodesToRemove.Sort(ReverseSortIntArray);
			foreach (int[] nodeSet in gNodesToRemove) {
				int rootIndex = nodeSet[0];
				if (nodeSet.Length != 2) {
					tvList.Nodes.RemoveAt(rootIndex);
				} else {
					tvList.Nodes[rootIndex].ChildNodes.RemoveAt(nodeSet[1]);
				}
			}
		}
	}

	public int ReverseSortIntArray(int[] x, int[] y) {
		if (x == null) {
			if (y == null) {
				return 0;
			} else {
				return 1;
			}
		} else {
			if (y == null) {
				return -1;
			} else {
				if (x[0] > y[0]) {
					return -1;
				} else if (x[0] == y[0]) {
					if (x.Length > 1){
						if (y.Length > 1) {
							if (x[1] > y[1]) {
								return -1;
							} else if (x[1] == y[1]) {
								return 0;
							} 
						} 
					} 
				} 
			}
		}
		return 1;
	}
}



