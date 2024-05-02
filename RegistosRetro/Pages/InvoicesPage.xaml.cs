using System;
using System.Collections.Generic;
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
    /// Interaction logic for InvoicesPage.xaml
    /// </summary>
    public partial class InvoicesPage : Page
    {
        public InvoicesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame frame = parentWindow.FindName("pageFrame") as Frame;
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Business.TInvoice.GetAll();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Run).Tag.ToString());
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void NewInvoice_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new NewInvoicePage());
        }

        private void Invoice_SearchText(object sender, RoutedEventArgs e)
        {
            var textBox = ((RegistosRetro.UserControls.SearchBox)sender).uc_txtBox.Text;
            dg_invoices.ItemsSource = null;
            dg_invoices.ItemsSource = Business.TInvoice.DynamicSearch(textBox);
        }

        private void Run_MouseDownClient(object sender, MouseButtonEventArgs e)
        {
            int idClient = Convert.ToInt32((sender as Run).Tag.ToString());
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(idClient));
        }
    }
}
