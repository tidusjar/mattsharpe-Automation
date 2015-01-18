using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace TestHelpers
{
    public static class Helper
    {
        static readonly Random Rand = new Random();

        #region Strings
        private const string Alpha = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endregion
        public static bool Exists(this IWebElement element)
        {
            try
            {
                var text = element.Text;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public static bool Exists(this IList<IWebElement> element)
        {
            try
            {
                foreach (var webElement in element)
                {
                    var text = webElement.Text;
                }

            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }
        public static string GenerateAlphaString(int size)
        {
            var chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alpha[Rand.Next(Alpha.Length)];
            }
            return new string(chars);
        }
    }
}
