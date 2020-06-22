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

            GetOperacoesAgrupadasCommand = new RelayCommand(ExecuteGetOperacoesCommand, () => !_isGettingOperacoes);
            DownloadOperacoesAgrupadasCsvCommand = new RelayCommand(ExecuteDownloadCsv, () => !_isDownloadingCsv);
            DownloadOperacoesAgrupadasExcelCommand = new RelayCommand(ExecuteDownloadExcel, () => !_isDownloadingExcel);
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
            try
            {
                _isGettingOperacoes = true;

                IEnumerable<OperacaoAtivoGroupingQueryModel> operacoes = await _exchangeService.Operacoes.GroupByAtivoAsync();

                OperacoesAgrupadas = new ObservableCollection<OperacaoAtivoGroupingQueryModel>(operacoes);

                MessageBox.Show($"O agrupamento resultou em {OperacoesAgrupadas.Count} linhas.");
            }
            finally
            {
                _isGettingOperacoes = false;
            }
        }

        private async void ExecuteDownloadCsv()
        {
            try
            {
                _isDownloadingCsv = true;

                byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByAtivoAsCsvFileAsync();

                var dialog = new SaveFileDialog {Title = "Salvar Operações", Filter = "CSV Files (*.csv)|*.csv", DefaultExt = ".csv"};

                if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);
            }
            finally
            {
                _isDownloadingCsv = false;
            }
        }

        private async void ExecuteDownloadExcel()
        {
            try
            {
                _isDownloadingExcel = true;

                byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadGroupingByAtivoAsExcelFileAsync();

                var dialog = new SaveFileDialog {Title = "Salvar Operações", Filter = "Excel Files (*.xlsx)|*.xlsx", DefaultExt = ".xlsx"};

                if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);
            }
            finally
            {
                _isDownloadingExcel = false;
            }
        }
    }
}
