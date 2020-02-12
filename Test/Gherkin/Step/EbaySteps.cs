using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Test.Domain;
using Test.Pages;

namespace Test.Gherkin.Step
{
    /// <summary>
    /// Class that contains all logic (steps) to search on Ebay
    /// </summary>
    [Binding]
    public class EbaySteps //: BaseSteps
    {
        //Driver we will use to nagivate in Google Chrome
        private EbayPage ebayPage;

        //Attributes of the class
        protected IWebDriver driver;

        

        /// <summary>
        /// Method used to load the initial configuration
        /// </summary>
        [BeforeScenario]
        public void LoadTest()
        {
            //Asigning the chrome driver and setting the default wait
            this.driver = new ChromeDriver();

            //Setting the default wait
            //this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(40);

            //this.waitManager = new WebDriverWait(this.driver, TimeSpan.FromSeconds(20));
            

            //Instanciating EbayPage            
            this.ebayPage = new EbayPage(this.driver);

            //Opening browser in full window size
            this.driver.Manage().Window.Maximize();            
        }

        /// <summary>
        /// Method used to close the chromedriver
        /// </summary>
        [AfterScenario]
        public void CloseTest()
        {
            this.driver.Quit();
        }

        /// <summary>
        /// Step in which the user enters on Ebay webpage
        /// </summary>
        [Given(@"User enters on Ebay")]
        public void GivenUserEntersOnEbay()
        {
            //Accessing into the webpage
            this.ebayPage.GoToUrl("https://www.ebay.com/");

            //Changing the language to english
            this.ebayPage.ChangeLanguageToEnglish();    
        }

        /// <summary>
        /// Step in which the user types shoes in the searchbar and clicks search
        /// </summary>
        [When(@"User searches for ""(.*)""")]
        public void WhenUserSearchesFor(string p0)
        {
            //Typing the word shoes in the searchbar
            this.ebayPage.SearchProduct(p0);            
        }      

        /// <summary>
        /// Step in which the user clicks on "More Filters" option
        /// </summary>
        [When(@"User clicks on More filters\.\.\. option")]
        public void WhenUserClicksOnMoreFilters_Option()
        {
            //Clicking on the Sort button
            this.ebayPage.OpenMoreFiltersOption();            
        }

        /// <summary>
        /// Step in which the user searches for the brand desired
        /// </summary>
        /// <param name="p0">The brand the user needs</param>
        [When(@"User selects brand ""(.*)""")]
        public void WhenUserSelectsBrand(string p0)
        {
            this.ebayPage.FilterForASpecificBrand(p0);            
        }

        /// <summary>
        /// Step in which the user searches for a size desired
        /// </summary>
        /// <param name="p0">The size desired</param>
        [When(@"User selects size (.*)")]
        public void WhenUserSelectsSize(int p0)
        {
            this.ebayPage.FilterForASpecificSize(p0);            
        }

        /// <summary>
        /// Step in which the user clicks to apply filters
        /// </summary>
        [When(@"User applies filters")]
        public void WhenUserAppliesFilters()
        {
            this.ebayPage.ApplyFilters();
        }

        /// <summary>
        /// Step in which the order of the shoes is displayed in ascendant order
        /// </summary>
        [When(@"User orders the results by price in ascendant order")]
        public void WhenUserOrdersTheResultsByPriceInAscendantOrder()
        {
            this.ebayPage.SortResultsByPriceAscendantOrder();
        }

        /// <summary>
        /// Step in which we assert that the order of the shoes are displayed correctly
        /// </summary>
        [Then(@"The order of shoes gets displayed correctly")]
        public void ThenTheOrderOfShoesGetsDisplayedCorrectly()
        {
            //Getting a list of products from the result we have found
            List<Product> productsList = this.ebayPage.GetListOfProducts(5);            

            //Asserting that the order is the one as expected
            Assert.IsTrue(productsList.SequenceEqual(productsList.OrderBy(a => a.FinalPrice)));            


            //Printing in the console
            for (int i = 0; i < productsList.Count; i++)
            {
                Console.WriteLine("Product number " + (i + 1) + " ---> " + productsList[i].ToString());
            }

            //Sorting products by name (ascendant) and printing them in console
            productsList = productsList.OrderBy(d => d.Name).ToList();
            Console.WriteLine("\n Sorting products by name (ascendant): \n");
            for (int i = 0; i < productsList.Count; i++)
            {
                Console.WriteLine("Product number " + (i + 1) + " ---> " + productsList[i].ToString());
            }

            //Sorting products by price (descendant) and printing them in console
            productsList = productsList.OrderByDescending(d => d.FinalPrice).ToList();
            Console.WriteLine("\n Sorting products by price (descendant): \n");
            for (int i = 0; i < productsList.Count; i++)
            {
                Console.WriteLine("Product number " + (i + 1) + " ---> " + productsList[i].ToString());
            }
        }
    }
}
