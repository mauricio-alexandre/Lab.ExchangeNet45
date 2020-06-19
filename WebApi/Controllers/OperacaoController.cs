using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Lab.ExchangeNet45.WebApi.Utils.CsvHelper;
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

        [HttpGet, Route, CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = await _mediator.Send(new GetOperacoesQuery(), cancellationToken);
            
            return Ok(operacoes);
        }

        [HttpGet, Route("csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = await _mediator.Send(new GetOperacoesQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoQueryModel>(operacoes, "operacoes.csv");
        }

        [HttpGet, Route("grouping"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByStandard(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);

            return Ok(grouping);
        }

        [HttpGet, Route("grouping/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByStandardCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoStandardGroupingQueryModel>(grouping, "operacoes-grouping.csv");
        }

        [HttpGet, Route("grouping/ativo"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByAtivo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [HttpGet, Route("grouping/ativo/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByAtivoCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoAtivoGroupingQueryModel>(grouping, "operacoes-grouping-ativo.csv");
        }

        [HttpGet, Route("grouping/tipo"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByTipo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [HttpGet, Route("grouping/tipo/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByTipoCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoTipoGroupingQueryModel>(grouping, "operacoes-grouping-tipo.csv");
        }

        [HttpGet, Route("grouping/conta"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByConta(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return Ok(grouping);
        }

        [HttpGet, Route("grouping/conta/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByContaCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoContaGroupingQueryModel>(grouping, "operacoes-grouping-conta.csv");
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