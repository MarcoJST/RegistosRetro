using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RegistosRetro.UserControls
{
    public partial class Button : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Button), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
            "Kind", typeof(MahApps.Metro.IconPacks.PackIconMaterialKind), typeof(Button), new PropertyMetadata(default(MahApps.Metro.IconPacks.PackIconMaterialKind)));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Black));

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Button));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public MahApps.Metro.IconPacks.PackIconMaterialKind Kind
        {
            get { return (MahApps.Metro.IconPacks.PackIconMaterialKind)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public Button()
        {
            InitializeComponent();
        }

        private void uc_button_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent);
            RaiseEvent(args);
        }
    }
}
