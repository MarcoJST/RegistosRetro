using Business;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

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
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("invoices_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Business.TInvoice.GetAll();
        }

        private void dg_foLink_Click(object sender, RoutedEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void Run_MouseDownInvoice(object sender, RoutedEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void dg_delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar a folha de obra selecionada?",
                                              "Eliminar Folha de Obra",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            TInvoice.Delete(Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB")));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicesPage());
        }

        private void NewInvoice_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage());
        }

        private void Invoice_SearchText(object sender, RoutedEventArgs e)
        {
            var textBox = ((RegistosRetro.UserControls.SearchBox)sender).uc_txtBox.Text;
            dg_invoices.ItemsSource = null;
            dg_invoices.ItemsSource = Business.TInvoice.DynamicSearch(textBox);
        }

        private void Run_MouseDownClient(object sender, MouseButtonEventArgs e)
        {
            int idClient = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(idClient));
        }
    }
}
