using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            if (AppState.CurrentUser == null)
            {
                NavigationService.Navigate(new LoginPage());
                return;
            }

            TxtUser.Text = $"Пользователь: {AppState.CurrentUser.FullName}";

            LoadTickets();
        }

        private void LoadTickets()
        {
            using (var db = new CinemaDBEntities1())
            {
                GridTickets.ItemsSource = db.Tickets
                    .Include(t => t.Sessions)
                    .Include(t => t.Sessions.Movies)
                    .Include(t => t.Seats)
                    .Where(t => t.UserId == AppState.CurrentUser.Id)
                    .ToList();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AppState.CurrentUser = null;

            MessageBox.Show("Вы успешно вышли из аккаунта.");

            NavigationService.Navigate(new LoginPage());
        }
    }
}
