using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CinemaApp.Views;
using WpfApp11;

namespace CinemaApp.Views
{
    public partial class CaptchaPage : Page
    {
        private readonly string _pendingLogin;
        private readonly string _pendingPassword;
        private string _captchaCode = "";

        private const int CaptchaWidth = 340;
        private const int CaptchaHeight = 70;
        private const int NoisePixelCount = 800;

        public CaptchaPage(string login, string password)
        {
            InitializeComponent();
            _pendingLogin = login;
            _pendingPassword = password;
            GenerateCaptcha();
        }

        public static string GenerateCaptchaCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var rnd = new Random();
            return new string(Enumerable.Range(0, 5)
                .Select(_ => chars[rnd.Next(chars.Length)])
                .ToArray());
        }

        public static bool ValidateCaptcha(string expected, string actual)
        {
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(actual))
                return false;
            return expected.Equals(actual.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static BitmapSource RenderCaptchaImage(string code)
        {
            var dv = new DrawingVisual();
            var rnd = new Random();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(240, 244, 248)),
                                 null, new Rect(0, 0, CaptchaWidth, CaptchaHeight));

                for (int i = 0; i < 6; i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb(
                        (byte)rnd.Next(100, 200),
                        (byte)rnd.Next(100, 200),
                        (byte)rnd.Next(100, 200))), 1.5);
                    dc.DrawLine(pen,
                        new Point(rnd.Next(0, CaptchaWidth), rnd.Next(0, CaptchaHeight)),
                        new Point(rnd.Next(0, CaptchaWidth), rnd.Next(0, CaptchaHeight)));
                }

                double x = 20;
                foreach (char c in code)
                {
                    var color = Color.FromRgb((byte)rnd.Next(0, 80), (byte)rnd.Next(0, 80), (byte)rnd.Next(0, 80));
                    var ft = new FormattedText(
                        c.ToString(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("Arial"), FontStyles.Italic, FontWeights.Bold, FontStretches.Normal),
                        32,
                        new SolidColorBrush(color),
                        VisualTreeHelper.GetDpi(dv).PixelsPerDip);

                    double yOffset = rnd.Next(-6, 6);
                    dc.DrawText(ft, new Point(x, CaptchaHeight / 2d - 18 + yOffset));
                    x += 56 + rnd.Next(-4, 4);
                }
            }

            var rtb = new RenderTargetBitmap(CaptchaWidth, CaptchaHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            var wb = new WriteableBitmap(rtb);
            wb.Lock();
            unsafe
            {
                var rndNoise = new Random();
                for (int i = 0; i < NoisePixelCount; i++)
                {
                    int px = rndNoise.Next(0, CaptchaWidth);
                    int py = rndNoise.Next(0, CaptchaHeight);
                    int* pixel = (int*)wb.BackBuffer + py * (wb.BackBufferStride / 4) + px;
                    *pixel = unchecked((int)0xFF000000)
                           | (rndNoise.Next(0, 256) << 16)
                           | (rndNoise.Next(0, 256) << 8)
                           | rndNoise.Next(0, 256);
                }
                wb.AddDirtyRect(new Int32Rect(0, 0, CaptchaWidth, CaptchaHeight));
            }
            wb.Unlock();
            return wb;
        }

        private void GenerateCaptcha()
        {
            _captchaCode = GenerateCaptchaCode();
            CaptchaImage.Source = RenderCaptchaImage(_captchaCode);
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            TextBoxCaptcha.Clear();
            ErrorText.Text = "";
            GenerateCaptcha();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCaptcha(_captchaCode, TextBoxCaptcha.Text))
            {
                ErrorText.Text = "Неверно!";
                GenerateCaptcha();
                TextBoxCaptcha.Clear();
                return;
            }

            if (AuthPage.Auth(_pendingLogin, _pendingPassword))
            {
                NavigationService?.Navigate(new MainPage());
            }
            else
            {
                ErrorText.Text = "Ошибка входа!";
                GenerateCaptcha();
                TextBoxCaptcha.Clear();
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }
    }
}