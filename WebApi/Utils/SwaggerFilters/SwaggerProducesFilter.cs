﻿using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Lab.ExchangeNet45.WebApi.Utils.SwaggerFilters
{
    public class SwaggerProducesFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attribute = apiDescription.GetControllerAndActionAttributes<SwaggerProducesAttribute>().SingleOrDefault();

            if (attribute == null)
            {
                return;
            }

            operation.produces.Clear();
            operation.produces = attribute.ContentTypes.ToList();
        }
    }
}