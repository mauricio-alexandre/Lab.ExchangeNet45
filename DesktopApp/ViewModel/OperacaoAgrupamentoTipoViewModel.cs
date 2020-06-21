using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class OperacaoAgrupamentoTipoViewModel : ViewModelBase
    {
        private bool _isGettingOperacoes;
        private ObservableCollection<OperacaoTipoGroupingQueryModel> _operacoesAgrupadas;

        public OperacaoAgrupamentoTipoViewModel()
        {
            Title = "Agrupadas por Tipo de Operação";
            GetOperacoesAgrupadasCommand = new RelayCommand(ExecuteGetOperacoesAgrupadasCommand, CanExecuteGetOperacoesAgrupadasCommand);
        }

        public string Title { get; }

        public ICommand GetOperacoesAgrupadasCommand { get; }

        public ObservableCollection<OperacaoTipoGroupingQueryModel> OperacoesAgrupadas
        {
            get => _operacoesAgrupadas;
            set => Set(() => OperacoesAgrupadas, ref _operacoesAgrupadas, value);
        }

        private async void ExecuteGetOperacoesAgrupadasCommand()
        {
            _isGettingOperacoes = true;

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes/grouping/tipo");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                string stringContent = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

                var operacoes = JsonConvert.DeserializeObject<IEnumerable<OperacaoTipoGroupingQueryModel>>(stringContent, settings);

                OperacoesAgrupadas = new ObservableCollection<OperacaoTipoGroupingQueryModel>(operacoes);
            }

            _isGettingOperacoes = false;

            Messenger.Default.Send(new NotificationMessage($"O agrupamento resultou em {OperacoesAgrupadas.Count} linhas."));
        }

        private bool CanExecuteGetOperacoesAgrupadasCommand() => !_isGettingOperacoes;
    }
}
