using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CinemaApp.Views;

namespace CinemaApp_RegTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RegTestSuccess()
        {
            var page = new RegPage();

            bool result = page.Registration("new_student@gmai.com", "Qwerty123!", 1);

            Assert.IsTrue(result);
        }
    }
}
