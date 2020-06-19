using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab.ExchangeNet45.OperacaoLoader
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "Exchange Operação Loader - Carregador de Massa de Dados";

            Action<ExchangeServiceConfiguration, string> carregar = ReadValidOpcaoDeCarga();

            ServicePointManager.DefaultConnectionLimit = 210;

            Console.WriteLine("Carregando...");

            try
            {
                var exchangeServiceConfiguration = ExchangeServiceConfiguration.FromAppSettings();

                string commandsJson = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "operacoes.json"));

                var stopwatch = new Stopwatch();

                stopwatch.Start();

                carregar.Invoke(exchangeServiceConfiguration, commandsJson);

                stopwatch.Stop();

                TimeSpan tempo = stopwatch.Elapsed;

                Console.WriteLine($"Tempo gasto: {(int)tempo.TotalHours:00}:{tempo.Minutes:00}:{tempo.Seconds:00}.{tempo.Milliseconds}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine();
            Console.WriteLine("Carga finalizada. Pressione qualquer tecla para sair...");

            Console.ReadKey();
        }

        private static void SendOneRequestPerCommand(ExchangeServiceConfiguration configuration, string commandsJson)
        {
            IEnumerable<string> bodies = JArray.Parse(commandsJson).Select(jToken => jToken.ToString(Formatting.None)).ToArray();

            var httpClient = new HttpClient();

            Parallel.ForEach(bodies, new ParallelOptions { MaxDegreeOfParallelism = 200 }, body =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, configuration.OperacoesUri)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                httpClient.SendAsync(request).GetAwaiter().GetResult();
            });
        }

        private static void SendOneRequestForAllCommands(ExchangeServiceConfiguration configuration, string commandsJson)
        {
            string body = JArray.Parse(commandsJson).ToString(Formatting.None);

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, configuration.OperacoesBatchUri)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = httpClient.SendAsync(request).GetAwaiter().GetResult();

                Console.WriteLine($"Batch, Status: {response.StatusCode}");
            }
        }

        private static Action<ExchangeServiceConfiguration, string> ReadValidOpcaoDeCarga()
        {
            while (true)
            {
                Console.WriteLine(@"Informe uma das opções de carga de operações (pressione apenas Enter para opção padrão: ""2""):");
                Console.WriteLine("1 - Um request por registro (vários requests)");
                Console.WriteLine("2 - Um request para todos os registros (batch)");
                string optionString = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(optionString))
                {
                    optionString = "2";
                    Console.WriteLine(optionString);
                }

                if (OpcaoCargaDictionary.ContainsKey(optionString)) return OpcaoCargaDictionary[optionString];

                Console.WriteLine("Opção inválida!");
            }
        }

        private static readonly IDictionary<string, Action<ExchangeServiceConfiguration, string>> OpcaoCargaDictionary = new Dictionary<string, Action<ExchangeServiceConfiguration, string>>
        {
            { "1", SendOneRequestPerCommand }, { "2", SendOneRequestForAllCommands }
        };
    }
}
