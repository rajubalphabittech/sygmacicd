using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using atm;
using SygmaFramework;
using SygmaFramework.Reports;

public partial class Apps_ATM_Payroll_Forms_SendToADP : ATMPage
{
    readonly MailMessage _email = new MailMessage();

    protected override void LoadATMPage()
    {
        if (IsPostBack) return;

        var dsCenter = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);
        ddSygmaCenterNo.DataSource = dsCenter.Tables[2].DefaultView;
        ddSygmaCenterNo.DataBind();

        if (dsCenter.Tables[2].Rows.Count > 0)
        {
            var ds = ATMDB.GetDataSet("up_p_getweekending", dsCenter.Tables[2].Rows[0][0].ToString());


            var dt = new DataTable();
            dt.Columns.Add("FiscalWeekending");
            dt.Columns.Add("FieldValue");
            var count = ds.Tables[0].Rows.Count;

            if (count != 0)
            {
                for (var i = 0; i < count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i][0] = Convert.ToDateTime(ds.Tables[0].Rows[i][0]).ToShortDateString();
                    dt.Rows[i][1] = Convert.ToDateTime(ds.Tables[0].Rows[i][1]).ToShortDateString();
                }
            }

            ddlWeekending.DataSource = dt;
            ddlWeekending.DataBind();
            btnSend.Enabled = dt.Rows.Count > 0;

            ds.Clear();
        }
        if (Session["AdpSuccess"] == null) return;

        if (Convert.ToBoolean(Session["AdpSuccess"].ToString()))
            Javascript.Notify("ADP report has been sent to ADP successfully");

        Session.Remove("AdpSuccess");
    }

    /// <summary>
    /// Method for emailing the ADP report
    /// </summary>
    private void sendEmail()
    {
        try
        {
            var from = AppSettings.GetAppSetting("adp", "emails", "from");
            var dsEmail = ATMDB.GetDataSet("up_p_getAdpToEmail", ddSygmaCenterNo.SelectedValue);
            var toEmail = dsEmail.Tables[0].Rows[0][0].ToString();
            var subject = "ADP Report - Center" + ddSygmaCenterNo.SelectedValue;
            var bccEmails = AppSettings.GetAppSetting("ADP", "emails", "bcc");

            var body = new StringBuilder();
            body.AppendFormat("Attached is the Sygma ADP Report {0}", Environment.NewLine);
            body.AppendFormat("Center: {0} ({1}) {2}", ddSygmaCenterNo.SelectedItem, ddSygmaCenterNo.SelectedValue,
                Environment.NewLine);
            body.AppendFormat("Weekending Date: {0} {1}", ddlWeekending.SelectedItem, Environment.NewLine);

            Email.AddEmails(_email.To, toEmail);
            _email.From = new MailAddress(from);
            if (bccEmails != string.Empty) Email.AddEmails(_email.Bcc, bccEmails);

            _email.Priority = MailPriority.High;
            _email.Subject = subject;
            _email.IsBodyHtml = false;
            _email.Body = body.ToString();
            buildCSVs();

            Email.Send(_email);
            ATMDB.GetDataSet("up_p_addAdpLog", Convert.ToInt32(ddSygmaCenterNo.SelectedValue),
                Convert.ToDateTime(ddlWeekending.SelectedItem.ToString()), UserName);
            Session["AdpSuccess"] = true;

            Javascript.Notify("ADP report has been sent to ADP successfully");
        }
        catch (Exception exp)
        {
            throw new Exception("Error Sending ADP Report", exp);
        }
    }

    /// <summary>
    /// Build the excel adp report and send the temporary stored path of the adp report
    /// </summary>
    /// <returns> adp report path in temporary folder </returns>
    private void buildCSVs()
    {
        try
        {
            var centerNo = Convert.ToInt32(ddSygmaCenterNo.SelectedValue);

            var weekend = Convert.ToDateTime(ddlWeekending.SelectedItem.ToString());
            var ds = ATMDB.GetDataSet("up_p_sendToAdp", centerNo, weekend, UserName,
                AppSettings.GetAppSetting("adpmainreport", "ratetype", "required"),
                AppSettings.GetAppSetting("adpbonusreport", "ratetype", "required"),
                AppSettings.GetAppSetting("adpexpensereport", "ratetype", "required"),
                AppSettings.GetAppSetting("adpptoreport", "ratetype", "required"));

            var tableCount = ds.Tables.Count;
            for (var i = 0; i < tableCount; i++)
            {
                const int TABLE_INDEX_ADP_REPORT = 0;
                const int TABLE_INDEX_BONUS_REPORT_B = 1;
                const int TABLE_INDEX_BONUS_REPORT_C = 2;
                const int TABLE_INDEX_EXPENSE_REPORT = 3;
                string baseFileName;
                switch (i)
                {
                    case TABLE_INDEX_ADP_REPORT:
                        baseFileName = "ADP_Main";
                        break;
                    case TABLE_INDEX_BONUS_REPORT_B:
                        baseFileName = "ADP_Bonus_Tax_B";
                        break;
                    case TABLE_INDEX_BONUS_REPORT_C:
                        baseFileName = "ADP_Bonus_Tax_C";
                        break;
                    case TABLE_INDEX_EXPENSE_REPORT:
                        baseFileName = "ADP_Expense";
                        break;
                    default:
                        baseFileName = "ADP_Pto";
                        break;
                }

                var dataTable = ds.Tables[i];

                var columnCount = dataTable.Columns.Count;

                var hasEarnings = false;

                //write header
                for (var c = 0; c < columnCount; c++)
                {
                    if (dataTable.Columns[c].ColumnName.StartsWith("EarningCode"))
                    {
                        dataTable.Columns[c].Caption = "Earnings 3 Code";
                        hasEarnings = true;
                    }
                    else if (dataTable.Columns[c].ColumnName.StartsWith("EarningAmount"))
                    {
                        dataTable.Columns[c].Caption = "Earnings 3 Amount";
                        hasEarnings = true;
                    }
                    else if (dataTable.Columns[c].ColumnName.StartsWith("ExpenseCode"))
                    {
                        dataTable.Columns[c].Caption = "Adjust Ded Code";
                        hasEarnings = true;
                    }
                    else if (dataTable.Columns[c].ColumnName.StartsWith("ExpenseAmount"))
                    {
                        dataTable.Columns[c].Caption = "Adjust Ded Amount";
                        hasEarnings = true;
                    }
                    else if (dataTable.Columns[c].ColumnName.StartsWith("PayNumber"))
                    {
                        dataTable.Columns[c].Caption = "Pay #";
                    }
                    else if (dataTable.Columns[c].ColumnName.StartsWith("TaxFrequency"))
                    {
                        dataTable.Columns[c].Caption = "Tax Frequency";
                    }
                    else
                    {
                        dataTable.Columns[c].Caption = dataTable.Columns[c].ColumnName;
                    }
                }

                //write data
                var fullFileName = getExcelFileFullName(baseFileName);
                using (var writer = new StreamWriter(fullFileName))
                    Rfc4180Writer.WriteDataTable(dataTable, writer, true);

                if (hasEarnings)
                {
                    _email.Attachments.Add(new Attachment(fullFileName));
                }
            }
        }
        catch (Exception exp)
        {
            throw new Exception("Error creating ADP Report Excel", exp);
        }
    }

    private string getExcelFileFullName(string baseFileName)
    {
        const char PAD_CHAR = '0';
        var tempDir = Path.Combine(Path.GetTempPath(), baseFileName);
        if (!Directory.Exists(tempDir))
            Directory.CreateDirectory(tempDir);
        var ffn = string.Concat(baseFileName + "_Center" + ddSygmaCenterNo.SelectedValue + "_", DateTime.Now.Year,
            DateTime.Now.Month.ToString().PadLeft(2, PAD_CHAR),
            DateTime.Now.Day.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Hour.ToString().PadLeft(2, PAD_CHAR),
            DateTime.Now.Minute.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Second.ToString().PadLeft(2, PAD_CHAR),
            ".csv");
        return Path.Combine(tempDir, ffn);
    }

    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        var sygmaCenterNo = Convert.ToInt32(ddSygmaCenterNo.SelectedValue);
        var dataSet = ATMDB.GetDataSet("up_p_getweekending", sygmaCenterNo);

        var dt = new DataTable();
        dt.Columns.Add("FiscalWeekending");
        dt.Columns.Add("FieldValue");
        var count = dataSet.Tables[0].Rows.Count;

        if (count != 0)
        {
            for (var i = 0; i < count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i][0] = Convert.ToDateTime(dataSet.Tables[0].Rows[i][0]).ToShortDateString();
                dt.Rows[i][1] = Convert.ToDateTime(dataSet.Tables[0].Rows[i][0]).ToShortDateString();
            }
        }

        ddlWeekending.DataSource = dt;
        ddlWeekending.DataBind();

        btnSend.Enabled = dt.Rows.Count > 0;
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            btnSend.Enabled = false;
            sendEmail();

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ADP Sent", "window.location.replace(window.location);", true);
        }
        catch (Exception exp)
        {
            throw new Exception("Error creating ADP Report Excel - Send Click", exp);
        }
    }
}
