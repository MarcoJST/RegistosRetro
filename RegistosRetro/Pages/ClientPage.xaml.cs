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
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public Helpers.TClient Client { get; set; }

        public ClientPage()
        {
            InitializeComponent();
        }

        public ClientPage(int idClient)
        {
            InitializeComponent();
            Client = Helpers.TClient.Initialize(idClient);
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("clients_btn");

            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.ItemsSource = Client.Invoices;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string name = inp_name.ucInput.Text.Trim();
            string address = inp_Address.ucInput.Text.Trim();
            string phone = inp_Phone.ucInput.Text.Trim();
            string email = inp_Email.ucInput.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Nome Obrigatório! Preencha o campo Nome.", "Nome Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Business.TClient.Exists(name, Client.id))
            {
                MessageBox.Show("Já existe um cliente com o nome de \"" + name + "\"!", "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrEmpty(email.Trim()) && Business.TClient.ExistsByEmail(email, Client.id))
            {
                var emailClient = Business.TClient.GetByEmail(email.Trim());
                MessageBox.Show("Já existe um cliente com o email facultado: " + emailClient.Name, "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrEmpty(phone.Trim()) && Business.TClient.ExistsByPhone(phone, Client.id))
            {
                var phoneClient = Business.TClient.GetByPhone(phone.Trim());
                MessageBox.Show("Já existe um cliente com o número de telefone facultado: " + phoneClient.Name, "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var updatedClient = Business.TClient.Update(Client.id, name, address, phone, email);
            MessageBox.Show("Cliente atualizado com sucesso!", "Cliente Atualizado", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(updatedClient.id));
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientsPage());
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar este cliente?",
                                              "Eliminar Cliente",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            Business.TClient.Delete(Client.id);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage());
        }

        private void dg_invoiceLink_Click(object sender, RoutedEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void Run_MouseDown_dg_Invoice(object sender, MouseButtonEventArgs e)
        {
            int idInvoice = Convert.ToInt32((sender as Run).Tag.ToString());
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
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
