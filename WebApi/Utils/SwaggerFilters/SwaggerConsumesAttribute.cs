using System;
using System.Collections.Generic;

namespace Lab.ExchangeNet45.WebApi.Utils.SwaggerFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerConsumesAttribute : Attribute
    {
        public SwaggerConsumesAttribute(params string[] contentTypes)
        {
            ContentTypes = contentTypes;
        }

        public IEnumerable<string> ContentTypes { get; }
    }
}