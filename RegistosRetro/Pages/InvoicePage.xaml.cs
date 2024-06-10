using Business;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace RegistosRetro.Pages
{
    public partial class InvoicePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<TClient> ClientsList { get; set; }
        private TClient _selectedClient;
        public TClient SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    OnPropertyChanged(nameof(SelectedClient));
                }
            }
        }
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
        private ObservableCollection<Helpers.TInvoicePayment> _invoicePayments;
        public ObservableCollection<Helpers.TInvoicePayment> InvoicePayments
        {
            get { return _invoicePayments; }
            set
            {
                if (_invoicePayments != value)
                {
                    if (_invoicePayments != null)
                    {
                        _invoicePayments.CollectionChanged -= InvoiceEntries_CollectionChanged;
                        foreach (var entry in _invoicePayments)
                        {
                            entry.PropertyChanged -= InvoicePayment_PropertyChanged;
                        }
                    }

                    _invoicePayments = value;

                    if (_invoicePayments != null)
                    {
                        _invoicePayments.CollectionChanged += InvoicePayment_CollectionChanged;
                        foreach (var entry in _invoicePayments)
                        {
                            entry.PropertyChanged += InvoicePayment_PropertyChanged;
                        }
                    }

                    OnPropertyChanged(nameof(InvoicePayments));
                    OnPropertyChanged(nameof(TotalAmountSum));
                }
            }
        }
        public int idInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public decimal TotalAmountSum
        {
            get { return Math.Round(InvoiceEntries.Sum(entry => entry.TotalAmount), 2); }
        }
        public decimal TotalPaidSum
        {
            get { return Math.Round(InvoicePayments.Sum(payment => payment.Amount), 2); }
        }
        public decimal TotalNotPaidSum
        {
            get { return TotalAmountSum - TotalPaidSum; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public InvoicePage()
        {
            InitializeComponent();
            idInvoice = 0;
            InvoiceTitle = "Nova Folha de Obra";
            InitializeInvoicePage();
        }

        public InvoicePage(int idInvoice)
        {
            InitializeComponent();
            this.idInvoice = idInvoice;
            InvoiceTitle = "Folha de Obra Nº " + idInvoice;
            InitializeInvoicePage();
        }

        private void InitializeInvoicePage()
        {
            btn_Delete.Visibility = idInvoice > 0 ? Visibility.Visible : Visibility.Collapsed;

            ServiceList = new ObservableCollection<TService>(Business.TService.GetAll());
            ClientsList = new ObservableCollection<TClient>(Business.TClient.GetAll());

            InvoiceEntries = new ObservableCollection<Helpers.TInvoiceEntry>();
            InvoicePayments = new ObservableCollection<Helpers.TInvoicePayment>();
            if (idInvoice > 0)
                FillInvoiceEntriesWithAnExistentInvoice();
            dg_invoiceEntries.ItemsSource = InvoiceEntries;
            dg_invoicePayments.ItemsSource = InvoicePayments;
            DataContext = this;
        }

        private void FillInvoiceEntriesWithAnExistentInvoice()
        {
            var invoice = TInvoice.Get(idInvoice);
            if (!ClientsList.Where(x => x.id == invoice.idClient).Any())
                ClientsList.Add(TClient.Get(invoice.idClient));
            SelectedClient = ClientsList.SingleOrDefault(x => x.id == invoice.idClient);
            var invoiceEntries = Helpers.TInvoiceEntry.InitializeList(TInvoiceEntry.GetAllFromInvoice(idInvoice));
            var invoicePayments = Helpers.TInvoicePayment.InitializeList(TInvoicePayment.GetAllFromInvoice(idInvoice));

            foreach (var item in invoiceEntries)
                AddNewRow(item);

            foreach (var item in invoicePayments)
                AddNewRowPayment(item);
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

            AddNewRow(newEntry);
        }

        private void AddNewRowPayment()
        {
            var newEntry = new Helpers.TInvoicePayment()
            {
                id = 0,
                idInvoice = 0,
                PaymentDate = DateTime.Now,
                Observations = "-",
                Amount = 0.00m,
                CreationDate = DateTime.Now,
            };

            AddNewRowPayment(newEntry);
        }

        private void AddNewRowPayment(Helpers.TInvoicePayment invoicePayment)
        {
            var newPayment = invoicePayment;
            newPayment.PropertyChanged += InvoicePayment_PropertyChanged;
            InvoicePayments.Add(newPayment);
            OnPropertyChanged(nameof(TotalPaidSum));
            OnPropertyChanged(nameof(TotalNotPaidSum));
        }

        private void AddNewRow(Helpers.TInvoiceEntry invoiceEntry)
        {
            var newEntry = invoiceEntry;
            newEntry.PropertyChanged += InvoiceEntry_PropertyChanged;
            InvoiceEntries.Add(newEntry);
            OnPropertyChanged(nameof(TotalAmountSum));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = parentWindow as MainWindow;
            Frame frame = parentWindow.FindName("pageFrame") as Frame;

            if (mainWindow != null)
                mainWindow.SelectMenuButton("invoices_btn");
            if (frame != null)
                frame.NavigationUIVisibility = NavigationUIVisibility.Visible;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateDataOfSubmitedForm())
                return;

            if (idInvoice > 0)
                UpdateInvoice();
            else
                AddNewInvoice();

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicePage(idInvoice));
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicesPage());
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar esta folha de obra?",
                                              "Eliminar Folha de Obra",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            TInvoice.Delete(idInvoice);

            Window parentWindow = Window.GetWindow(this);
            Frame pageFrame = parentWindow.FindName("pageFrame") as Frame;

            if (pageFrame != null)
                pageFrame.Navigate(new InvoicesPage());
        }

        private void UpdateInvoice()
        {
            var selectedClient = inp_client.ucComboBox.SelectedItem as TClient;
            var invoice = TInvoice.Get(idInvoice);
            var idCurrentClient = selectedClient.id;

            if (idCurrentClient != invoice.idClient)
                TInvoice.UpdateClient(idInvoice, idCurrentClient);

            //Add and update entries
            foreach (var entry in InvoiceEntries)
            {
                if (entry.id > 0)
                    Business.TInvoiceEntry.Update(entry.id, TService.GetByReference(entry.ServiceReference).id, entry.Description, entry.Local, entry.Amount, entry.Quantity, entry.ServiceDate);
                else
                {
                    entry.id = Business.TInvoiceEntry.Add(invoice.id, TService.GetByReference(entry.ServiceReference).id, entry.Description, entry.Local, entry.Amount, entry.Quantity, entry.ServiceDate).id;
                }
            }

            //Delete entries
            foreach (var entry in TInvoiceEntry.GetAllFromInvoice(idInvoice))
                if (!InvoiceEntries.Where(x => x.id == entry.id).Any())
                    Business.TInvoiceEntry.Delete(entry.id);

            //Add and update payments
            foreach (var payment in InvoicePayments)
            {
                if (payment.id > 0)
                    Business.TInvoicePayment.Update(payment.id, payment.Amount, payment.Observations, payment.PaymentDate);
                else
                {
                    payment.id = Business.TInvoicePayment.Add(invoice.id, payment.Amount, payment.Observations, payment.PaymentDate).id;
                }
            }

            //Delete payments
            foreach (var payment in TInvoicePayment.GetAllFromInvoice(idInvoice))
                if (!InvoicePayments.Where(x => x.id == payment.id).Any())
                    Business.TInvoicePayment.Delete(payment.id);

            MessageBox.Show("Folha de obra atualizada com sucesso!", "Folha de Obra Atualizada", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddNewInvoice()
        {
            var selectedClient = inp_client.ucComboBox.SelectedItem as TClient;

            var invoice = Business.TInvoice.Add(selectedClient.id);

            idInvoice = invoice.id;

            foreach (var entry in InvoiceEntries)
                Business.TInvoiceEntry.Add(invoice.id, TService.GetByReference(entry.ServiceReference).id, entry.Description, entry.Local, entry.Amount, entry.Quantity, entry.ServiceDate);

            foreach (var payment in InvoicePayments)
                Business.TInvoicePayment.Add(invoice.id, payment.Amount, payment.Observations, payment.PaymentDate);

            MessageBox.Show("Folha de obra adicionada com sucesso!", "Folha de Obra Adicionada", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private bool ValidateDataOfSubmitedForm()
        {
            if (inp_client.ucComboBox.SelectedItem == null)
            {
                MessageBox.Show("É necessário selecionar o cliente! Preencha o campo \"Cliente\".", "Cliente Inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!InvoiceEntries.Any())
            {
                MessageBox.Show("A folha de obra deve ter pelo menos um serviço prestado.", "FO vazia", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (var entry in InvoiceEntries)
            {
                if (string.IsNullOrEmpty(entry.ServiceReference) || string.IsNullOrEmpty(entry.ServiceName))
                {
                    MessageBox.Show("Existem entradas na FO com o serviço prestado por preencher.", "Serviço inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (entry.Quantity <= 0)
                {
                    MessageBox.Show("Existem entradas na FO com quantidades invállidas. A quantidade do serviço deve ser maior que 0.", "Quantidade Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (entry.ServiceDate == default(DateTime) || !IsValidDateFormat(entry.ServiceDate))
                {
                    MessageBox.Show("Existem entradas na FO com datas inválidas. A data do serviço deve seguir a regra (dia/mês/ano). Ex: " + DateTime.Now.ToString("dd/MM/yyyy"), "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            foreach (var payment in InvoicePayments)
            {
                if (payment.Amount <= 0)
                {
                    MessageBox.Show("Existem pagamentos com valores inválidos. O valor do pagamento deve ser maior que 0.", "Pagamento Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (payment.PaymentDate == default(DateTime) || !IsValidDateFormat(payment.PaymentDate))
                {
                    MessageBox.Show("Existem pagamentos com datas inválidas. A data do pagamento deve seguir a regra (dia/mês/ano). Ex: " + DateTime.Now.ToString("dd/MM/yyyy"), "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
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

                    if (idInvoice > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar esta entrada da folha de obra?",
                                                  "Eliminar Entrada Folha de Obra",
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Warning);

                        if (result != MessageBoxResult.Yes)
                            return;
                    }

                    item.PropertyChanged -= InvoiceEntry_PropertyChanged;
                    InvoiceEntries.Remove(item);
                    OnPropertyChanged(nameof(TotalAmountSum));
                }
            }
        }

        private void DeletePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = FindParent<DataGridRow>(button);

            if (row != null)
            {
                var item = row.Item as Helpers.TInvoicePayment;
                if (item != null)
                {

                    if (idInvoice > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Tem a certeza que deseja eliminar este pagamento da folha de obra?",
                                                  "Eliminar Pagamaento Folha de Obra",
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Warning);

                        if (result != MessageBoxResult.Yes)
                            return;
                    }

                    item.PropertyChanged -= InvoicePayment_PropertyChanged;
                    InvoicePayments.Remove(item);
                    OnPropertyChanged(nameof(TotalPaidSum));
                    OnPropertyChanged(nameof(TotalNotPaidSum));
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

        private void btn_addNewRow_click(object sender, RoutedEventArgs e)
        {
            AddNewRow();
        }

        private void btn_addNewRowPayment_click(object sender, RoutedEventArgs e)
        {
            AddNewRowPayment();
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

        private void dg_invoicePayments_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            OnPropertyChanged(nameof(TotalPaidSum));
            OnPropertyChanged(nameof(TotalNotPaidSum));
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

        private void InvoicePayment_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Helpers.TInvoicePayment payment in e.NewItems)
                {
                    payment.PropertyChanged += InvoicePayment_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Helpers.TInvoicePayment payment in e.OldItems)
                {
                    payment.PropertyChanged -= InvoicePayment_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalPaidSum));
            OnPropertyChanged(nameof(TotalNotPaidSum));
        }

        private void InvoiceEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Helpers.TInvoiceEntry.TotalAmount))
            {
                OnPropertyChanged(nameof(TotalAmountSum));
            }
        }

        private void InvoicePayment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Helpers.TInvoicePayment.Amount))
            {
                OnPropertyChanged(nameof(TotalPaidSum));
                OnPropertyChanged(nameof(TotalNotPaidSum));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
