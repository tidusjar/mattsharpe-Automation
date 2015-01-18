using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MattsharpeAutomation;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TestHelpers;

namespace Tests
{
    [TestFixture(typeof(ChromeDriver)), Category("Chrome")]
    [TestFixture(typeof(InternetExplorerDriver)), Category("IE")]
    [TestFixture(typeof(FirefoxDriver)), Category("Firefox")]
    [Description("This tests the functionality on the https://mattsharpe.github.io/ website.")]
    public class HomePageTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _webDriver;
        private HomePage _homePage;

        /// <summary>
        /// Acceptance Criteria:
        /// 
        ///A user should be able to fill in all the fields and save a risk.
        ///They should be informed that either the save was successful or a validation error occured.
        ///All fields are mandatory
        ///When changing the risk status to approved, the approval date should be set to today's date.
        ///The approval date should not be in the past.
        /// </summary>

        [SetUp]
        public void MyTestInitialize()
        {
            _webDriver = WebDriverSetup<TWebDriver>.SetUp();
            _homePage = new HomePage(_webDriver);
        }


        [TearDown]
        public void MyTestCleanup()
        {
            _webDriver.Dispose();
        }

        [Test]
        [Description("Positive Test")]
        public void SuccessfulOpenStatusSave()
        {
            Assert.IsTrue(_homePage.OpenStatus(), "We did not get a success message.");
        }

        [Test]
        public void CheckInstructionsArePresent()
        {
            Assert.IsTrue(_homePage.InstructionsPresent());
        }

        [Test]
        [Description("When changing the risk status to approved, the approval date should be set to today's date.")]
        public void AssertIfApprovedStatusThenShouldBeSetTodaysDate()
        {
            Assert.IsTrue(_homePage.SetApprovedAndValidate(),"The date was not correct: " + _homePage.ApprovalDate.GetAttribute("value"));
        }

        [Test]
        [Description("The approval date should not be in the past.")]
        public void CheckDateCannotBeInThePast()
        {
            Assert.IsTrue(_homePage.SetDateAndFillInInformation(DateTime.Now.AddDays(-5)),"We did not get an error message!");
        }

        [Test]
        [Description("All fields are mandatory")]
        public void ValidationCheck()
        {
            Assert.IsTrue(_homePage.Validate(),"Validation failed.");
        }
    }
}
