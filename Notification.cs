using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace seltest;

public class Notification
{
    [Theory]
    [InlineData("***", "***")]
    public void Notification_Creation(string mail, string password)
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
            previousUrl=driver.Url; 

            var notificationButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Notification Setup")));
            notificationButton.Click();

            wait.Until(d => d.Url != previousUrl);
            previousUrl=driver.Url;
            
            var addNotification = wait.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Add New Notification")));
            addNotification.Click();

            wait.Until(d => d.Url != previousUrl);
            previousUrl=driver.Url;


            driver.FindElement(By.Id("InternalName")).SendKeys("BeachsideShow Villa, Goa");
            driver.FindElement(By.Id("AirbnbCalendarLink")).SendKeys("https://www.airbnb.com/calendar/ical/1059779286518121339.ics?s=7e44f9044e36fbadca0c9074a2f2ae93");

            var phoneInput = driver.FindElement(By.Name("NotificationNumbers"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input'));", phoneInput, "+919876543210");

            new SelectElement(driver.FindElement(By.Id("TimeZoneId"))).SelectByValue("India Standard Time");
            new SelectElement(driver.FindElement(By.Id("NotificationTime"))).SelectByValue("10:00");

            driver.FindElement(By.Id("saveButton")).Click();

            wait.Until(d => d.Url != previousUrl);
            Assert.DoesNotContain("Create", driver.Url);
        }
        finally
        {
            driver.Quit();
        }
    }
}
