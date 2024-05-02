using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class NewClientPage : Page
    {
        public NewClientPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame frame = parentWindow.FindName("pageFrame") as Frame;
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            if (Business.TClient.Exists(name))
            {
                MessageBox.Show("Já existe um cliente com o nome de \"" + name + "\"!", "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            if (!string.IsNullOrEmpty(email.Trim()) && Business.TClient.ExistsByEmail(email))
            {
                var emailClient = TClient.GetByEmail(email.Trim());
                MessageBox.Show("Já existe um cliente com o email facultado: " + emailClient.Name, "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrEmpty(phone.Trim()) && Business.TClient.ExistsByPhone(phone))
            {
                var phoneClient = TClient.GetByPhone(phone.Trim());
                MessageBox.Show("Já existe um cliente com o número de telefone facultado: " + phoneClient.Name, "Cliente Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var client = Business.TClient.Add(name, address, phone, email);
            MessageBox.Show("Cliente adicionado com sucesso!", "Cliente Adicionado", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(client.id));
        }

    }
}
