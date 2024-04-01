using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace RegistosRetro.Pages
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>

    public partial class ClientsPage : Page
    {

        public ClientsPage()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Business.TClient.GetAll();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int idClient = Convert.ToInt32((sender as Run).Tag.ToString());
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(idClient));
        }

        private void NewClient_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage());
        }

        private void NewClient_SearchText(object sender, RoutedEventArgs e)
        {
            var textBox = ((RegistosRetro.UserControls.SearchBox)sender).uc_txtBox.Text;
            dg_clientss.ItemsSource = null;
            dg_clientss.ItemsSource = Business.TClient.DynamicSearch(textBox);
        }

    }
}
