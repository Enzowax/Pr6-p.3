using System.Windows;
using CinemaApp.Views;

namespace CinemaApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
        }
    }
}