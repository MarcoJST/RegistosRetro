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
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public Helpers.TService Service { get; set; }

        public ServicePage(int idService)
        {
            InitializeComponent();
            Service = Helpers.TService.Initialize(idService);
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("services_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Service.InvoiceEntries;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string reference = inp_reference.ucInput.Text.Trim();
            string service = inp_service.ucInput.Text.Trim();
            string amount = inp_amount.ucInput.Text.Trim();

            if (string.IsNullOrEmpty(service))
            {
                MessageBox.Show("O nome do serviço é obrigatório! Preencha o campo Serviço.", "Serviço Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Business.TService.Exists(service, Service.id))
            {
                MessageBox.Show("Já existe um serviço com o nome de \"" + service + "\"!", "Serviço Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Business.TService.ExistsByReference(reference, Service.id))
            {
                MessageBox.Show("Já existe um serviço com a referência \"" + reference + "\"!", "Serviço Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var updatedService = Business.TService.Update(Service.id, reference, service, Convert.ToDecimal(amount, new CultureInfo("en-GB")));
            MessageBox.Show("Serviço atualizado com sucesso!", "Serviço Atualizado", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicePage(updatedService.id));
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicesPage());
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar este serviço?",
                                              "Eliminar Serviço",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            TService.Delete(Service.id);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicesPage());
        }

        private void Run_MouseDown_dg_FO(object sender, MouseButtonEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void Run_MouseDown_dg_Client(object sender, MouseButtonEventArgs e)
        {
            int idClient = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(idClient));
        }

        private void dg_foLink_Click(object sender, RoutedEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }
    }
}
