using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PracticalWork4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string RecordsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BudgetAccounting\\records.csv";
        public static string TypesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BudgetAccounting\\types.csv";

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(RecordsPath))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BudgetAccounting");
                File.Create(RecordsPath).Close();
            }
            if (!File.Exists(TypesPath))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BudgetAccounting");
                File.Create(TypesPath).Close();
            }

            DatePicker.SelectedDate = DateTime.Today;
            BudgetRecord.ComboBoxUpdate();
            BudgetRecord.DataGridUpdate();
        }

        private void AddNewRecordType_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecordType_Window window = new AddNewRecordType_Window();
            bool? result = window.ShowDialog();

            if (result == true)
            {
                if (string.IsNullOrWhiteSpace(window.NewRecordType_TextBox.Text))
                {
                    Errors_TextBlock.Text = "Некорректное название типа записи";
                }
                else
                {
                    BudgetRecord.types.Add(new BudgetRecord("", window.NewRecordType_TextBox.Text, 0, null));
                    BudgetRecord.ComboBoxUpdate();
                }
            }
        }

        private void AddRecord_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecordMethod(true);
        }

        private void ChangeSelectedRecord_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecordMethod(false);
        }

        private void DeleteRecord_Button_Click(object sender, RoutedEventArgs e)
        {
            BudgetRecord.records.Remove((BudgetRecord)DataGrid.SelectedItem);
            BudgetRecord.DataGridUpdate();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Files.Serialization(BudgetRecord.records, RecordsPath);
            Files.Serialization(BudgetRecord.types, TypesPath);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BudgetRecord.DataGridUpdate();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                RecordName_TextBox.Text = ((BudgetRecord)DataGrid.SelectedItem).Name;
                RecordTypes_ComboBox.Text = ((BudgetRecord)DataGrid.SelectedItem).TypeName;

                if (((BudgetRecord)DataGrid.SelectedItem).IsIncome) MoneyAmount_TextBox.Text = ((BudgetRecord)DataGrid.SelectedItem).Money.ToString();
                else MoneyAmount_TextBox.Text = "-" + ((BudgetRecord)DataGrid.SelectedItem).Money.ToString();
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) // Боже, это гениально. Я просто в ахере https://stackoverflow.com/questions/32206954/c-sharp-wpf-how-to-delete-a-column-in-datagrid
        {
            if (e.PropertyName == "Date")
            {
                e.Column = null;
            }
        }

        void AddNewRecordMethod(bool RecordIsNew)
        {
            if (string.IsNullOrWhiteSpace(RecordName_TextBox.Text))
            {
                Errors_TextBlock.Text = "Некорректное имя записи";
            }
            else
            {
                if (RecordTypes_ComboBox.SelectedIndex == -1)
                {
                    Errors_TextBlock.Text = "Не выбран тип записи";
                }
                else
                {
                    if (!int.TryParse(MoneyAmount_TextBox.Text, out int result))
                    {
                        Errors_TextBlock.Text = "Некорректная сумма денег";
                    }
                    else
                    {
                        int index;
                        if (RecordIsNew)
                        {
                            BudgetRecord.records.Add(new BudgetRecord(RecordName_TextBox.Text, RecordTypes_ComboBox.Text, result, DatePicker.SelectedDate));
                        }
                        else
                        {
                            if (DataGrid.SelectedIndex != -1)
                            {
                                index = BudgetRecord.records.IndexOf((BudgetRecord)DataGrid.SelectedItem);
                                BudgetRecord.records[index] = new BudgetRecord(RecordName_TextBox.Text, RecordTypes_ComboBox.Text, result, DatePicker.SelectedDate);
                            }
                        }

                        BudgetRecord.DataGridUpdate();
                    }
                }
            }
        }

        
    }
}
