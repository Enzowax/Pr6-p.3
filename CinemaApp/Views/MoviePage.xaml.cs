using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using WpfApp11;


namespace CinemaApp.Views
{
    public partial class MoviePage : Page
    {
        private Movies _movie;
        public MoviePage(Movies movie)
        {
            InitializeComponent();
            _movie = movie;
            TxtRating.Text = $"⭐ {movie.Rating}";
            TxtAgeRating.Text = movie.AgeRating;
            TxtTitle.Text = movie.Title;
            TxtDesc.Text = movie.Description;
            TxtGenres.Text = movie.Genres;

            try
            {
                if (!string.IsNullOrEmpty(movie.PosterPath))
                {
                    var uri = new Uri(movie.PosterPath, UriKind.RelativeOrAbsolute);
                    ImgPoster.Source = new BitmapImage(uri);
                }
            }
            catch { }

            LoadSessions();
        }

        private void LoadSessions()
        {
            using (var db = new CinemaDBEntities1()) 
            {
                var sessions = db.Sessions
                    .Include(s => s.Halls)              
                    .Where(s => s.MovieId == _movie.Id)
                    .OrderBy(s => s.DateTime)
                    .ToList();

                ListSessions.ItemsSource = sessions;

                if (!sessions.Any())
                {
                    MessageBox.Show("Для этого фильма нет сеансов в базе данных.");
                }
            }
        }

        private void BtnSession_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentUser == null)
            {
                MessageBox.Show("Сначала войдите в аккаунт!");
                NavigationService.Navigate(new LoginPage());
                return;
            }

            if (sender is Button btn && btn.DataContext is Sessions session)
            {
                NavigationService.Navigate(new SessionPage(session.Id));
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
