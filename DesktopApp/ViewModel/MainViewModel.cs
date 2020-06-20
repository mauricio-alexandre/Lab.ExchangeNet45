using GalaSoft.MvvmLight;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Lab.ExchangeNet45.DesktopApp";
        }

        public string Title { get; }
    }
}