using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Views;
using System;

namespace CinemaApp_AuthTests
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void AuthTest()
        {
            Assert.IsFalse(AuthPage.Auth("wrong_user", "wrong_pass"));
            Assert.IsFalse(AuthPage.Auth("", "password123"));
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            Assert.IsTrue(AuthPage.Auth("admin", "admin123"));
            Assert.IsTrue(AuthPage.Auth("user1", "pass1234"));
        }

        [TestMethod]
        public void AuthTestFail()
        {
            Assert.IsFalse(AuthPage.Auth("   ", "admin123"));
            Assert.IsFalse(AuthPage.Auth("ab", "admin123"));
            Assert.IsFalse(AuthPage.Auth("' OR '1'='1", ""));
        }
    }
}