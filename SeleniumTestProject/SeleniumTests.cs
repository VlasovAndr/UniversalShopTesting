using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace SeleniumTestProject
{
    public class SeleniumTests
    {

        [Theory]
        [InlineData(@"ConfigFiles\ELDORADO.json")]
        [InlineData(@"ConfigFiles\DNS.json")]
        [InlineData(@"ConfigFiles\WILDBERRIES.json")]

        public void UniversalShopTesting(string pathToConfigFile)
        {

            var website = JsonConvert.DeserializeObject<Website>(File.ReadAllText(pathToConfigFile));
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            IWebDriver driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl(website.Url);
            driver.Manage().Window.Maximize();
            Thread.Sleep(3000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.StartPage.SearchField))).
                SendKeys(website.Pages.StartPage.ItemForSearch + Keys.Enter);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.ProductListPage.SelectedItem))).Click();
            var expectedResult = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.ProductDetailsPage.NameOfItem))).
                Text.ToLower().Replace(",", string.Empty);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.ProductDetailsPage.AddToBasketButton))).Click();
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.ProductDetailsPage.BasketButton))).Click();
            var actualResult = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(website.Pages.BasketPage.NameOfItem)))
                .Text.ToLower().Replace(",", string.Empty);
            var actualCount = driver.FindElements(By.XPath(website.Pages.BasketPage.NameOfItem)).Count;

            Assert.Contains(actualResult, expectedResult);
            Assert.Equal(1, actualCount);
            driver.Quit();

        }
    }
}

