using System.Windows;
using CinemaApp.Views;

namespace CinemaApp
{
    /// <summary>
    /// Главное окно приложения.
    /// Навигация начинается со страницы авторизации.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Заменено с new MainPage() на new AuthPage()
            // чтобы пользователь сначала авторизовался
            MainFrame.Navigate(new AuthPage());
        }
    }
}
