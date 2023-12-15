using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Prakt14
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e) // Задать
        {
            int value;

            if (Int32.TryParse(tbRowCount.Text, out value)) Data1.RowCount = value;
            else
            {
                MessageBox.Show("Введите правильное значение - Количество строк", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                tbRowCount.Focus();
                return;
            }

            if (Int32.TryParse(tbColumnCount.Text, out value)) Data1.ColumnCount = value;
            else
            {
                MessageBox.Show("Введите правильное значение - Количество столбцов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                tbColumnCount.Focus();
                return;
            }

            StreamWriter inFile = new StreamWriter("config.ini");
            inFile.WriteLine(Data1.RowCount);
            inFile.WriteLine(Data1.ColumnCount);
            inFile.Close();

            Close();
        }

        private void Window_Activated(object sender, EventArgs e) // Перенос информации с главной формы в форму "Настройки"
        {
            tbRowCount.Focus();
            tbRowCount.Text = Data1.RowCount.ToString();
            tbColumnCount.Text = Data1.ColumnCount.ToString();
        }
    }

    public static class Data1
    {
        public static int RowCount;
        public static int ColumnCount;
    }
}
