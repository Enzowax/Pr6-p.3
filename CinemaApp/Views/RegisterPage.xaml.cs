using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CinemaDBEntities1())
            {
                if (db.Users.Any(u => u.Login == TxtLogin.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                var user = new Users
                {
                    FullName = TxtFullName.Text,
                    Login = TxtLogin.Text,
                    Password = TxtPassword.Password
                };

                db.Users.Add(user);
                db.SaveChanges();

                MessageBox.Show("Регистрация успешна!");

                AppState.CurrentUser = user;
                NavigationService.Navigate(new MainPage());
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
