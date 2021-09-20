using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace SeleniumTestProject
{
    class JsonReaderItem
    {
        public string urlAdress { get; set; }
        public string searchField { get; set; }
        public string itemForSearch { get; set; }
        public string searchButton { get; set; }
        public string selectedItemInSearch { get; set; }
        public string addToBasketButton { get; set; }
        public string basketButton { get; set; }
        public string itemInBasket { get; set; }
    }
    public class SeleniumTests
    {

        [Theory]
        [InlineData(@"ELDinput.json")]
        [InlineData(@"DNSinput.json")]

        public async void UniversalShopTesting(string pathToJsonFile)
        {
            using (FileStream fs = new FileStream(pathToJsonFile, FileMode.OpenOrCreate))
            {

                JsonReaderItem readJsonFile = await System.Text.Json.JsonSerializer.DeserializeAsync<JsonReaderItem>(fs);
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--disable-notifications");
                IWebDriver driver = new ChromeDriver(options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                driver.Navigate().GoToUrl(readJsonFile.urlAdress);
                driver.FindElement(By.XPath(readJsonFile.searchField)).SendKeys(readJsonFile.itemForSearch);
                driver.FindElement(By.XPath(readJsonFile.searchButton)).Click();
                var expectedResult = (driver.FindElement(By.XPath(readJsonFile.selectedItemInSearch))).Text.ToLower().Replace(",", string.Empty);
                driver.FindElement(By.XPath(readJsonFile.selectedItemInSearch)).Click();
                driver.FindElement(By.XPath(readJsonFile.addToBasketButton)).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath(readJsonFile.basketButton)).Click();
                var actualResult = (driver.FindElement(By.XPath(readJsonFile.itemInBasket))).Text.ToLower().Replace(",",string.Empty) ;
                var actualCount = (driver.FindElements(By.XPath(readJsonFile.itemInBasket))).Count;

                Assert.Contains(actualResult, expectedResult);
                Assert.Equal(1, actualCount);
                driver.Quit();
            }
        }
    }
}

