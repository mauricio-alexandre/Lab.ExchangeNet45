using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Lab.ExchangeNet45.WebApi.Utils.CsvHelper
{
    public class CsvHttpResponseMessage<T> : HttpResponseMessage where T : class
    {
        private const string MediaType = "text/csv";

        public CsvHttpResponseMessage(IEnumerable<T> records, string csvFileName) : base(HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(csvFileName)) throw new ArgumentNullException(nameof(csvFileName));

            var contentBuilder = new StringBuilder();

            using (var dataWriter = new StringWriter(contentBuilder))
            using (var csvWriter = new CsvWriterFactory().Create(dataWriter))
            {
                csvWriter.WriteRecords(records);
            }

            string content = contentBuilder.ToString();
            
            Content = new StringContent(content, Encoding.GetEncoding(1252), MediaType);
            Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
            Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {FileName = csvFileName};
        }
    }
}