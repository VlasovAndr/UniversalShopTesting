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
        public string UrlAdress { get; set; }
        public string SearchField { get; set; }
        public string ItemForSearch { get; set; }
        public string SearchButton { get; set; }
        public string SelectedItemInSearch { get; set; }
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

        public async void UniversalShopTesting(string pathToJsonFile)
        {
            using (FileStream fs = new FileStream(pathToJsonFile, FileMode.OpenOrCreate))
            {

                JsonReaderItem readJsonFile = await System.Text.Json.JsonSerializer.DeserializeAsync<JsonReaderItem>(fs);
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--disable-notifications");
                IWebDriver driver = new ChromeDriver(options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                driver.Navigate().GoToUrl(readJsonFile.UrlAdress);
                driver.FindElement(By.XPath(readJsonFile.SearchField)).SendKeys(readJsonFile.ItemForSearch);
                driver.FindElement(By.XPath(readJsonFile.SearchButton)).Click();
                var expectedResult = (driver.FindElement(By.XPath(readJsonFile.SelectedItemInSearch))).Text.ToLower().Replace(",", string.Empty);
                driver.FindElement(By.XPath(readJsonFile.SelectedItemInSearch)).Click();
                driver.FindElement(By.XPath(readJsonFile.AddToBasketButton)).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath(readJsonFile.BasketButton)).Click();
                var actualResult = (driver.FindElement(By.XPath(readJsonFile.ItemInBasket))).Text.ToLower().Replace(",",string.Empty) ;
                var actualCount = (driver.FindElements(By.XPath(readJsonFile.ItemInBasket))).Count;

                Assert.Contains(actualResult, expectedResult);
                Assert.Equal(1, actualCount);
                driver.Quit();
            }
        }
    }
}

