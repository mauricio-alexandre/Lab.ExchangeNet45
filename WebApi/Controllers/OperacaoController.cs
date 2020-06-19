using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using MediatR;
using WebApi.OutputCache.V2;

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
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = await _mediator.Send(new GetOperacoesQuery(), cancellationToken);
            
            return Ok(operacoes);
        }

        [CacheOutput(ServerTimeSpan = 300)]
        [HttpGet, Route("grouping")]
        public async Task<IHttpActionResult> GroupByStandard(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);

            return Ok(grouping);
        }

        [CacheOutput(ServerTimeSpan = 300)]
        [HttpGet, Route("grouping/ativo")]
        public async Task<IHttpActionResult> GroupByAtivo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [CacheOutput(ServerTimeSpan = 300)]
        [HttpGet, Route("grouping/tipo")]
        public async Task<IHttpActionResult> GroupByTipo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [CacheOutput(ServerTimeSpan = 300)]
        [HttpGet, Route("grouping/conta")]
        public async Task<IHttpActionResult> GroupByConta(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return Ok(grouping);
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