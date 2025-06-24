using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace seltest
{
    public class Subscription
    {
        [Theory]
        [InlineData("***", "***")]
        public void Buy_New_Subscription(string mail, string password)
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
                previousUrl = driver.Url;

                var notificationButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Buy Now")));
                notificationButton.Click();

                wait.Until(d => d.Url != previousUrl);
                previousUrl = driver.Url;

                var changePlanButtons = driver.FindElements(By.XPath("/html/body/div[1]/main/section/div/div[2]/div/div/div/form/button"));
                if (changePlanButtons.Count > 0)
                {
                    return;
                }

                //driver.FindElement(By.Name("cardnumber")).SendKeys("4242424242424242");
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.CssSelector("iframe[name^='__privateStripeFrame'][title='Secure card number input frame']")));
                driver.FindElement(By.Name("cardnumber")).SendKeys("4242424242424242");
                driver.SwitchTo().DefaultContent();

                //driver.FindElement(By.Id("card-expiry-element")).SendKeys("0140");
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.CssSelector("iframe[title='Secure expiration date input frame']")));
                driver.FindElement(By.Name("exp-date")).SendKeys("0140");
                driver.SwitchTo().DefaultContent();

                //driver.FindElement(By.Id("cvc")).SendKeys("205");
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.CssSelector("iframe[title='Secure CVC input frame']")));
                driver.FindElement(By.Name("cvc")).SendKeys("205");
                driver.SwitchTo().DefaultContent();

                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("pay-button"))).Click();

                wait.Until(d => d.Url != previousUrl);

                Assert.Contains("MySubscriptions", driver.Url);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
