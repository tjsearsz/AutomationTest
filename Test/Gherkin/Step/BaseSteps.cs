using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;

namespace Test.Gherkin.Step
{
    /// <summary>
    /// Class that contains all the global logics for any steps/test to be implemented
    /// </summary>
    [Binding]
    public class BaseSteps
    {
        //Attributes of the class
        protected static IWebDriver driver;

        /// <summary>
        /// Method used to load the initial configuration
        /// </summary>
        [BeforeFeature(), Scope(Feature = "AutomationTest")]
        public static void LoadTest()
        {
            //Asigning the chrome driver and setting the default wait
            driver = new ChromeDriver();            

            //Opening browser in full window size
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Method used to close the chromedriver
        /// </summary>
        [AfterFeature(), Scope(Feature = "AutomationTest")]
        public static void CloseTest()
        {
            driver.Quit();
        }

    }
}
