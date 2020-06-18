using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Lab.ExchangeNet45.Contracts.Operacao.Commands
{
    public class AddOperacaoCommand : IRequest
    {
        public long Id { get; set; }

        public DateTime Data { get; set; }

        public char TipoOperacao { get; set; }

        [Required]
        public string Ativo { get; set; }

        public int Quantidade { get; set; }

        public double Preco { get; set; }

        public int Conta { get; set; }
    }
}
