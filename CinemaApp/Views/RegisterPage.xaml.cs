using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp11; // Проверьте, что этот namespace соответствует вашим моделям БД

namespace CinemaApp.Views
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        public bool Registration(string fullName, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName) || 
                string.IsNullOrWhiteSpace(login) || 
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            using (var db = new CinemaDBEntities1())
            {
                if (db.Users.Any(u => u.Login == login))
                {
                    return false;
                }

                try
                {
                    var user = new Users
                    {
                        FullName = fullName,
                        Login = login,
                        Password = password
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    
                    AppState.CurrentUser = user;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (Registration(TxtFullName.Text, TxtLogin.Text, TxtPassword.Password))
            {
                MessageBox.Show("Регистрация успешна!");
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Проверьте данные или логин уже занят.");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}