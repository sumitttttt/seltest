using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace seltest
{
    public class Login
    {
        [Theory]
        [InlineData("***", "***")]
        public void Login_WithValidCredentials_ShouldRedirectToEmailUrl(string mail, string password)
        {
            IWebDriver driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                driver.Navigate().GoToUrl("https://shinetimer.azurewebsites.net");
                var loginLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Login")));
                loginLink.Click();

                string previousUrl = driver.Url;
                var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email")));
                emailInput.SendKeys($"{mail}@gmail.com");

                var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Password")));
                passwordInput.SendKeys(password);

                var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login-submit")));
                loginButton.Click();

                wait.Until(d => d.Url != previousUrl);

                Assert.Contains(mail, driver.Url);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}