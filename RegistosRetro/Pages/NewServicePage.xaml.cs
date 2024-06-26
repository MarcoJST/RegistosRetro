﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("services_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;
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

            if (Business.TService.Exists(service))
            {
                MessageBox.Show("Já existe um serviço com o nome de \"" + service + "\"!", "Serviço Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Business.TService.ExistsByReference(reference))
            {
                MessageBox.Show("Já existe um serviço com a referência \"" + reference + "\"!", "Serviço Existente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newService = Business.TService.Add(reference, service, Convert.ToDecimal(amount, new CultureInfo("en-GB")));
            MessageBox.Show("Serviço adicionado com sucesso!", "Serviço Adicionado", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicePage(newService.id));
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ServicesPage());
        }
    }
}
