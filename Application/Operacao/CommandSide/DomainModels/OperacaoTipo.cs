using System;
using System.Linq;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels
{
    public class OperacaoTipo
    {
        private OperacaoTipo(char value)
        {
            if (!IsValid(value))
                throw new ArgumentNullException(nameof(value), "É necessário informar um tipo de operação válido.");

            Value = value;
        }

        public static readonly OperacaoTipo Compra = new OperacaoTipo('C');

        public static readonly OperacaoTipo Venda = new OperacaoTipo('V');

        public static bool IsValid(char value)
        {
            return !char.IsWhiteSpace(value) && new[] {'C', 'V'}.Contains(value);
        }

        public static OperacaoTipo Parse(char value) => new OperacaoTipo(value);

        public char Value { get; private set; }

        #region Equality Member (Value)

        private OperacaoTipo() { }

        protected bool Equals(OperacaoTipo other) => Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OperacaoTipo)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        #endregion
    }
}
