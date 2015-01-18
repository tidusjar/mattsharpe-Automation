using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using TestHelpers;

namespace MattsharpeAutomation
{
    public class HomePage : Page
    {
        private IWebDriver _webDriver;

        public HomePage(IWebDriver webDriver)
        {
            this._webDriver = webDriver;

            PageFactory.InitElements(webDriver, this);
        }
        #region Web Elements

        #region Nav
        [FindsBy(How = How.LinkText, Using = "Test")]
        public IWebElement TestTab { get; set; }
        
        [FindsBy(How = How.LinkText, Using = "Instructions")]
        public IWebElement InstructionsTab { get; set; }
        #endregion

        #region Test Page
        [FindsBy(How = How.Id, Using = "RiskTitle")]
        public IWebElement RiskTitle { get; set; }
        
        [FindsBy(How = How.Id, Using = "status")]
        public IWebElement StatusDropDown { get; set; }

        [FindsBy(How = How.Id, Using = "approvalDate")]
        public IWebElement ApprovalDate { get; set; }
        
        [FindsBy(How = How.Id, Using = "owner")]
        public IWebElement OwnerEmail { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id=\"form\"]/button")]
        public IWebElement SaveButton { get; set; }

        
        [FindsBy(How = How.XPath, Using = "//*[@id=\"test\"]/div[1]")]
        public IWebElement SuccessMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"test\"]/div[2]")]
        public IWebElement ErrorMessage { get; set; }

        #endregion

        #region Instructions Page

        [FindsBy(How = How.XPath, Using = "//*[@id=\"instructions\"]/h1")]
        public IWebElement InstructionsTitle { get; set; }

        [FindsBy(How = How.Id, Using = "instructions")]
        public IWebElement InstructionsContent { get; set; }


        #endregion

        #endregion



        public bool InstructionsPresent()
        {
            InstructionsTab.Click();
            //Resouce the text.
            if (InstructionsTitle.Text == "Sword Active Risk - QA Interview Test" && InstructionsContent.Exists())
            {
                return true;
            }
            return false;
        }

        public bool OpenStatus()
        {
            
            RiskTitle.SendKeys("Mr");
            var statusSelector = new SelectElement(StatusDropDown);
            statusSelector.SelectByIndex(0);
            var dateTime = DateTime.Now.ToString("dd-MMM-yyyy");

            ApprovalDate.SendKeys("01");
            ApprovalDate.SendKeys("May");
            ApprovalDate.SendKeys(Keys.Tab);
            ApprovalDate.SendKeys("2015");

            OwnerEmail.SendKeys(GenerateEmail());

            SaveButton.Click();
            Reinitilize();
            var a = InvalidInputElements;
            if (InvalidInputElements.Exists())
            {
                if (InvalidInputElements.Any(element => element.Exists()))
                {
                    return false;
                }
            }
            if (SuccessMessage.Exists())
            {
                
                return true;
            }
            
            return false;
        }

        private string GenerateEmail()
        {
            return Helper.GenerateAlphaString(6) + "@" + Helper.GenerateAlphaString(6) + "." +
                    Helper.GenerateAlphaString(3);
        }

        public bool SetApprovedAndValidate()
        {
            RiskTitle.SendKeys("Mr");
            var statusSelector = new SelectElement(StatusDropDown);
            statusSelector.SelectByIndex(3); //Approved Status
            OwnerEmail.SendKeys(GenerateEmail());

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            Reinitilize();
            var currentValue = ApprovalDate.GetAttribute("value");
            if (currentValue == date)
            {
                return true;
            }
            

            return false;
        }

        public bool SetDateAndFillInInformation(DateTime date)
        {
            RiskTitle.SendKeys("Mr");
            var statusSelector = new SelectElement(StatusDropDown);
            statusSelector.SelectByIndex(3); //Approved Status
            OwnerEmail.SendKeys(GenerateEmail());

            //Done Via jQuery, just a different wat to do things
            ((IJavaScriptExecutor)_webDriver).ExecuteScript(string.Format("$('#approvalDate').val('{0}');", date.ToString("yyyy-MM-dd")));
            SaveButton.Click();
            if (ErrorMessage.Exists() && !string.IsNullOrEmpty(ErrorMessage.Text))
            {
                return true;
            }

            return false;
        }

        public bool Validate()
        {
            var errors = 0;
            SaveButton.Click();
            errors += ErrorCheck(); //Nothing is filled in so we should have 1 error

            RiskTitle.SendKeys("Mrs");
            SaveButton.Click();
            errors += ErrorCheck(); //We are missing approved date and email. 2 errors

            ((IJavaScriptExecutor)_webDriver).ExecuteScript(string.Format("$('#approvalDate').val('{0}');", DateTime.Now.ToString("yyyy-MM-dd")));
            SaveButton.Click();
            errors += ErrorCheck(); //We are missing and email. 3 errors

            OwnerEmail.SendKeys(GenerateEmail());
            SaveButton.Click();
            errors += ErrorCheck(); //We are not missing anything = success message
            if (SuccessMessage.Exists() && !string.IsNullOrEmpty(SuccessMessage.Text))
            {
                if (errors == 3)
                {
                    return true;
                }
            }
            return false;
        }
        private void Reinitilize()
        {
            PageFactory.InitElements(_webDriver,this);
        }

        public int ErrorCheck()
        {
            if (ErrorMessage.Exists() && !string.IsNullOrEmpty(ErrorMessage.Text))
            {
                return 1;
            }
            return 0;
        }

    }
}
