using System.Windows;
using System.Windows.Controls;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class CheckoutPage : Page
    {
        private readonly Sessions _session;
        private readonly Seats _seat;

        public CheckoutPage(Sessions session, Seats seat)
        {
            InitializeComponent();
            _session = session;
            _seat = seat;

            TxtInfo.Text = $"К оплате: {_session.Price}";
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CinemaDBEntities1())
            {
                db.Tickets.Add(new Tickets
                {
                    SessionId = _session.Id,
                    SeatId = _seat.Id,
                    UserId = AppState.CurrentUser.Id
                });

                db.SaveChanges();
            }

            MessageBox.Show("Куплено!");
            NavigationService.Navigate(new MainPage());
        }
    }
}
