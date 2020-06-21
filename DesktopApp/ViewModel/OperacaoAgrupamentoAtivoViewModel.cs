using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lab.ExchangeNet45.Contracts.HttpClient;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Microsoft.Win32;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class OperacaoAgrupamentoAtivoViewModel : ViewModelBase
    {
        private readonly ExchangeService _exchangeService;

        private bool _isGettingOperacoes;
        private bool _isDownloadingCsv;
        private bool _isDownloadingExcel;
        private ObservableCollection<OperacaoAtivoGroupingQueryModel> _operacoesAgrupadas;

        public OperacaoAgrupamentoAtivoViewModel(ExchangeService exchangeService)
        {
            _exchangeService = exchangeService;

            Title = "Agrupadas por Ativo";

            GetOperacoesAgrupadasCommand = new RelayCommand(ExecuteGetOperacoesCommand, CanExecuteGetOperacoesCommand);
            DownloadOperacoesAgrupadasCsvCommand = new RelayCommand(ExecuteDownloadCsv, CanExecuteDownloadCsv);
            DownloadOperacoesAgrupadasExcelCommand = new RelayCommand(ExecuteDownloadExcel, CanExecuteDownloadExcel);
        }

        public string Title { get; }

        public ICommand GetOperacoesAgrupadasCommand { get; }

        public ICommand DownloadOperacoesAgrupadasCsvCommand { get; }

        public ICommand DownloadOperacoesAgrupadasExcelCommand { get; }


        public ObservableCollection<OperacaoAtivoGroupingQueryModel> OperacoesAgrupadas
        {
            get => _operacoesAgrupadas;
            set => Set(() => OperacoesAgrupadas, ref _operacoesAgrupadas, value);
        }

        private async void ExecuteGetOperacoesCommand()
        {
            _isGettingOperacoes = true;

            //using (var httpClient = new HttpClient())
            //{
            //    var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes/grouping/ativo");

            //    HttpResponseMessage response = await httpClient.SendAsync(request);

            //    string stringContent = await response.Content.ReadAsStringAsync();

            //    var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            //    var operacoes = JsonConvert.DeserializeObject<IEnumerable<OperacaoAtivoGroupingQueryModel>>(stringContent, settings);

            //    OperacoesAgrupadas = new ObservableCollection<OperacaoAtivoGroupingQueryModel>(operacoes);
            //}

            IEnumerable<OperacaoAtivoGroupingQueryModel> operacoes = await _exchangeService.Operacoes.GroupByAtivoAsync();

            OperacoesAgrupadas = new ObservableCollection<OperacaoAtivoGroupingQueryModel>(operacoes);

            _isGettingOperacoes = false;

            MessageBox.Show($"O agrupamento resultou em {OperacoesAgrupadas.Count} linhas.");
        }

        private bool CanExecuteGetOperacoesCommand() => !_isGettingOperacoes;


        private async void ExecuteDownloadCsv()
        {
            _isDownloadingCsv = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByAtivoAsCsvFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "CSV Files (*.csv)|*.csv", DefaultExt = ".csv" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingCsv = false;
        }

        private bool CanExecuteDownloadCsv() => !_isDownloadingCsv;


        private async void ExecuteDownloadExcel()
        {
            _isDownloadingExcel = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByAtivoAsExcelFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "Excel Files (*.xlsx)|*.xlsx", DefaultExt = ".xlsx" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingExcel = true;
        }

        private bool CanExecuteDownloadExcel() => !_isDownloadingExcel;
    }
}
