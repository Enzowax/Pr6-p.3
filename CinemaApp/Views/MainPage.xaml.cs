using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            UpdateData();
            BtnProfile.Content = AppState.CurrentUser == null ? "Войти" : "Личный кабинет";
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if (ListMovies == null) return;
            UpdateData();
        }

        private void UpdateData()
        {
            using (var db = new CinemaDBEntities1())
            {
                var query = db.Movies.AsQueryable();

                if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
                    query = query.Where(m => m.Title.Contains(TxtSearch.Text));

                if (CmbSort != null)
                {
                    if (CmbSort.SelectedIndex == 0)
                        query = query.OrderBy(m => m.Title);
                    else
                        query = query.OrderByDescending(m => m.Rating);
                }

                ListMovies.ItemsSource = query.ToList();
            }
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentUser == null)
                NavigationService.Navigate(new LoginPage());
            else
                NavigationService.Navigate(new ProfilePage());
        }

        private void ListMovies_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListMovies.SelectedItem is Movies m)
                NavigationService.Navigate(new MoviePage(m));
        }
    }
}
