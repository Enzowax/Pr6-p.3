using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Views; // Укажите ваш namespace проекта
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class RegTests
    {
        [TestMethod]
        public void RegTestSuccess()
        {
            var page = new RegisterPage();
            string uniqueLogin = "newuser_" + System.Guid.NewGuid().ToString().Substring(0, 5);
            bool result = page.Registration("Иванов Иван", uniqueLogin, "Qwerty12345");
            Assert.IsTrue(result);

            string longName = new string('А', 100);
            string login = "long_name_" + System.Guid.NewGuid().ToString().Substring(0, 5);
            bool result2 = page.Registration(longName, login, "Pass12345");
            Assert.IsTrue(result2, "Система должна поддерживать длинные имена");
        }

        [TestMethod]
        public void RegTestFail()
        {
            var page = new RegisterPage();

            bool duplicateResult = page.Registration("Тест Тестович", "Enzo", "anyPassword");
            Assert.IsFalse(duplicateResult);

            Assert.IsFalse(page.Registration("Имя", "", "пароль"));

            Assert.IsFalse(page.Registration("Имя", "login123", ""));

            Assert.IsFalse(page.Registration(" ", " ", " "));
        }
    }
}