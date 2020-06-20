using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Lab.ExchangeNet45.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var ptBr = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = ptBr;
            CultureInfo.DefaultThreadCurrentUICulture = ptBr;

            FrameworkElement.LanguageProperty.OverrideMetadata
            (
                forType: typeof(FrameworkElement),
                typeMetadata: new FrameworkPropertyMetadata
                (
                    defaultValue: XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)
                )
            );
        }
    }
}
