using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Views;
using System;
using System.Windows.Media.Imaging;
using System.Threading;

namespace CinemaApp_AuthTests
{
    [TestClass]
    public class CaptchaTests
    {
        [TestMethod]
        public void CaptchaCode_BasicValidation()
        {
            string code = CaptchaPage.GenerateCaptchaCode();
            Assert.IsNotNull(code);
            Assert.AreEqual(5, code.Length);
        }

        [TestMethod]
        public void ValidateCaptcha_LogicTest()
        {
            Assert.IsTrue(CaptchaPage.ValidateCaptcha("XYZ12", "xyz12"));
            Assert.IsFalse(CaptchaPage.ValidateCaptcha("XYZ12", "ABCDE"));
        }

        [TestMethod]
        public void RenderCaptchaImage_STA_Test()
        {
            BitmapSource bmp = null;
            var thread = new Thread(() =>
            {
                // Если тут падает ошибка, проверь, что RenderCaptchaImage — static метод
                bmp = CaptchaPage.RenderCaptchaImage("AB3C7");
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.IsNotNull(bmp);
        }
    }
}