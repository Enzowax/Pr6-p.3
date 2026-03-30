using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CinemaDBEntities1())
            {
                var user = db.Users
                    .FirstOrDefault(u =>
                        u.Login == TxtLogin.Text &&
                        u.Password == TxtPass.Password);

                if (user != null)
                {
                    AppState.CurrentUser = user;
                    NavigationService.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
