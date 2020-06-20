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

using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace Lab.ExchangeNet45.DesktopApp.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<OperacaoTodasViewModel>();

            Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public OperacaoTodasViewModel OperacaoTodas => ServiceLocator.Current.GetInstance<OperacaoTodasViewModel>();

        private static void NotifyUserMethod(NotificationMessage message)
        {
            MessageBox.Show(message.Notification);
        }
    }
}