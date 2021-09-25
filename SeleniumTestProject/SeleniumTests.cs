using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Navigate().GoToUrl(configElements.UrlAdress);
            driver.FindElement(By.XPath(configElements.SearchField)).SendKeys(configElements.ItemForSearch);
            driver.FindElement(By.XPath(configElements.SearchButton)).Click();
            var expectedResult = (driver.FindElement(By.XPath(configElements.SelectedItemInSearch))).Text.ToLower().Replace(",", string.Empty);
            driver.FindElement(By.XPath(configElements.SelectedItemInSearch)).Click();
            driver.FindElement(By.XPath(configElements.AddToBasketButton)).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath(configElements.BasketButton)).Click();
            var actualResult = (driver.FindElement(By.XPath(configElements.ItemInBasket))).Text.ToLower().Replace(",", string.Empty);
            var actualCount = (driver.FindElements(By.XPath(configElements.ItemInBasket))).Count;

            Assert.Contains(actualResult, expectedResult);
            Assert.Equal(1, actualCount);
            driver.Quit();

        }
    }
}

