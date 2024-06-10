﻿using Business;
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
            int idClient = Convert.ToInt32((sender as Run).Tag.ToString(), new CultureInfo("en-GB"));
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
                pageFrame.Navigate(new NewClientPage());
        }

        private void dg_ClientLink_Click(object sender, RoutedEventArgs e)
        {
            int idClient = Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB"));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientPage(idClient));
        }

        private void Client_SearchText(object sender, RoutedEventArgs e)
        {
            var textBox = ((RegistosRetro.UserControls.SearchBox)sender).uc_txtBox.Text;
            dg_clientss.ItemsSource = null;
            dg_clientss.ItemsSource = Business.TClient.DynamicSearch(textBox);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("clients_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void dg_delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar o cliente selecionado?",
                                              "Eliminar Cliente",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            TClient.Delete(Convert.ToInt32((sender as Button).Tag.ToString(), new CultureInfo("en-GB")));
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new ClientsPage());
        }
    }
}
