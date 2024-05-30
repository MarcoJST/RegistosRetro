using Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class NewInvoicePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<TService> ServiceList { get; set; }
        private ObservableCollection<Helpers.TInvoiceEntry> _invoiceEntries;
        public ObservableCollection<Helpers.TInvoiceEntry> InvoiceEntries
        {
            get { return _invoiceEntries; }
            set
            {
                if (_invoiceEntries != value)
                {
                    if (_invoiceEntries != null)
                    {
                        _invoiceEntries.CollectionChanged -= InvoiceEntries_CollectionChanged;
                        foreach (var entry in _invoiceEntries)
                        {
                            entry.PropertyChanged -= InvoiceEntry_PropertyChanged;
                        }
                    }

                    _invoiceEntries = value;

                    if (_invoiceEntries != null)
                    {
                        _invoiceEntries.CollectionChanged += InvoiceEntries_CollectionChanged;
                        foreach (var entry in _invoiceEntries)
                        {
                            entry.PropertyChanged += InvoiceEntry_PropertyChanged;
                        }
                    }

                    OnPropertyChanged(nameof(InvoiceEntries));
                    OnPropertyChanged(nameof(TotalAmountSum));
                }
            }
        }


        public decimal TotalAmountSum
        {
            get { return InvoiceEntries.Sum(entry => entry.TotalAmount); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NewInvoicePage()
        {
            ServiceList = new ObservableCollection<TService>(Business.TService.GetAll());
            InitializeComponent();
            InvoiceEntries = new ObservableCollection<Helpers.TInvoiceEntry>();
            dg_invoiceEntries.ItemsSource = InvoiceEntries;
            DataContext = this;
        }

        private void AddNewRow()
        {
            var newEntry = new Helpers.TInvoiceEntry()
            {
                Amount = 0,
                Description = "-",
                Local = "-",
                Quantity = 0,
                ServiceDate = DateTime.Now,
                TotalAmount = 0,
            };

            newEntry.PropertyChanged += InvoiceEntry_PropertyChanged;
            InvoiceEntries.Add(newEntry);
            OnPropertyChanged(nameof(TotalAmountSum));
        }


        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame frame = parentWindow.FindName("pageFrame") as Frame;
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;

            var clients = TClient.GetAll().OrderBy(x=> x.Name).ToList();
            inp_client.ucComboBox.ItemsSource = clients;
            inp_client.ucComboBox.DisplayMemberPath = "Name";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (inp_client.ucComboBox.SelectedItem == null)
            {
                MessageBox.Show("É necessário selecionar o cliente! Preencha o campo \"Cliente\".", "Cliente Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!InvoiceEntries.Any())
            {
                MessageBox.Show("A folha de obra deve ter pelo menos um serviço prestado.", "FO vazia", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var entry in InvoiceEntries)
            {
                if (string.IsNullOrEmpty(entry.ServiceReference) || string.IsNullOrEmpty(entry.ServiceName))
                {
                    MessageBox.Show("Existem entradas na FO com o serviço prestado por preencher.", "Serviço inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (entry.Quantity <= 0)
                {
                    MessageBox.Show("Existem entradas na FO com quantidades invállidas. A quantidade do serviço deve ser maior que 0.", "Quantidade Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (entry.ServiceDate == default(DateTime) || !IsValidDateFormat(entry.ServiceDate))
                {
                    MessageBox.Show("Existem entradas na FO com datas inválidas. A data do serviço deve seguir a regra (dia/mês/ano). Ex: " + DateTime.Now.ToString("dd/MM/yyyy"), "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var selectedClient = inp_client.ucComboBox.SelectedItem as TClient;

            var invoice = Business.TInvoice.Add(selectedClient.id);

            foreach (var entry in InvoiceEntries)
                Business.TInvoiceEntry.Add(invoice.id, TService.GetByReference(entry.ServiceReference).id, entry.Description, entry.Local, entry.Amount, entry.Quantity, entry.ServiceDate);

            MessageBox.Show("Folha de obra adicionada com sucesso!", "Folha de Obra Adicionada", MessageBoxButton.OK, MessageBoxImage.Information);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(invoice.id));
        }

        private bool IsValidDateFormat(DateTime date)
        {
            string[] dateFormats = { "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yyyy" };
            string dateStr = date.ToString("d/M/yyyy"); // Convert to a flexible format for validation
            DateTime tempDate;
            return DateTime.TryParseExact(dateStr, dateFormats, null, System.Globalization.DateTimeStyles.None, out tempDate);
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = FindParent<DataGridRow>(button);

            if (row != null)
            {
                var item = row.Item as Helpers.TInvoiceEntry;
                if (item != null)
                {
                    item.PropertyChanged -= InvoiceEntry_PropertyChanged;
                    InvoiceEntries.Remove(item);
                    OnPropertyChanged(nameof(TotalAmountSum));
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            while (parentObject != null && !(parentObject is T))
            {
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }

            return parentObject as T;
        }

        private void AddNewRow(object sender, RoutedEventArgs e)
        {
            AddNewRow();
        }

        private void dg_invoiceEntries_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = e.EditingElement as TextBox;
            if (textBox != null)
            {
                var column = e.Column as DataGridTextColumn;
                if (column != null && column.Header.ToString() == "Ref.")
                {
                    var item = e.Row.Item as Helpers.TInvoiceEntry;
                    if (item != null)
                    {
                        string enteredReference = textBox.Text.Trim();
                        if (!string.IsNullOrEmpty(enteredReference) && TService.ExistsByReference(enteredReference))
                        {
                            var service = TService.GetByReference(enteredReference);
                            item.Amount = service.Amount;
                            item.ServiceName = service.Service;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(enteredReference))
                                MessageBox.Show("Referência inválida. Por favor insira uma referência válida.", "Referência inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                            item.Amount = 0;
                            item.ServiceName = "";
                            item.ServiceReference = "";
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(TotalAmountSum));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is TService selectedService)
            {
                if (comboBox.DataContext is Helpers.TInvoiceEntry entry)
                {
                    entry.ServiceReference = selectedService.Reference;
                    entry.Amount = selectedService.Amount;
                }
            }
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var datePicker = sender as DatePicker;
                if (datePicker != null)
                {
                    var dataGrid = FindParent<DataGrid>(datePicker);
                    if (dataGrid != null)
                    {
                        dataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        e.Handled = true;
                    }
                }
            }
        }

        private void InvoiceEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Helpers.TInvoiceEntry entry in e.NewItems)
                {
                    entry.PropertyChanged += InvoiceEntry_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Helpers.TInvoiceEntry entry in e.OldItems)
                {
                    entry.PropertyChanged -= InvoiceEntry_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalAmountSum));
        }

        private void InvoiceEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Helpers.TInvoiceEntry.TotalAmount))
            {
                OnPropertyChanged(nameof(TotalAmountSum));
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
