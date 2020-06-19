using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Lab.ExchangeNet45.WebApi.Utils.CsvHelper
{
    public class CsvReaderFactory
    {
        public CsvReader Create(TextReader textReader, bool hasHeaderRecord = true, string delimiter = ";")
        {
            IReaderConfiguration readerConfiguration = CreateCsvReaderConfiguration();
            readerConfiguration.HasHeaderRecord = hasHeaderRecord;
            readerConfiguration.Delimiter = delimiter;

            return new CsvReader(textReader, (CsvConfiguration) readerConfiguration);
        }

        private static IReaderConfiguration CreateCsvReaderConfiguration()
        {
            IReaderConfiguration readerConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture);

            foreach (Type classMapType in CsvClassMapHelper.GetAllConcreteTypesAssignableFromCsvClassMap())
            {
                readerConfiguration.RegisterClassMap(classMapType);
            }

            readerConfiguration.IncludePrivateMembers = true;
            readerConfiguration.TrimOptions = TrimOptions.Trim;

            // Ignore Missing Field
            readerConfiguration.MissingFieldFound = (headerNames, index, context) => { };

            return readerConfiguration;
        }
    }
}
