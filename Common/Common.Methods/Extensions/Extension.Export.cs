using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Common.Methods.Extensions
{
    public static class ExtensionsExport
    {
        public static void ExportExcel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            var xlApp = new Application();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var workbooks = xlApp.Workbooks;
            var workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            var worksheet = (Worksheet)workbook.Worksheets[1];

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                var range = (Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }
            for (var r = 0; r < dt.Rows.Count; r++)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
            }
            xlApp.Visible = true;
        }

        public static void ExportExcel(ListView listView)
        {
            if (listView == null) return;
            var xlApp = new Application();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var workbooks = xlApp.Workbooks;
            var workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            var worksheet = (Worksheet)workbook.Worksheets[1];
            for (var i = 0; i < listView.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = listView.Columns[i].Text;
                var range = (Range)worksheet.Cells[1, i + 1];
                range.Columns.ColumnWidth = (listView.Columns[i].Width / 10) + 5;
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }

            for (var r = 0; r < listView.Items.Count; r++)
            {
                for (var i = 0; i < listView.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = listView.Items[r].SubItems[i].Text;
                }
            }
            xlApp.Visible = true;
        }

        public static void ExportExcel(string[] columnNames, string[][] values)
        {
            var xlApp = new Application();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var workbooks = xlApp.Workbooks;
            var workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            var worksheet = (Worksheet)workbook.Worksheets[1];
            for (var i = 0; i < columnNames.Length; i++)
            {
                worksheet.Cells[1, i + 1] = columnNames[i];
                var range = (Range)worksheet.Cells[1, i + 1];
                range.Columns.ColumnWidth = 50;
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }

            for (var r = 0; r < values.Length; r++)
            {
                for (var i = 0; i < columnNames.Length; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = values[r][i];
                }
            }
            xlApp.Visible = true;
        }

        public static ListView ExportListView(ListView lvs, DataTable dt)
        {
            if (lvs != null)
            {
                lvs.Items.Clear();
                lvs.Columns.Clear();

                foreach (DataColumn items in dt.Columns)
                    lvs.Columns.Add(items.ColumnName);
                
                for (var r = 0; r < dt.Rows.Count; r++)
                {
                    lvs.Items.Add(dt.Rows[r][0].ToString(), 0);
                    for (var i = 1; i < dt.Columns.Count; i++)
                        lvs.Items[r].SubItems.Add(dt.Rows[r][i].ToString());
                }
            }
            
            return lvs;
        }

        public static void ExportToCSV(DataTable dt, string filename, bool includeHeaders)
        {
            TextWriter writer = new StreamWriter(filename);
            if (includeHeaders)
            {
                var headerValues = new List<string>();
                foreach (DataColumn column in dt.Columns)
                {
                    headerValues.Add(QuoteValue(column.ColumnName));
                }

                writer.WriteLine(String.Join(",", headerValues.ToArray()));
            }

            foreach (DataRow row in dt.Rows)
            {
                var items = row.ItemArray.Select(o => QuoteValue(o.ToString())).ToArray();
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"", value.Replace("\"", "\"\""), "\"");
        }
    }
}
