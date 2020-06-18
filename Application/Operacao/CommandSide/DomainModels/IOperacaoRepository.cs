namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels
{
    public interface IOperacaoRepository
    {
        Operacao FindById(long id);
        void Add(Operacao operacao);
    }
}
