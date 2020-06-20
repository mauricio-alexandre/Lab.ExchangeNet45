using System.Collections.Generic;
using System.ComponentModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Lab.ExchangeNet45.DesktopApp";
            TabItems = new INotifyPropertyChanged[]
            {
                ServiceLocator.Current.GetInstance<OperacaoTodasViewModel>(),
                ServiceLocator.Current.GetInstance<OperacaoAgrupamentoStandardViewModel>()
            };
        }

        public string Title { get; }

        public IEnumerable<INotifyPropertyChanged> TabItems { get; }
    }
}