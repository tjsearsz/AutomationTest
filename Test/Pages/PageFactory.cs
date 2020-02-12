using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Pages
{
    /// <summary>
    /// Factory class for all the webpages
    /// </summary>
    [Obsolete("This class got replaced by DotNetLibrary in order to be consistent", true)]
    public class MainPageFactory
    {
        /// <summary>
        /// Factory for the Ebay Page
        /// </summary>
        /// <param name="driver">driver for the tests</param>
        /// <returns>The Ebay Page</returns>
        [Obsolete("This method got replaced by DotNetLibrary in order to be consistent", true)]
        public static EbayPage CreateEbayPage(IWebDriver driver)
        {
            return new EbayPage(driver);
        }

        /// <summary>
        /// Factory for the base page
        /// </summary>
        /// <param name="driver">driver for the tests</param>
        /// <returns>The base page</returns>
        [Obsolete("This class got replaced by DotNetLibrary in order to be consistent", true)]
        public static BasePage CreateBasePage(IWebDriver driver)
        {
            return new BasePage(driver);
        }
    }
}
