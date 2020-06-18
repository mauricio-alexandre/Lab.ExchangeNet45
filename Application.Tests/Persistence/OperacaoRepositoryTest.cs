using System;
using Application.Tests.Utils;
using Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels;
using Lab.ExchangeNet45.Application.Operacao.CommandSide.Persistence;
using NUnit.Framework;

namespace Application.Tests.Persistence
{
    [TestFixture]
    public class OperacaoRepositoryTest : NHibernateSqLiteTestFixture
    {
        private void Clear()
        {
            ExecuteSessionTransactionScope(session =>
            {
                session.CreateQuery($"delete from {nameof(Operacao)}").ExecuteUpdate();
            });
        }

        [SetUp]
        public void Before() => Clear();

        [TearDown]
        public void After() => Clear();

        [Test]
        public void When_ANewOperacaoIsAdded_Expect_ToBeFoundOnTheDatabase()
        {
            Operacao aNewOperacao = null;

            ExecuteSessionTransactionScope(session =>
            {
                aNewOperacao = new Operacao(1, new DateTime(2020, 6, 17, 10, 33, 45), OperacaoTipo.Compra, "VIVT4", 100, 49.70d, 9999);
                new OperacaoRepository(session).Add(aNewOperacao);
            });

            Operacao foundOnTheDatabase = null;

            ExecuteSessionTransactionScope(session =>
            {
                foundOnTheDatabase = new OperacaoRepository(session).FindById(aNewOperacao.Id);
            });

            Assert.True(aNewOperacao.PublicInstancePropertiesEqual(foundOnTheDatabase));
        }
    }
}
