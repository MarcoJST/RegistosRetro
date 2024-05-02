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
    /// <summary>
    /// Interaction logic for GasPage.xaml
    /// </summary>
    public partial class GasPage : Page
    {
        public GasPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame frame = parentWindow.FindName("pageFrame") as Frame;
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
    }
}
