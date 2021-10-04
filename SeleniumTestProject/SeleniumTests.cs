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
    class ConfigElements
    {
        public string UrlAdress { get; set; }
        public string SearchField { get; set; }
        public string ItemForSearch { get; set; }
        public string SearchButton { get; set; }
        public string SelectedItemInSearch { get; set; }
        public string NameOfItem { get; set; }
        public string AddToBasketButton { get; set; }
        public string BasketButton { get; set; }
        public string ItemInBasket { get; set; }
    }
    public class SeleniumTests
    {

        [Theory]
        [InlineData(@"ConfigFiles\ELDORADO.json")]
        [InlineData(@"ConfigFiles\DNS.json")]
        [InlineData(@"ConfigFiles\WILDBERRIES.json")]

        public void UniversalShopTesting(string pathToConfigFile)
        {

            var configElements = JsonConvert.DeserializeObject<ConfigElements>(File.ReadAllText(pathToConfigFile));
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            IWebDriver driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl(configElements.UrlAdress);
            driver.Manage().Window.Maximize();
            Thread.Sleep(3000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.SearchField))).SendKeys(configElements.ItemForSearch + Keys.Enter);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.SelectedItemInSearch))).Click();
            var expectedResult = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.NameOfItem))).Text.ToLower().Replace(",", string.Empty);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.AddToBasketButton))).Click();
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.BasketButton))).Click();
            var actualResult = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(configElements.ItemInBasket))).Text.ToLower().Replace(",", string.Empty);
            var actualCount = driver.FindElements(By.XPath(configElements.ItemInBasket)).Count;

            Assert.Contains(actualResult, expectedResult);
            Assert.Equal(1, actualCount);
            driver.Quit();

        }
    }
}

