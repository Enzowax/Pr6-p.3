using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class SessionPage : Page
    {
        private Sessions _currentSession;
        private List<Seats> _selectedSeats = new List<Seats>();

        public SessionPage(int sessionId)
        {
            InitializeComponent();
            LoadData(sessionId);
        }

        private void LoadData(int sessionId)
        {
            using (var db = new CinemaDBEntities1())
            {
                _currentSession = db.Sessions
                    .Include(s => s.Movies)
                    .Include(s => s.Halls)
                    .Include(s => s.Halls.Seats)
                    .Include(s => s.Tickets)
                    .FirstOrDefault(s => s.Id == sessionId);

                if (_currentSession == null) return;
                TxtMovieTitle.Text = _currentSession.Movies.Title;
                TxtSessionInfo.Text = $"{_currentSession.DateTime:dd MMMM, HH:mm} — {_currentSession.Halls.Name} ({_currentSession.Halls.Classification})";

                var takenSeatIds = _currentSession.Tickets.Select(t => t.SeatId).ToList();

                var rowsData = _currentSession.Halls.Seats
                    .GroupBy(s => s.Row)
                    .OrderBy(g => g.Key)
                    .Select(g => new
                    {
                        RowNumber = g.Key,
                        Seats = g.OrderBy(s => s.Number).Select(s => new
                        {
                            SeatObj = s,
                            Number = s.Number,
                            IsAvailable = !takenSeatIds.Contains(s.Id)
                        }).ToList()
                    }).ToList();

                RowsControl.ItemsSource = rowsData;
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton?.DataContext == null) return;

            dynamic dataItem = toggleButton.DataContext;
            Seats seat = dataItem.SeatObj;

            if (toggleButton.IsChecked == true)
                _selectedSeats.Add(seat);
            else
                _selectedSeats.Remove(seat);
        }

        private void BtnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            _selectedSeats.Clear();

            var allButtons = FindVisualChildren<ToggleButton>(RowsControl);
            foreach (var btn in allButtons)
            {
                if (btn.IsChecked == true)
                {
                    btn.IsChecked = false;
                }
            }
        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void BtnReserve_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы одно место.");
                return;
            }

            NavigationService.Navigate(new OrderPage(_currentSession, _selectedSeats));
        }
    }
}