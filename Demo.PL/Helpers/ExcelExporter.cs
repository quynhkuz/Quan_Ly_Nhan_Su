using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class ExcelExporter
    {
        /// <summary>
        /// Tạo file Excel từ tiêu đề và các dòng dữ liệu, trả về mảng byte.
        /// </summary>
        public static byte[] Export(string sheetName, string title, IList<string> headers, IList<IList<object>> rows)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(sheetName);

            sheet.Cell(1, 1).Value = title;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 14;
            sheet.Range(1, 1, 1, headers.Count).Merge();

            for (var i = 0; i < headers.Count; i++)
            {
                var cell = sheet.Cell(3, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            for (var r = 0; r < rows.Count; r++)
            {
                for (var c = 0; c < rows[r].Count; c++)
                {
                    var cell = sheet.Cell(4 + r, c + 1);
                    cell.Value = ToCellValue(rows[r][c]);
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    if (rows[r][c] is decimal or double or int or long)
                        cell.Style.NumberFormat.Format = "#,##0";
                }
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static XLCellValue ToCellValue(object value) => value switch
        {
            null => string.Empty,
            string s => s,
            decimal d => d,
            double db => db,
            int i => i,
            long l => l,
            bool b => b,
            System.DateTime dt => dt,
            _ => value.ToString()
        };
    }
}
