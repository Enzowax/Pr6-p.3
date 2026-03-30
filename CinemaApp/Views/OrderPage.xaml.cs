using System;
using System.Collections.Generic;
using System.Linq; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp11; 

namespace CinemaApp.Views 
{
    public partial class OrderPage : Page
    {
        private readonly Sessions _currentSession;
        private readonly List<Seats> _selectedSeats;

        public OrderPage(Sessions session, List<Seats> selectedSeats)
        {
            InitializeComponent(); 

            _currentSession = session;
            _selectedSeats = selectedSeats;

            FillOrderData();
        }

        private void FillOrderData()
        {
            TxtMovieTitle.Text = _currentSession.Movies.Title;
            TxtGenres.Text = _currentSession.Movies.Genres;
            TxtDescription.Text = _currentSession.Movies.Description;

            try
            {
                if (!string.IsNullOrEmpty(_currentSession.Movies.PosterPath))
                {
                    ImgPoster.Source = new BitmapImage(new Uri(_currentSession.Movies.PosterPath, UriKind.RelativeOrAbsolute));
                }
            }
            catch { }

            TxtHall.Text = _currentSession.Halls.Name;
            TxtDateTime.Text = _currentSession.DateTime.ToString("f");

            TxtSeats.Text = string.Join(", ", _selectedSeats.Select(s => s.Number));

            decimal total = _selectedSeats.Count * _currentSession.Price;
            TxtTotal.Text = $"{total} ₽";
        }

        private void BtnFinalConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован!");
                return;
            }

            try
            {
                using (var db = new CinemaDBEntities1())
                {
                    foreach (var seat in _selectedSeats)
                    {
                        var ticket = new Tickets
                        {
                            SessionId = _currentSession.Id,
                            SeatId = seat.Id,
                            UserId = AppState.CurrentUser.Id
                        };
                        db.Tickets.Add(ticket);
                    }
                    db.SaveChanges();
                }

                MessageBox.Show("Покупка прошла успешно!");
                NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}