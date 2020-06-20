using System;
using OfficeOpenXml;

namespace Lab.ExchangeNet45.WebApi.Utils.ExcelHelper
{
    public static class ExcelWorksheetDataTypeExtensions
    {
        public static void FormatCellsValues(this ExcelWorksheet worksheet)
        {
            ExcelCellAddress start = worksheet.Dimension.Start;
            ExcelCellAddress end = worksheet.Dimension.End;
            
            for (int row = start.Row; row <= end.Row; row++)
            {
                for (int column = start.Column; column <= end.Column; column++)
                {
                    ExcelRange cell = worksheet.Cells[row, column];

                    object cellRawValue = cell.Value;

                    if (cellRawValue is char)
                    {
                        cell.Value = cellRawValue.ToString();
                    } 
                    else if (cellRawValue is DateTime)
                    {
                        cell.Style.Numberformat.Format = "dd/mm/yyyy HH:mm:ss";
                    }
                }
            }
        }
    }
}