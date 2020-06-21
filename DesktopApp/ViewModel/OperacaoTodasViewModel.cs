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
    public class OperacaoTodasViewModel : ViewModelBase
    {
        private readonly ExchangeService _exchangeService;

        private bool _isGettingOperacoes;
        private bool _isDownloadingCsv;
        private bool _isDownloadingExcel;
        private ObservableCollection<OperacaoQueryModel> _operacoes;

        public OperacaoTodasViewModel(ExchangeService exchangeService)
        {
            _exchangeService = exchangeService;

            Title = "Todas";

            GetOperacoesCommand = new RelayCommand(ExecuteGetOperacoesCommand, CanExecuteGetOperacoesCommand);
            DownloadOperacoesCsvCommand = new RelayCommand(ExecuteDownloadCsv, CanExecuteDownloadCsv);
            DownloadOperacoesExcelCommand = new RelayCommand(ExecuteDownloadExcel, CanExecuteDownloadExcel);
        }

        public string Title { get; }

        public ICommand GetOperacoesCommand { get; }

        public ICommand DownloadOperacoesCsvCommand { get; }

        public ICommand DownloadOperacoesExcelCommand { get; }


        public ObservableCollection<OperacaoQueryModel> Operacoes
        {
            get => _operacoes;
            set => Set(() => Operacoes, ref _operacoes, value);
        }

        private async void ExecuteGetOperacoesCommand()
        {
            _isGettingOperacoes = true;

            IEnumerable<OperacaoQueryModel> operacoes = await _exchangeService.Operacoes.GetAsync();

            Operacoes = new ObservableCollection<OperacaoQueryModel>(operacoes);

            _isGettingOperacoes = false;

            MessageBox.Show($"{Operacoes.Count} operações encontradas.");
        }

        private bool CanExecuteGetOperacoesCommand() => !_isGettingOperacoes;


        private async void ExecuteDownloadCsv()
        {
            _isDownloadingCsv = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadAsCsvFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "CSV Files (*.csv)|*.csv", DefaultExt = ".csv" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingCsv = false;
        }

        private bool CanExecuteDownloadCsv() => !_isDownloadingCsv;


        private async void ExecuteDownloadExcel()
        {
            _isDownloadingExcel = true;

            byte[] byteArrayContent = await _exchangeService.Operacoes.DownloadAsExcelFileAsync();

            var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "Excel Files (*.xlsx)|*.xlsx", DefaultExt = ".xlsx" };

            if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);

            _isDownloadingExcel = false;
        }

        private bool CanExecuteDownloadExcel() => !_isDownloadingExcel;
    }
}
