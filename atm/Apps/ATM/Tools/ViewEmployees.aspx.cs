using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_Tools_ViewEmployees : ATMPage
{
    private DataTable dtClassifications;
    private bool gIsApsEnabledSygmaCenterNo;
    protected override void LoadATMPage()
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            LoadCenters();
            DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
            dtClassifications = dsClassification.Tables[1];
        }
    }

    private void SetPageVariables()
    {
        AddClientVariable("gUserName", UserName);
    }

    protected void ddProgSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CenterSelectionIndexChanged();
    }

    protected void CenterSelectionIndexChanged()
    {
        if (ProgSortExpression == "")
        {
            ProgSortExpression = "HireDate";
            ProgSortDir = SortDirection.Descending;
        }
        SetEmployees();
    }

    protected void txtProgName_TextChanged(object sender, EventArgs e)
    {
        SetEmployees();
    }
    
    private void LoadCenters()
    {
        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);

        ddProgSygmaCenterNo.DataSource = dv;
        ddProgSygmaCenterNo.DataBind();
        ddProgSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        if (ddProgSygmaCenterNo.Items.Count == 2)
        {
            ddProgSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }
    }

    private void SetEmployees()
    {
        if (ddProgSygmaCenterNo.SelectedIndex > 0)
        {
            gIsApsEnabledSygmaCenterNo = Convert.ToBoolean(ATMDB.GetScalar("up_p_isApsEnabledCenter", ddProgSygmaCenterNo.SelectedValue));
            DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
            dtClassifications = dsClassification.Tables[1];
            DataView dv = ATMDB.GetDataView("up_getEmployees", UserName, ddProgSygmaCenterNo.SelectedValue);
            AddSort(dv);
            AddNameFilter(dv);
            gvEmployees.DataSource = dv;
            gvEmployees.DataBind();
            pnlProgression.Visible = true;
            lblEmployeeCount.Text = dv.Count.ToString();
        }
        else
        {
            pnlProgression.Visible = false;
        }
    }

    private void AddSort(DataView dv)
    {
        if (ProgSortExpression != "")
            dv.Sort = string.Format("{0} {1}", ProgSortExpression, WebCommon.GetDBSortDirection(ProgSortDir));
    }
    private void AddNameFilter(DataView dv)
    {
        string[] names = txtProgName.Text.Trim().Replace(".", "").Replace(",", "").Replace("'", "''").Replace("%", "").Replace("*", "").Split(' ');
        if (names.Length > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
            {
                if (sb.Length > 0)
                    sb.Append(" OR ");
                sb.AppendFormat("WebDisplay like '%{0}%'", name);
            }
            dv.RowFilter = sb.ToString();
        }
    }

    protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                DataRowView row = (DataRowView)e.Row.DataItem;
                Label lblGuaranteedPay = (Label)e.Row.FindControl("lblGuaranteedPay");
                Label lblProgRate = (Label)e.Row.FindControl("lblProgRate");
                Label lblAPSException = (Label)e.Row.FindControl("lblAPSExcptn");
                Label lblClassification = (Label)e.Row.FindControl("lblClassification");

                if (!Convert.IsDBNull(row["GuaranteedPay"]))
                {
                    lblGuaranteedPay.Text = Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##");
                }
                else
                {
                    lblGuaranteedPay.Text = "";
                }

                if (!Convert.IsDBNull(row["ProgressionRate"]))
                {
                    lblProgRate.Text = (Convert.ToDecimal(row["ProgressionRate"]) * 100).ToString("0.####");
                }
                else
                {
                    lblProgRate.Text = "";
                }

                if (!Convert.IsDBNull(row["ApsException"]))
                {
                    lblAPSException.Text = Convert.ToBoolean(row["ApsException"]) ? "Enabled" : "Disabled";
                }
                if (!Convert.IsDBNull(row["ClassificationId"]))
                {
                    string classificationId = row["ClassificationId"].ToString();
                    string filter = String.Format("ClassificationId = {0}", classificationId);
                    DataRow[] drClass = dtClassifications.Select(filter);
                    string classification = drClass[0]["ClassificationName"].ToString();
                    if (classification != "")
                    {
                        lblClassification.Text = classification;
                    }
                    else
                    {
                        lblClassification.Text = "";
                    }

                }
            break;
        }
    }

    protected void gvEmployees_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == ProgSortExpression)
        {
            ProgSortDir = (ProgSortDir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        else
        {
            ProgSortExpression = e.SortExpression;
            ProgSortDir = SortDirection.Ascending;
        }
        SetEmployees();
    }

    //View States
    public string ProgSortExpression
    {
        get
        {
            if (ViewState["ProgSortExpression"] == null)
                ViewState.Add("ProgSortExpression", "");
            return (string)ViewState["ProgSortExpression"];
        }
        set { ViewState["ProgSortExpression"] = value; }
    }
    public SortDirection ProgSortDir
    {
        get
        {
            if (ViewState["ProgSortDir"] == null)
                ViewState.Add("ProgSortDir", SortDirection.Ascending);
            return (SortDirection)ViewState["ProgSortDir"];
        }
        set { ViewState["ProgSortDir"] = value; }
    }

}