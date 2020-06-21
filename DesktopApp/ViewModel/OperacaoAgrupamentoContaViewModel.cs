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
    public class OperacaoAgrupamentoContaViewModel : ViewModelBase
    {
        private readonly ExchangeService _exchangeService;

        private bool _isGettingOperacoes;
        private bool _isDownloadingCsv;
        private bool _isDownloadingExcel;
        private ObservableCollection<OperacaoContaGroupingQueryModel> _operacoesAgrupadas;

        public OperacaoAgrupamentoContaViewModel(ExchangeService exchangeService)
        {
            _exchangeService = exchangeService;

            Title = "Agrupadas por Conta";

            GetOperacoesAgrupadasCommand = new RelayCommand(ExecuteGetOperacoesAgrupadasCommand, CanExecuteGetOperacoesAgrupadasCommand);
            DownloadOperacoesAgrupadasCsvCommand = new RelayCommand(ExecuteDownloadCsv, CanExecuteDownloadCsv);
            DownloadOperacoesAgrupadasExcelCommand = new RelayCommand(ExecuteDownloadExcel, CanExecuteDownloadExcel);
        }

        public string Title { get; }

        public ICommand GetOperacoesAgrupadasCommand { get; }

        public ICommand DownloadOperacoesAgrupadasCsvCommand { get; }

        public ICommand DownloadOperacoesAgrupadasExcelCommand { get; }
        

        public ObservableCollection<OperacaoContaGroupingQueryModel> OperacoesAgrupadas
        {
            get => _operacoesAgrupadas;
            set => Set(() => OperacoesAgrupadas, ref _operacoesAgrupadas, value);
        }

        private async void ExecuteGetOperacoesAgrupadasCommand()
        {
            _isGettingOperacoes = true;

            IEnumerable<OperacaoContaGroupingQueryModel> operacoes = await _exchangeService.Operacoes.GroupByContaAsync();

            OperacoesAgrupadas = new ObservableCollection<OperacaoContaGroupingQueryModel>(operacoes);

            _isGettingOperacoes = false;

            MessageBox.Show($"O agrupamento resultou em {OperacoesAgrupadas.Count} linhas.");
        }

        private bool CanExecuteGetOperacoesAgrupadasCommand() => !_isGettingOperacoes;


        private async void ExecuteDownloadCsv()
        {
            _isDownloadingCsv = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByContaAsCsvFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "CSV Files (*.csv)|*.csv", DefaultExt = ".csv" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingCsv = false;
        }

        private bool CanExecuteDownloadCsv() => !_isDownloadingCsv;


        private async void ExecuteDownloadExcel()
        {
            _isDownloadingExcel = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByContaAsExcelFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "Excel Files (*.xlsx)|*.xlsx", DefaultExt = ".xlsx" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingExcel = false;
        }

        private bool CanExecuteDownloadExcel() => !_isDownloadingExcel;
    }
}
