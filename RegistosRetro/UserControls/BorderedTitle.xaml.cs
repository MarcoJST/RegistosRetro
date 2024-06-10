using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RegistosRetro.UserControls
{
    /// <summary>
    /// Interaction logic for BorderedTitle.xaml
    /// </summary>
    public partial class BorderedTitle : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(BorderedTitle), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register("TitleFontSize", typeof(double), typeof(BorderedTitle), new PropertyMetadata(22.0));

        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register("TitleFontWeight", typeof(FontWeight), typeof(BorderedTitle), new PropertyMetadata(FontWeights.DemiBold));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(BorderedTitle), new PropertyMetadata(Brushes.Black));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }

        public FontWeight TitleFontWeight
        {
            get { return (FontWeight)GetValue(TitleFontWeightProperty); }
            set { SetValue(TitleFontWeightProperty, value); }
        }

        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }

        public BorderedTitle()
        {
            InitializeComponent();
        }
    }
}
