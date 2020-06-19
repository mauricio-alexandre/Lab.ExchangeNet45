using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using MediatR;

namespace Lab.ExchangeNet45.WebApi.Controllers
{
    [RoutePrefix("api/operacoes")]
    public class OperacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public OperacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route]
        public IHttpActionResult Get()
        {
            return Ok(new { Test = "It's working" });
        }

        [HttpPost, Route]
        public async Task<IHttpActionResult> Post([FromBody] AddOperacaoCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPost, Route("batch")]
        public async Task<IHttpActionResult> PostBatch([FromBody] AddOperacoesBatchCommand commands, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _mediator.Send(commands, cancellationToken);

            return Ok();
        }
    }
}