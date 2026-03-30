using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CinemaApp.Views;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class AuthPage : Page
    {
        private int _failedAttempts = 0;

        public AuthPage()
        {
            InitializeComponent();
        }

        public static bool Auth(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return false;

            if (login.Length < 3 || password.Length < 3)
                return false;

            using (var db = new CinemaDBEntities1())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user != null)
                {
                    AppState.CurrentUser = user;
                    return true;
                }
                return false;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = PasswordBoxPassword.Password;

            if (Auth(login, password))
            {
                _failedAttempts = 0;
                NavigationService?.Navigate(new MainPage());
            }
            else
            {
                _failedAttempts++;
                ErrorText.Text = "Неверный логин или пароль.";

                if (_failedAttempts >= 3)
                {
                    NavigationService?.Navigate(new CaptchaPage(login, password));
                }
            }
        }

        private void ToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage());
        }
    }
}