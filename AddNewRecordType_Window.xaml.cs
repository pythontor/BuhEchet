using System.Windows;

namespace PracticalWork4
{
    /// <summary>
    /// Логика взаимодействия для AddNewRecordType_Window.xaml
    /// </summary>
    public partial class AddNewRecordType_Window : Window
    {
        public AddNewRecordType_Window()
        {
            InitializeComponent();
        }

        private void SaveNewRecordType_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
