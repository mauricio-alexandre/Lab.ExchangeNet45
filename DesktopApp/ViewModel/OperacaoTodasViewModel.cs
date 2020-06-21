using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class OperacaoTodasViewModel : ViewModelBase
    {
        private bool _isGettingOperacoes;
        private bool _isDownloadingCsv;
        private bool _isDownloadingExcel;
        private ObservableCollection<OperacaoQueryModel> _operacoes;

        public OperacaoTodasViewModel()
        {
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

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                string stringContent = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};

                var operacoes = JsonConvert.DeserializeObject<IEnumerable<OperacaoQueryModel>>(stringContent, settings);

                Operacoes = new ObservableCollection<OperacaoQueryModel>(operacoes);
            }

            _isGettingOperacoes = false;

            MessageBox.Show($"{Operacoes.Count} operações encontradas.");
        }

        private bool CanExecuteGetOperacoesCommand() => !_isGettingOperacoes;


        private async void ExecuteDownloadCsv()
        {
            _isDownloadingCsv = true;

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes/csv-file");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                byte[] byteArrayContent = await response.Content.ReadAsByteArrayAsync();

                var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "CSV Files (*.csv)|*.csv", DefaultExt = ".csv" };
                
                if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);
            }

            _isDownloadingCsv = false;
        }

        private bool CanExecuteDownloadCsv() => !_isDownloadingCsv;


        private async void ExecuteDownloadExcel()
        {
            _isDownloadingExcel = true;

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44362/api/operacoes/excel-file");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                byte[] byteArrayContent = await response.Content.ReadAsByteArrayAsync();

                var dialog = new SaveFileDialog { Title = "Salvar Operações", Filter = "Excel Files (*.xlsx)|*.xlsx", DefaultExt = ".xlsx" };

                if (dialog.ShowDialog() == true) File.WriteAllBytes(dialog.FileName, byteArrayContent);
            }

            _isDownloadingExcel = true;
        }

        private bool CanExecuteDownloadExcel() => !_isDownloadingExcel;
    }
}
