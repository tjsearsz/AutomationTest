using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace Test
{
    /// <summary>
    /// Class in charge of manipulating all the driver actions
    /// </summary>
    public class DriverManager
    {
        //Attributes of the class
        private readonly IWebDriver driver;
        private Actions action;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public DriverManager()
        {
            //Asigning the chrome driver and setting the default wait
            this.driver = new ChromeDriver();
            
            //Setting the default wait
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //Opening browser in full window size
            this.driver.Manage().Window.Maximize();

            //Creating new actions responsible for trigger events
            this.action = new Actions(this.driver);
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
        /// This Methods closes the driver, therefore every window
        /// </summary>
        public void QuitTest()
        {
            this.driver.Quit();
        }

        /// <summary>
        /// Property to get the driver of the test
        /// </summary>
        public IWebDriver Driver
        {
            get { return this.driver; }
        }

        /// <summary>
        /// Method to perform a mouseover action
        /// </summary>
        /// <param name="element"> The element we will perform the mouse over</param>
        public void PeformMouseOver(IWebElement element)
        {
            try
            {
                //Performing the action of moseover
                this.action.MoveToElement(element).Perform();
            }
            catch(StaleElementReferenceException ex)
            {
                /*When the page reloads, this action gets lost.
                 * Therefore, we need to instanciate it again.*/
                this.action = new Actions(this.driver);

                //Performing the action of moseover
                this.action.MoveToElement(element).Perform();
            }            
        }
    }
}
