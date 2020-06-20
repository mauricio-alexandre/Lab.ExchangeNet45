using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using OfficeOpenXml;
// stackoverflow.com/questions/13511690/generic-excel-generator-function-for-epplus/13511691

namespace Lab.ExchangeNet45.WebApi.Utils.ExcelHelper
{
    public class ExcelHttpResponseMessage<T> : HttpResponseMessage where T : class
    {
        private const string MediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelHttpResponseMessage(IEnumerable<T> records, string excelFileName) : base(HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(excelFileName)) throw new ArgumentNullException(nameof(excelFileName));

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                Type type = typeof(T);
                PropertyInfo[] headings = type.GetProperties();

                for (int index = 0; index < headings.Length; index++)
                {
                    worksheet.Cells[1, index + 1].Value = headings[index].Name;
                }
                
                worksheet.Cells["A2"].LoadFromCollection(records);

                worksheet.FormatCellsValues();

                Content = new ByteArrayContent(package.GetAsByteArray());
                Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
                Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = excelFileName };
            }
        }
    }
}