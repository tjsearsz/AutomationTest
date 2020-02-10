using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using Test.Page;

namespace Test.Step
{
    /// <summary>
    /// Class that contains all logic (steps) to search on Ebay
    /// </summary>
    [Binding]
    public class EbaySteps
    {
        //Driver we will use to nagivate in Google Chrome
        private DriverManager manager;
        private EbayPage ebayPage;

        /// <summary>
        /// Method used to load the initial configuration
        /// </summary>
        [BeforeScenario]
        public void LoadTest()
        {
            this.manager = new DriverManager();
            this.ebayPage = new EbayPage(this.manager);

        }
        
        /// <summary>
        /// Step in which the user enters on Ebay webpage
        /// </summary>
        [Given(@"User enters on Ebay")]
        public void GivenUserEntersOnEbay()
        {
            //Accessing into the webpage
            this.manager.GoToUrl("https://www.ebay.com/");

            //If the language on Ebay is not English, we will change it
            if(!this.ebayPage.CurrentLanguage.Equals("English"))
            {
                this.ebayPage.LanguageDropdown.Click();
                this.ebayPage.EnglishOption.Click();
            }      
            
            Thread.Sleep(5000);
        }
        
        /// <summary>
        /// Step in which the user types shoes in the searchbar and clicks search
        /// </summary>
        [When(@"User searches for shoes")]
        public void WhenUserSearchesForShoes()
        {
            //Typing the word shoes in the searchbar
            this.ebayPage.SearchBar.SendKeys("Shoes");

            //Clicking on the "Search" button
            this.ebayPage.SearchButton.Click();
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Step in which the user clicks on "More Filters" option
        /// </summary>
        [When(@"User clicks on More filters\.\.\. option")]
        public void WhenUserClicksOnMoreFilters_Option()
        {
            //Clicking on the Sort button
            this.ebayPage.MoreFilters.Click();
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Step in which the user searches for the brand desired
        /// </summary>
        /// <param name="p0">The brand the user needs</param>
        [When(@"User selects brand ""(.*)""")]
        public void WhenUserSelectsBrand(string p0)
        {
            //Clicking on the Brand section
            this.ebayPage.BrandFilter.Click();

            Thread.Sleep(5000);

            //Searching for the PUMA brand
            this.ebayPage.BrandSearchBar.SendKeys(p0);

            Thread.Sleep(5000);

            //Searching for the PUMA option and clicking on it
            this.ebayPage.SelectChecBoxForSpecificFilter(p0).Click();

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Step in which the user searches for a size desired
        /// </summary>
        /// <param name="p0">The size desired</param>
        [When(@"User selects size (.*)")]
        public void WhenUserSelectsSize(int p0)
        {
            //Clicking on the size filter
            this.ebayPage.SizeFilter.Click();
            Thread.Sleep(5000);

            //Selecting the size 10 option
            this.ebayPage.SelectChecBoxForSpecificFilter(p0.ToString()).Click();

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Step in which the user clicks to apply filters
        /// </summary>
        [When(@"User applies filters")]
        public void WhenUserAppliesFilters()
        {
            //Clicking the apply button
            this.ebayPage.ApplyFilterButton.Click();
            Thread.Sleep(5000);

            Console.WriteLine("Number of results found: " + 
                this.ebayPage.GetNumberOfResults);
        }

        /// <summary>
        /// Step in which the order of the shoes is displayed in ascendant order
        /// </summary>
        [When(@"User orders the results by price in ascendant order")]
        public void WhenUserOrdersTheResultsByPriceInAscendantOrder()
        {
            //Placing mouse on top of the sort button
            this.manager.PeformMouseOver(this.ebayPage.SortButton);
            Thread.Sleep(5000);

            //Clicking on the price in ascendant order
            this.ebayPage.AscendantPrice.Click();

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Step in which we assert that the order of the shoes are displayed correctly
        /// </summary>
        [Then(@"The order of shoes gets displayed correctly")]
        public void ThenTheOrderOfShoesGetsDisplayedCorrectly()
        {
            //Getting the list of the prices for five products
            List<Decimal> PricesArray = this.ebayPage.GetPriceOfResults(this.ebayPage.GetResults(5));

            //Getting the list of names for five products
            List<String> NamesArray = this.ebayPage.GetNameOfResults(this.ebayPage.GetResults(5));

            //Getting the shipping cost for five products
            List<Decimal> ShippingArray = this.ebayPage.GetShippingPriceOfResults(this.ebayPage.GetResults(5));

            //List that will contain the final price (base price + shipping)
            List<Decimal> FinalPrice = new List<decimal>();

            //Interating through every amount and performing Base price plus shipping
            for (int i = 0; i < PricesArray.Count; i++)            
                FinalPrice.Add(PricesArray[i] + ShippingArray[i]);

            //Asserting that the order is the one as expected
            Assert.IsTrue(FinalPrice.SequenceEqual(FinalPrice.OrderBy(a => a)));

            //Printing in the console
            for (int i = 0; i < FinalPrice.Count; i++)
            {
                Console.WriteLine("Product number " + (i + 1) + " ---> Name: " 
                    + NamesArray[i] + " | Price (Including Shipping): " + FinalPrice[i]);
            }
            Thread.Sleep(5000);
        }

        [When(@"User orders the results by name in ascendant order")]
        public void WhenUserOrdersTheResultsByNameInAscendantOrder()
        {
            //Placing mouse on top of the sort button
            Console.WriteLine("aca llegue");
            Thread.Sleep(5000);



        }
        
        /// <summary>
        /// Step in which the order of shoes is displayed in descendant order
        /// </summary>
        [When(@"User orders the results by price in descendant order")]
        public void WhenUserOrdersTheResultsByPriceInDescendantOrder()
        {
            //Placing mouse on top of the sort button
            this.manager.PeformMouseOver
                (this.ebayPage.SortButton);
            Thread.Sleep(5000);

            //Clicking on the price in ascendant order
            this.ebayPage.DescendantPrice.Click();

            Thread.Sleep(5000);

            //Getting the list of the prices for five products
            List<Decimal> PricesArray = this.ebayPage.GetPriceOfResults(this.ebayPage.GetResults(5));

            //Getting the list of names for five products
            List<String> NamesArray = this.ebayPage.GetNameOfResults(this.ebayPage.GetResults(5));

            //Getting the shipping cost for five products
            List<Decimal> ShippingArray = this.ebayPage.GetShippingPriceOfResults(this.ebayPage.GetResults(5));

            //List that will contain the final price (base price + shipping)
            List<Decimal> FinalPrice = new List<decimal>();

            //Interating through every amount and performing Base price plus shipping
            for (int i = 0; i < PricesArray.Count; i++)
                FinalPrice.Add(PricesArray[i] + ShippingArray[i]);

            //Printing in the console
            for (int i = 0; i < FinalPrice.Count; i++)
            {
                Console.WriteLine("Product number " + (i + 1) + " ---> Name: "
                    + NamesArray[i] + " | Price (Including Shipping): " + FinalPrice[i]);
            }

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Method used to close the chromedriver
        /// </summary>
        [AfterScenario]
        public void CloseTest()
        {
            this.manager.QuitTest();
        }
    }
}
