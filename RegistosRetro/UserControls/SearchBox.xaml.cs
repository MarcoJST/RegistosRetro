using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace RegistosRetro.UserControls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private DispatcherTimer Timer { get; set; }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "BackgroundSearchBox", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "ForegroundSearchBox", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Black));

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
            "SearchText",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TextBox));

        public Brush BackgroundSearchBox
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public SearchBox()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler SearchText
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }

        private void uc_txtBox_Loaded(object sender, RoutedEventArgs e)
        {
            uc_txtBox.CaretBrush = uc_txtBox.Foreground;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            uc_textbox_PostSearch(sender, e as RoutedEventArgs);
        }

        private void uc_txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Timer.Stop();
            Timer.Start();
        }

        private void uc_textbox_PostSearch(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(SearchEvent);
            RaiseEvent(args);
        }
    }
}
