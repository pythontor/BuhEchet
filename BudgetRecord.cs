using System;
using System.Collections.Generic;
using System.Windows;

namespace PracticalWork4
{
    internal class BudgetRecord
    {
        public static List<BudgetRecord> records = Files.Deserialization<BudgetRecord>(MainWindow.RecordsPath);
        public static List<BudgetRecord> types = Files.Deserialization<BudgetRecord>(MainWindow.TypesPath);
        public string Name { get; private set; }
        public string TypeName { get; private set; }
        public int _Money;
        public int Money
        {
            get { return _Money; }
            private set
            {
                if (value < 0)
                {
                    _Money = -1 * value;
                }
                else
                {
                    _Money = value;
                }
            }
        }
        public bool IsIncome { get; private set; }
        public DateTime? Date { get; private set; }

        public BudgetRecord(string Name, string TypeName, int Money, bool IsIncome, DateTime? Date) // для десериализации
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.Money = Money;
            this.IsIncome = IsIncome;
            this.Date = Date;
        }
        public BudgetRecord(string Name, string TypeName, int Money, DateTime? Date) // для создания
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.Money = Money;

            if (Money > 0)
            {
                IsIncome = true;
            }
            else
            {
                IsIncome = false;
            }

            if (Date != null) // офигеть, он жёлтый!
            {
                this.Date = (DateTime)Date;
            }
            else
            {
                this.Date = DateTime.Today;
            }
        }

        public static void ComboBoxUpdate()
        {
            List<string> types = new List<string>();

            foreach (var type in BudgetRecord.types)
            {
                types.Add(type.TypeName);
            }

            if (types.Count > 0)
            {
                ((MainWindow)Application.Current.MainWindow).RecordTypes_ComboBox.ItemsSource = types;
            }
        }

        public static void DataGridUpdate()
        {
            List<BudgetRecord> records = new List<BudgetRecord>();
            int result = 0;

            foreach (var record in BudgetRecord.records)
            {
                if (record.Date == ((MainWindow)Application.Current.MainWindow).DatePicker.SelectedDate)
                {
                    records.Add(record);

                    if (record.IsIncome)
                    {
                        result += record.Money;
                    }
                    else
                    {
                        result -= record.Money;
                    }
                }
            }

            ((MainWindow)Application.Current.MainWindow).DataGrid.ItemsSource = records;
            ((MainWindow)Application.Current.MainWindow).Result_TextBlock.Text = "Итог: " + result;
        }
    }
}
