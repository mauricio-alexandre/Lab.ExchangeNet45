using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Lab.ExchangeNet45.WebApi.Utils.CsvHelper
{
    public class CsvClassMapHelper
    {
        public static IEnumerable<Type> GetAllConcreteTypesAssignableFromCsvClassMap()
        {
            Type openGenericClassMapType = typeof(ClassMap<>);

            return typeof(CsvClassMapHelper)
                .Assembly
                .GetTypes()
                .Where(type => type.BaseType != null)
                .Where(type => type.BaseType.IsGenericType)
                .Where(type => openGenericClassMapType.IsAssignableFrom(type.BaseType.GetGenericTypeDefinition()));
        }
    }
}
