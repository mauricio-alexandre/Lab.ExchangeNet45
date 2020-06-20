using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Lab.ExchangeNet45.WebApi.Utils.CsvHelper;
using Lab.ExchangeNet45.WebApi.Utils.ExcelHelper;
using Lab.ExchangeNet45.WebApi.Utils.SwaggerFilters;
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

        [SwaggerProduces("text/csv")]
        [HttpGet, Route("csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = await _mediator.Send(new GetOperacoesQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoQueryModel>(operacoes, "operacoes.csv");
        }

        [SwaggerProduces("application/octet-stream")]
        [HttpGet, Route("excel-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetExcelFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = await _mediator.Send(new GetOperacoesQuery(), cancellationToken);

            return new ExcelHttpResponseMessage<OperacaoQueryModel>(operacoes, "operacoes.xlsx");
        }


        [HttpGet, Route("grouping"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByStandard(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);

            return Ok(grouping);
        }

        [SwaggerProduces("text/csv")]
        [HttpGet, Route("grouping/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByStandardCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoStandardGroupingQueryModel>(grouping, "operacoes-grouping.csv");
        }

        [SwaggerProduces("application/octet-stream")]
        [HttpGet, Route("grouping/excel-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByStandardExcelFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByStandardQuery(), cancellationToken);
            
            return new ExcelHttpResponseMessage<OperacaoStandardGroupingQueryModel>(grouping, "operacoes-grouping.xlsx");
        }


        [HttpGet, Route("grouping/ativo"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByAtivo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [SwaggerProduces("text/csv")]
        [HttpGet, Route("grouping/ativo/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByAtivoCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoAtivoGroupingQueryModel>(grouping, "operacoes-grouping-ativo.csv");
        }

        [SwaggerProduces("application/octet-stream")]
        [HttpGet, Route("grouping/ativo/excel-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByAtivoExcelFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoAtivoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByAtivoQuery(), cancellationToken);

            return new ExcelHttpResponseMessage<OperacaoAtivoGroupingQueryModel>(grouping, "operacoes-grouping-ativo.xlsx");
        }


        [HttpGet, Route("grouping/tipo"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByTipo(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return Ok(grouping);
        }

        [SwaggerProduces("text/csv")]
        [HttpGet, Route("grouping/tipo/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByTipoCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoTipoGroupingQueryModel>(grouping, "operacoes-grouping-tipo.csv");
        }

        [SwaggerProduces("application/octet-stream")]
        [HttpGet, Route("grouping/tipo/excel-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByTipoExcelFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByTipoQuery(), cancellationToken);

            return new ExcelHttpResponseMessage<OperacaoTipoGroupingQueryModel>(grouping, "operacoes-grouping-tipo.xlsx");
        }


        [HttpGet, Route("grouping/conta"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GroupByConta(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return Ok(grouping);
        }

        [SwaggerProduces("text/csv")]
        [HttpGet, Route("grouping/conta/csv-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByContaCsvFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return new CsvHttpResponseMessage<OperacaoContaGroupingQueryModel>(grouping, "operacoes-grouping-conta.csv");
        }

        [SwaggerProduces("application/octet-stream")]
        [HttpGet, Route("grouping/conta/excel-file"), CacheOutput(ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetGroupByContaExcelFile(CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> grouping = await _mediator.Send(new GroupOperacoesByContaQuery(), cancellationToken);

            return new ExcelHttpResponseMessage<OperacaoContaGroupingQueryModel>(grouping, "operacoes-grouping-conta.xlsx");
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