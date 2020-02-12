using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Test.Pages
{
    /// <summary>
    /// Base class for the Page Object pattern
    /// </summary>
    public class BasePage
    {
        //Attribute for the class
        protected IWebDriver driver;
        private Actions action;        

        /*Constructor that will call a factory to instanciate
        * this page, the driver and set initial configurations*/
        public BasePage(IWebDriver driver)
        {
            //Setting the value of the driver
            PageFactory.InitElements(driver, this);

            //Asigning the driver
            this.driver = driver;

            //Creating new actions responsible for trigger events
            this.action = new Actions(this.driver);

        }

        /// <summary>
        /// Method to perform a mouseover action
        /// </summary>
        /// <param name="element"> The element we will perform the mouse over</param>
        public void PeformMouseOver(IWebElement element)
        {                    
            this.action.MoveToElement(element).Perform();               
        }

        /// <summary>
        /// Method used to go to a given URL
        /// </summary>
        /// <param name="url">The url we want to go</param>
        public void GoToUrl(String url)
        {
            this.driver.Navigate().GoToUrl(url);
        }
    }
}
