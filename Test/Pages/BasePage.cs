using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

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
        private WebDriverWait waitManager;

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

            //Creating the explicit wait
            this.waitManager = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

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

        /// <summary>
        /// Method that performs the explicit wait on the element we want
        /// </summary>
        /// <param name="element">The element we need to wait to appear</param>
        protected void WaitForElement(IWebElement element)
        {
            this.waitManager.Until(d => element.Displayed);
        }

        /// <summary>
        /// Method that performs the explicit wait on a list of elements we need to have
        /// </summary>
        /// <param name="list">The list of elements</param>
        protected void WaitForElement(IList<IWebElement> list)
        {
            this.waitManager.Until(d => { return list.Count > 0; });
        }

        /// <summary>
        /// Method that performs the explicit wait on an element/lists of elements until they are no longer visible
        /// </summary>
        /// <param name="element">The element we need to disappear</param>
        protected void WaitForElementGoInvisible(IWebElement element)
        {
            this.waitManager.Until(d =>
            {
                /*We will wait until the element is not visible
                 * if any of those two exception are thrown. The element is no longer there*/
                try
                {
                    return !element.Displayed;
                }
                catch (NoSuchElementException e)
                {
                    return true;
                }
                catch (StaleElementReferenceException e)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Method that performs the explicit wait on an element/lists of elements until they are no longer visible
        /// </summary>
        /// <param name="elements">the raw By on which we will find the element(s)</param>
        protected void WaitForElementGoInvisible(By elements)
        {
            this.waitManager.Until(ExpectedConditions.InvisibilityOfElementLocated(elements));
        }        

        /// <summary>
        /// Method that waits for a list of element to appear and obtains them
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected IList<IWebElement> WaitAndGetElements (By element)
        {
           return  this.waitManager.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy
                        (element));
        }
    }
}
