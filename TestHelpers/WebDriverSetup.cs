using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;

namespace TestHelpers
{
    public static class WebDriverSetup<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private static IWebDriver _webDriver;
        public static IWebDriver SetUp()
        {
            //The reason for setting up a EventFiringWebDriver is we could be able to take a screenshot or log something when we hit an exception
            //e.g. If we throw an AssertException then we can take a screenshot and save it to a location.
            EventFiringWebDriver firingDriver;
            firingDriver = new EventFiringWebDriver(Activator.CreateInstance(typeof(TWebDriver), new object[] { Constants.DriverLocation }) as IWebDriver);
            _webDriver = firingDriver;
            _webDriver = NavigateToWebsite(_webDriver);

            return _webDriver;
        }


        private static IWebDriver NavigateToWebsite(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl(new Uri("https://mattsharpe.github.io/"));
            return webDriver;
        }
    }
}
