/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Lab.ExchangeNet45.DesktopApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Lab.ExchangeNet45.Contracts.HttpClient;
using Lab.ExchangeNet45.Contracts.HttpClient.Resources;
using Lab.ExchangeNet45.DesktopApp.Integration;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new ExchangeHttpClientFactory().CreateFromAppSettings());
            SimpleIoc.Default.Register<OperacaoResource>();
            SimpleIoc.Default.Register<ExchangeService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<OperacaoTodasViewModel>();
            SimpleIoc.Default.Register<OperacaoAgrupamentoStandardViewModel>();
            SimpleIoc.Default.Register<OperacaoAgrupamentoAtivoViewModel>();
            SimpleIoc.Default.Register<OperacaoAgrupamentoTipoViewModel>();
            SimpleIoc.Default.Register<OperacaoAgrupamentoContaViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public OperacaoTodasViewModel OperacaoTodas => ServiceLocator.Current.GetInstance<OperacaoTodasViewModel>();

        public OperacaoAgrupamentoStandardViewModel OperacaoAgrupamentoStandard => ServiceLocator.Current.GetInstance<OperacaoAgrupamentoStandardViewModel>();

        public OperacaoAgrupamentoAtivoViewModel OperacaoAgrupamentoAtivo => ServiceLocator.Current.GetInstance<OperacaoAgrupamentoAtivoViewModel>();

        public OperacaoAgrupamentoTipoViewModel OperacaoAgrupamentoTipo => ServiceLocator.Current.GetInstance<OperacaoAgrupamentoTipoViewModel>();

        public OperacaoAgrupamentoContaViewModel OperacaoAgrupamentoConta => ServiceLocator.Current.GetInstance<OperacaoAgrupamentoContaViewModel>();

    }
}