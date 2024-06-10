using Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistosRetro.Pages
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("services_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Business.TService.GetAll();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int idService = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicePage(idService));
        }

        private void dg_ServiceLink_Click(object sender, RoutedEventArgs e)
        {
            int idService = Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicePage(idService));
        }

        private void NewService_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new NewServicePage());
        }

        private void Service_SearchText(object sender, RoutedEventArgs e)
        {
            var textBox = ((RegistosRetro.UserControls.SearchBox)sender).uc_txtBox.Text;
            dg_services.ItemsSource = null;
            dg_services.ItemsSource = Business.TService.DynamicSearch(textBox);
        }

        private void dg_delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar o serviço selecionado?",
                                              "Eliminar Serviço",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            TService.Delete(Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB")));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicesPage());
        }
    }
}
