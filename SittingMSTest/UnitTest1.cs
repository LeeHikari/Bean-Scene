using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace SittingMSTest
{
    [TestClass]
    public class UnitTest1
    {
        private string appUrl = "https://localhost:44393/";

        [TestMethod]
        public void ChromeCreateSittingsTest()
        {
            IWebDriver driver = null;
            try
            {
                driver = new ChromeDriver();
                driver.Navigate().GoToUrl(appUrl);
                driver.FindElement(By.Id("Dashboard")).Click();
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
