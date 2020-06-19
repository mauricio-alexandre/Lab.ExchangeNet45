using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Lab.ExchangeNet45.WebApi.Utils.CsvHelper
{
    public class CsvWriterFactory
    {
        public CsvWriter Create(TextWriter textWriter, bool hasHeaderRecord = true)
        {
            IWriterConfiguration writerConfiguration = CreateCsvWriterConfiguration();
            writerConfiguration.HasHeaderRecord = hasHeaderRecord;

            return new CsvWriter(textWriter, (CsvConfiguration) writerConfiguration);
        } 

        private static IWriterConfiguration CreateCsvWriterConfiguration()
        {
            IWriterConfiguration writerConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture);

            foreach (Type classMapType in CsvClassMapHelper.GetAllConcreteTypesAssignableFromCsvClassMap())
            {
                writerConfiguration.RegisterClassMap(classMapType);
            }

            writerConfiguration.IncludePrivateMembers = true;
            writerConfiguration.Delimiter = ";";
            writerConfiguration.TrimOptions = TrimOptions.Trim;
            writerConfiguration.SanitizeForInjection = true;
            //writerConfiguration.ShouldQuote = (field, context) => true;

            return writerConfiguration;
        }
    }
}
