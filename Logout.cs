using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace seltest;

public class Logout
{
    [Theory]
    [InlineData("***", "***")]
    public void Logout_ShouldRedirectToHomePage(string mail, string password)
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Navigate().GoToUrl("https://shinetimer.azurewebsites.net");
            var loginLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Login")));
            loginLink.Click();

            string previousUrl = driver.Url;
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email"))).SendKeys($"{mail}@gmail.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Password"))).SendKeys(password);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login-submit"))).Click();

            wait.Until(d => d.Url != previousUrl);

            var logoutButton = wait.Until(driver =>
                driver.FindElement(By.XPath("/html/body/header/nav/div/div/div/ul/li[2]/ul/li[3]/form/button"))
            );
            logoutButton.Click();

            var loginVisibleAgain = wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Login")));
            Assert.True(loginVisibleAgain.Displayed);
        }
        finally
        {
            driver.Quit();
        }
    }
}
