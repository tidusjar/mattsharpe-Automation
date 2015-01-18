using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace MattsharpeAutomation
{
    public class Page
    {

        [FindsBy(How = How.CssSelector, Using = "input:invalid")]
        public IList<IWebElement> InvalidInputElements { get; set; }

    }
}
