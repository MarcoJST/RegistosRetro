using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RegistosRetro.Pages
{
    /// <summary>
    /// Interaction logic for StockPage.xaml
    /// </summary>
    public partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("stock_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
    }
}
