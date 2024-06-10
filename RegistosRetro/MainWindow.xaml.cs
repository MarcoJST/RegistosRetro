using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private bool isMaximized = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        private void SideBarButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;

            SelectMenuButton(clickedButton.Name);
        
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

        public void SelectMenuButton(string buttonName)
        {
            foreach (var child in FindVisualChildren<ToggleButton>(this))
            {
                if (child.Name == buttonName)
                    child.IsChecked = true;
                else
                    child.IsChecked = false;
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            clients_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    isMaximized = true;
                }
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
#if DEBUG
            return;
#endif

            Business.RemoteDB.CopyToRemoteDB();
        }

    }
}
