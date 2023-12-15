using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Lib_4;
using LibMas;

namespace Prakt14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[,] matrix;

        private void dataGridMatrix_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) // Редактирование ячеек таблицы
        {
            int columnIndex = e.Column.DisplayIndex;
            int rowIndex = e.Row.GetIndex();
            if (Int32.TryParse(((TextBox)e.EditingElement).Text, out matrix[columnIndex, rowIndex]))
                tbResult.Clear();
            else MessageBox.Show("Введите правильное значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCreateTable_Click(object sender, RoutedEventArgs e) // Создать таблицу
        {
            if (Int32.TryParse(tbRowCount.Text, out int rowCount) &&
                Int32.TryParse(tbColumnCount.Text, out int columnCount) &&
                rowCount >= 0 && columnCount >= 0)
            {
                matrix = new int[rowCount, columnCount];
                dataGridMatrix.ItemsSource = VisualArray.ToDataTable(matrix).DefaultView;

                textBlockTableSize.Text = $"Размер таблицы: {rowCount}x{columnCount}";
                tbResult.Clear();
            }
            else MessageBox.Show("Введите правильные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnFillTable_Click(object sender, RoutedEventArgs e) // Заполнить таблицу
        {
            if (Int32.TryParse(tbRowCount.Text, out int rowCount) &&
                Int32.TryParse(tbColumnCount.Text, out int columnCount) &&
                rowCount >= 0 && columnCount >= 0)
            {
                matrix = Arrays.Fill(rowCount, columnCount);
                dataGridMatrix.ItemsSource = VisualArray.ToDataTable(matrix).DefaultView;

                textBlockTableSize.Text = $"Размер таблицы: {rowCount}x{columnCount}";
                tbResult.Clear();
            }
            else MessageBox.Show("Введите правильные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e) // Рассчитать
        {
            if (matrix != null)
            {
                int count = Calculation.ColumnsUniqueElements(matrix);
                tbResult.Text = count.ToString();
            }
            else MessageBox.Show("Создайте таблицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void miInfo_Click(object sender, RoutedEventArgs e) // О программе
        {
            MessageBox.Show("Практическая работа №13\n" +
                "Разработчик: Антонишин Кирилл Сергеевич\n" +
                "Дана целочисленная матрица размера M * N. Найти количество ее столбцов, все элементы которых различны.",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void miExit_Click(object sender, RoutedEventArgs e) // Выход
        {
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e) // Подтверждение закрытия окна
        {
            MessageBoxResult confirm = MessageBox.Show("Вы уверены, что хотите завершить работу с программой?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e) // Открыть
        {
            Arrays.Load(out matrix, out bool isLoaded);
            if (isLoaded) dataGridMatrix.ItemsSource = VisualArray.ToDataTable(matrix).DefaultView;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) // Сохранить
        {
            if (matrix != null) Arrays.Save(matrix);
            else MessageBox.Show("Создайте таблицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void dataGridMatrix_SelectionChanged(object sender, SelectionChangedEventArgs e) // Изменение StatusBar'а при выделении ячейки
        {
            int rowIndex = dataGridMatrix.SelectedIndex;
            int columnIndex = dataGridMatrix.CurrentCell.Column.DisplayIndex;
            if (rowIndex == -1) textBlockSelectedCell.Text = "";
            else textBlockSelectedCell.Text = $"Выделенная ячейка: [{rowIndex + 1};{columnIndex + 1}]";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Открытие окна "Авторизация"
            Authorization auth = new Authorization();
            auth.Owner = this;
            auth.ShowDialog();

            try
            {
                // Установка размера таблицы из файла конфигурации
                StreamReader outFile = new StreamReader("config.ini");
                tbRowCount.Text = outFile.ReadLine();
                tbColumnCount.Text = outFile.ReadLine();
                outFile.Close();

                // Обновление StatusBar'а
                textBlockTableSize.Text = $"Размер таблицы: {tbRowCount.Text}x{tbColumnCount.Text}";
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить размер таблицы из файла конфигурации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miSettings_Click(object sender, RoutedEventArgs e) // Настройки
        {
            // Заносим текущие значения в класс для отображения в окне "Настройки"
            if (Int32.TryParse(tbRowCount.Text, out int rowCount)) Data1.RowCount = rowCount;
            else Data1.RowCount = 0;
            if (Int32.TryParse(tbColumnCount.Text, out int columnCount)) Data1.ColumnCount = columnCount;
            else Data1.ColumnCount = 0;

            // Открываем окно "Настройки"
            Settings sett = new Settings();
            sett.Owner = this;
            sett.ShowDialog();

            // Берем полученные значения и заносим их в соответствующие элементы
            tbRowCount.Text = Data1.RowCount.ToString();
            tbColumnCount.Text = Data1.ColumnCount.ToString();

            // Обновляем StatusBar
            textBlockTableSize.Text = $"Размер таблицы: {Data1.RowCount}x{Data1.ColumnCount}";
        }
    }
}