using System.Web.Http;

namespace Lab.ExchangeNet45.WebApi.Controllers
{
    [RoutePrefix("api/operacoes")]
    public class OperacaoController : ApiController
    {
        [HttpGet, Route]
        public IHttpActionResult Get()
        {
            return Ok(new { Test = "It's working" });
        }
    }
}