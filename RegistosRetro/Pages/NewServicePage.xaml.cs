using Business;
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
    public partial class NewServicePage : Page
    {
        public NewServicePage()
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
            string reference = inp_reference.ucInput.Text.Trim();
            string service = inp_service.ucInput.Text.Trim();
            string amount = inp_amount.ucInput.Text.Trim();

            if (string.IsNullOrEmpty(service))
            {
                MessageBox.Show("O nome do serviço é obrigatório! Preencha o campo Serviço.", "Serviço Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Business.TService.Exists(service))
            {
                MessageBox.Show("Já existe um serviço com o nome de \"" + service + "\"!", "Serviço Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newService = Business.TService.Add(reference, service, Convert.ToDecimal(amount));
            MessageBox.Show("Serviço adicionado com sucesso!", "Serviço Adicionado", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicePage(newService.id));
        }
    }
}
