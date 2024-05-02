using System.Windows;
using System.Windows.Controls;

namespace RegistosRetro.UserControls
{
    public partial class Input : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", typeof(string), typeof(Input), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty WidthTypeProperty = DependencyProperty.Register(
            "WidthType", typeof(string), typeof(Input), new PropertyMetadata("Medium"));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(string), typeof(Input), new PropertyMetadata("Text"));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(string), typeof(Input), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
            "ReadOnly", typeof(bool), typeof(Input), new PropertyMetadata(false));

        public static readonly DependencyProperty MandatoryProperty = DependencyProperty.Register(
            "Mandatory", typeof(bool), typeof(Input), new PropertyMetadata(false));

        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

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

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
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

        public Input()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Type == "Number")
            {
                ucInput.PreviewTextInput += TextBox_PreviewTextInput;
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ","
                || textBox.Text.Contains(",") && e.Text == ",")
            {
                e.Handled = true;
            }
        }

    }
}
