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
using MahApps.Metro.Controls;
using RegistosRetro.Pages;

namespace RegistosRetro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Button lastSelectedButton;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        private void SideBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset properties for the previously selected button
            if (lastSelectedButton != null)
            {
                lastSelectedButton.Background = Brushes.Transparent;
                lastSelectedButton.Foreground = Brushes.White;
                lastSelectedButton.FontWeight = FontWeights.Regular;
            }

            // Set properties for the currently selected button
            Button clickedButton = sender as Button;
            // Define colors using Color structure
            Color backgroundColor = (Color)ColorConverter.ConvertFromString("#F4CA4A");
            Color foregroundColor = (Color)ColorConverter.ConvertFromString("#2F2F2F");

            // Create SolidColorBrushes using defined colors
            SolidColorBrush backgroundBrush = new SolidColorBrush(backgroundColor);
            SolidColorBrush foregroundBrush = new SolidColorBrush(foregroundColor);

            // Set properties of the clicked button
            clickedButton.Background = backgroundBrush;
            clickedButton.Foreground = foregroundBrush;
            clickedButton.FontWeight = FontWeights.DemiBold;

            lastSelectedButton = clickedButton;

            if (clickedButton.Name == "home_btn")
                pageFrame.NavigationService.Navigate(new HomePage());
            else if (clickedButton.Name == "clients_btn")
                pageFrame.NavigationService.Navigate(new ClientsPage());
            else if (clickedButton.Name == "services_btn")
                pageFrame.NavigationService.Navigate(new ServicesPage());
            else if (clickedButton.Name == "invoices_btn")
                pageFrame.NavigationService.Navigate(new InvoicesPage());
            else if (clickedButton.Name == "debts_btn")
                pageFrame.NavigationService.Navigate(new DebtsPage());
            else if (clickedButton.Name == "stock_btn")
                pageFrame.NavigationService.Navigate(new StockPage());
            else if (clickedButton.Name == "gas_btn")
                pageFrame.NavigationService.Navigate(new GasPage());
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            home_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            services_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            home_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
