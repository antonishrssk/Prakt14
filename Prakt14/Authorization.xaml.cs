using System.Windows;

namespace Prakt14
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            pbPassword.Focus();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e) // Кнопка "Войти"
        {
            if (pbPassword.Password == "123") Close();
            else
            {
                MessageBox.Show("Пароль неверный", "Не удалось войти", MessageBoxButton.OK, MessageBoxImage.Error);
                pbPassword.Focus();
            }
        }

        private void btnEsc_Click(object sender, RoutedEventArgs e) // Кнопка "Отмена"
        {
            this.Owner.Close();
        }
    }
}
