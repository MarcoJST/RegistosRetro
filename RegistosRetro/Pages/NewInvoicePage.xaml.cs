using Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class NewInvoicePage : Page
    {
        public ObservableCollection<TService> ServiceList { get; set; }
        ObservableCollection<TInvoiceEntry> InvoiceEntries = new ObservableCollection<TInvoiceEntry>();

        public NewInvoicePage()
        {
            ServiceList = new ObservableCollection<TService>(Business.TService.GetAll());
            InitializeComponent();
            dg_invoiceEntries.ItemsSource = InvoiceEntries;
            DataContext = this;
        }

        private void AddNewRow()
        {
            InvoiceEntries.Add( new TInvoiceEntry()
                {
                    Amount = 0,
                    Description = "-",
                    Local = "-",
                    Quantity = 0,
                    ServiceDate = DateTime.Now,
                    TotalAmount = 0,
                });
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame frame = parentWindow.FindName("pageFrame") as Frame;
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;

            var clients = TClient.GetAll().OrderBy(x=> x.Name).ToList();
            inp_client.ucComboBox.ItemsSource = clients;
            inp_client.ucComboBox.DisplayMemberPath = "Name";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string client = inp_client.ucComboBox.Text.Trim();

            if (string.IsNullOrEmpty(client))
            {
                MessageBox.Show("Nome Obrigatório! Preencha o campo Nome.", "Nome Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var invoice = Business.TInvoice.Add(1);
            MessageBox.Show("Folha de obra adicionada com sucesso!", "Folha de Obra Adicionada", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(invoice.id));
        }

        private void DeleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            var button = sender as FrameworkElement;

            var row = FindParent<DataGridRow>(button);

            if (row != null)
            {
                var item = row.Item as TInvoiceEntry;
                InvoiceEntries.Remove(item);
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            while (parentObject != null && !(parentObject is T))
            {
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }

            return parentObject as T;
        }

        private void AddNewRow(object sender, RoutedEventArgs e)
        {
            AddNewRow();
        }
    }
}
