using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Lab.ExchangeNet45.WebApi.Utils.SwaggerFilters
{
    public class SwaggerConsumesFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attribute = apiDescription.GetControllerAndActionAttributes<SwaggerConsumesAttribute>().SingleOrDefault();

            if (attribute == null)
            {
                return;
            }

            operation.consumes.Clear();
            operation.consumes = attribute.ContentTypes.ToList();
        }
    }
}