using System.Windows;
using System.Windows.Controls;

namespace RegistosRetro.UserControls
{
    public partial class ComboBoxInput : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", typeof(string), typeof(ComboBoxInput), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty WidthTypeProperty = DependencyProperty.Register(
            "WidthType", typeof(string), typeof(ComboBoxInput), new PropertyMetadata("Medium"));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(ComboBoxInput), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(ComboBoxInput), new PropertyMetadata(null));

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
            "ReadOnly", typeof(bool), typeof(ComboBoxInput), new PropertyMetadata(false));

        public static readonly DependencyProperty MandatoryProperty = DependencyProperty.Register(
            "Mandatory", typeof(bool), typeof(ComboBoxInput), new PropertyMetadata(false));

        public string WidthType
        {
            get { return (string)GetValue(WidthTypeProperty); }
            set { SetValue(WidthTypeProperty, value); }
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public bool Mandatory
        {
            get { return (bool)GetValue(MandatoryProperty); }
            set { SetValue(MandatoryProperty, value); }
        }

        public ComboBoxInput()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
