using System;
using System.Collections.Generic;

namespace Lab.ExchangeNet45.WebApi.Utils.SwaggerFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerProducesAttribute : Attribute
    {
        public SwaggerProducesAttribute(params string[] contentTypes)
        {
            ContentTypes = contentTypes;
        }

        public IEnumerable<string> ContentTypes { get; }
    }
}