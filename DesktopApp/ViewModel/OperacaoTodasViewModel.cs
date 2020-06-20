using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class OperacaoTodasViewModel : ViewModelBase
    {
        private bool _isGettingOperacoes;
        private ObservableCollection<OperacaoQueryModel> _operacoes;

        public OperacaoTodasViewModel()
        {
            Title = "Todas";
            GetOperacoesCommand = new RelayCommand(ExecuteGetOperacoesCommand, CanExecuteGetOperacoesCommand);
        }

        public string Title { get; }

        public ICommand GetOperacoesCommand { get; }

        public ObservableCollection<OperacaoQueryModel> Operacoes
        {
            get => _operacoes;
            set => Set(() => Operacoes, ref _operacoes, value);
        }

        private async void ExecuteGetOperacoesCommand()
        {
            _isGettingOperacoes = true;

            //await Task.Delay(TimeSpan.FromSeconds(5));

            //var operacoes = new[]
            //{
            //    new OperacaoQueryModel
            //    {
            //        Id = 1,
            //        Data = new DateTime(2020, 6, 19, 17, 59, 59),
            //        TipoOperacao = 'C',
            //        Ativo = "VIVT4",
            //        Conta = 1111,
            //        Preco = 49.9,
            //        Quantidade = 100
            //    },
            //    new OperacaoQueryModel
            //    {
            //        Id = 2,
            //        Data = new DateTime(2020, 6, 15, 11, 21, 10),
            //        TipoOperacao = 'V',
            //        Ativo = "BBSE3",
            //        Conta = 2222,
            //        Preco = 33.4,
            //        Quantidade = 200
            //    }
            //};

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                string stringContent = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};

                var operacoes = JsonConvert.DeserializeObject<IEnumerable<OperacaoQueryModel>>(stringContent, settings);

                Operacoes = new ObservableCollection<OperacaoQueryModel>(operacoes);
            }

            //Operacoes = new ObservableCollection<OperacaoQueryModel>(operacoes);

            _isGettingOperacoes = false;

            Messenger.Default.Send(new NotificationMessage($"{Operacoes.Count} operações encontradas."));
        }

        private bool CanExecuteGetOperacoesCommand() => !_isGettingOperacoes;
    }
}
