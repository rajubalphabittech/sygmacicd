using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using atm;
using atm.helpers;
using atm.Helpers;
using Newtonsoft.Json.Serialization;
using Sygma.Framework.ClosedXML.Excel;
using SygmaFramework;
using SygmaFramework.Spreadsheet;


/// <summary>
/// Summary description for ATMPage
/// </summary>
public abstract class ATMReportPage : ATMPage
{
	protected string SygmaCenterNo;

	protected void GetSelectedCenters(string newCenterNumber)
	{
		if (SygmaCenterNo == null)
		{
			SygmaCenterNo = newCenterNumber;
		}
		else
		{
			SygmaCenterNo = SygmaCenterNo + "," + newCenterNumber;
		}
	}

	/// <summary>
	/// Method for generating the excel report and return the temp saved path
	/// </summary>
	/// <param name="reportId">Report#</param>
	/// <param name="parms">Parameters need for the report stored procedure</param>
	/// <returns>a boolean indicating if the report had records</returns>
	protected bool BuildExcelReport(string reportId, params object[] parms)
	{
		object val = null;
		try
		{
			var dsConfig = ATMDB.GetDataSet("up_p_getReportConfig", reportId);
			const int TABLE_INDEX_REPORT_INFO = 0;
			const int TABLE_INDEX_SHEET_NAMES = 1;

			const int COLUMN_INDEX_PROC_NAME = 0;
			const int COLUMN_INDEX_TEMPLATE_NAME = 1;
			const int COLUMN_INDEX_REPORT_NAME = 2;

			var storedProcedureName = dsConfig.Tables[TABLE_INDEX_REPORT_INFO].Rows[0][COLUMN_INDEX_PROC_NAME].ToString();
			var excelTemplateName =
					dsConfig.Tables[TABLE_INDEX_REPORT_INFO].Rows[0][COLUMN_INDEX_TEMPLATE_NAME].ToString();
			var reportName = dsConfig.Tables[TABLE_INDEX_REPORT_INFO].Rows[0][COLUMN_INDEX_REPORT_NAME].ToString();

			bool usingTemplate;
			XLWorkbook book;

			if (!Convert.IsDBNull(dsConfig.Tables[TABLE_INDEX_REPORT_INFO].Rows[0][COLUMN_INDEX_TEMPLATE_NAME]) &&
					File.Exists(excelTemplateName))
			{
				usingTemplate = true;

				book = new XLWorkbook(excelTemplateName);
			}
			else
			{
				usingTemplate = false;
				var sheetCount = dsConfig.Tables[TABLE_INDEX_SHEET_NAMES].Rows.Count;
				var sheetNames = new string[sheetCount];
				for (var i = 0; i < sheetCount; i++)
				{
					sheetNames[i] = dsConfig.Tables[TABLE_INDEX_SHEET_NAMES].Rows[i][0].ToString();
				}
				book = new XLWorkbook();
				sheetNames.ForEach(name => book.AddWorksheet(name));
			}

			var dataSet = ATMDB.GetDataSet(storedProcedureName, parms);

#if DEBUG
			try
			{
				dataSet.WriteXml(@"d:\debugtemp\temp.xml");
			}
			catch { }
#endif


			var tableCount = dataSet.Tables.Count;
			var reportHasRecords = false;
			for (var i = 0; i < tableCount; i++)
			{
				if (dataSet.Tables[i].Rows.Count > 0)
				{
					reportHasRecords = true;
				}
			}

			var textColumnNumbers = new List<int>();

			for (var i = 1; i <= tableCount; i++)
			{
				var sheet = book.Worksheet(i);

				var table = dataSet.Tables[i - 1];
				if (reportId == "3") table = RemoveUnusedMeasurements(table); // "3" is the reportid for the director's report

				var columnCount = table.Columns.Count;
				var rowCount = table.Rows.Count;

				if (!usingTemplate)
				{
					//write header
					var header = sheet.Range(1, 1, 1, columnCount);
					for (var c = 1; c <= columnCount; c++)
					{
						header.Cell(1, c).Value = table.Columns[c - 1].ColumnName;
					}
					header.Style.Font.Bold = true;
				}

				var rowNumber = 2;
				var columnNumber = 1;
				var dateTimeCols = getDateTimeColumns(table);

				for (var r = 0; r < rowCount; r++)
				{
					for (var j = 0; j < columnCount; j++)
					{
						val = table.Rows[r][j];
						if (val != null)
						{
							var cell = sheet.Cell(rowNumber, columnNumber);
							if (val is DateTime)
							{
								cell.DataType = XLCellValues.DateTime;

								var dateFormat = cell.Style.NumberFormat.Format;

								cell.Value = (DateTime)val;
								if (string.IsNullOrEmpty(dateFormat.Replace("General", string.Empty)))
									cell.Style.NumberFormat.Format = (dateTimeCols.Contains(j))
											? "M/d/yyyy HH:mm"
											: "M/d/yyyy";
								else
									cell.Style.NumberFormat.Format = dateFormat;
							}
							else if (val is short || val is int || val is double || val is decimal || val is byte)
							{
								cell.Value = Convert.ToDouble(val);
							}
							else
							{
								var textVal = val.ToString().Trim();
								if (textVal != string.Empty)
								{
									cell.SetValue(textVal);
									if (!textColumnNumbers.Contains(columnNumber))
										textColumnNumbers.Add(columnNumber);
								}
							}
						}
						columnNumber++; //incrememnt the excel column
					}
					rowNumber++; //increment the excel row
					columnNumber = 1;
				}
				sheet.Columns().AdjustToContents();

				textColumnNumbers.ForEach(num =>
						sheet.Column(num).AddIgnoredError(false, false, false, false, false, false, true, false, false));
			}

			if (!reportHasRecords) return false;

			const char PAD_CHAR = '0';

			var fileName = string.Concat(reportName + "_", DateTime.Now.Year,
					DateTime.Now.Month.ToString().PadLeft(2, PAD_CHAR),
					DateTime.Now.Day.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Hour.ToString().PadLeft(2, PAD_CHAR),
					DateTime.Now.Minute.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Second.ToString().PadLeft(2, PAD_CHAR),
					FileExtensions.EXCEL_2007);

			Web.StreamExcelDocument(book, fileName, Response);
		}
		catch (ThreadAbortException) { }
		catch (Exception exp)
		{
			var msg = new StringBuilder("Writing data to Excel file.");
			if (val != null)
				msg.AppendFormat("{0} val: {1} valtype: {2}", msg, val, val.GetType());
			throw new Exception(msg.ToString(), exp);
		}

		return true;
	}

	private DataTable RemoveUnusedMeasurements(DataTable table)
	{
		string[] excludedHeaders = { "" };

		var columnHeaders = table.Columns
															.Cast<DataColumn>()
															.Select(x => x.ColumnName)
															.Where(x => !excludedHeaders.Contains(x))
															.ToArray();

		foreach (var columnName in columnHeaders)
		{
			if (!table.Columns[columnName].IsNumeric()) continue;

			try
			{
				// 1) remove get all distinct, non-null column values
				var values = table.AsEnumerable().Select(r => r.Field<object>(columnName)).Distinct().ToList();
				values.RemoveAll(n => n == null);

				// 2) if there are more than one value, then do not remove the column
				if (values.Count != 1) continue;

				// 3) if the only value is 0, remove the column
				if (!double.TryParse(values[0].ToString(), out var sum)) continue;
				if (sum != 0.0) continue;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				continue;
			}

			table.Columns.Remove(columnName);
		}

		return table;
	}

	private static List<int> getDateTimeColumns(DataTable dt)
	{
		var list = new List<int>(dt.Columns.Count);

		list.AddRange(from DataColumn col in dt.Columns where isColumnDateTime(col) select col.Ordinal);

		list.TrimExcess();
		return list;
	}

	private static bool isColumnDateTime(DataColumn col)
	{
		return col.ColumnName.EndsWith("DateTime");
	}
}
