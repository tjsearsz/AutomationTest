using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using Test.Domain;

namespace Test.Pages
{
    /// <summary>
    /// Class that contains all the element on Ebay's webpage
    /// </summary>
    public class EbayPage : BasePage
    {
        // Search bar element
        [FindsBy(How = How.Id, Using = "gh-ac")]
        private IWebElement searchBar;       

        // Search button element
        [FindsBy(How = How.Id, Using = "gh-btn")]
        private IWebElement searchButton;

        // More filters button element
        [FindsBy(How = How.CssSelector, Using = ".x-refine__main__list--more button")]
        private IWebElement moreFilters;

        // Property to get the brand filter in the filters' modal
        [FindsBy(How = How.Id , Using = "refineOverlay-mainPanel-Brand")]
        private IWebElement brandFilter;

        // Size filter element
        [FindsBy(How = How.Id, Using = "refineOverlay-mainPanel-US%20Shoe%20Size%20%28Men%27s%29")]
        private IWebElement sizeFilter;

        //Brand search bar element
        [FindsBy(How = How.Id, Using = "refineOverlay-subPanel-_x-searchable-0[0]")]
        private IWebElement brandSearchBar;

        [FindsBy(How = How.CssSelector, Using = "div.x-overlay-sub-panel__col > div")]
        private IList<IWebElement> FilterOptionsList;

        //Apply filter button element
        [FindsBy(How = How.CssSelector, Using = ".x-overlay-footer__apply > button")]
        private IWebElement applyFilterButton;

        //Sort Button Element
        [FindsBy(How = How.CssSelector, Using = ".x-flyout__button")]
        private IList<IWebElement> sortButton;      

        //Number of results elements
        [FindsBy(How = How.CssSelector, Using = "h1.srp-controls__count-heading > span.BOLD")]
        IList<IWebElement> numberOfResults;        

        //Get ascendant sort option element
        [FindsBy(How = How.CssSelector, Using = ".srp-sort__menu > li")]
        IList<IWebElement> ascendantPrice;

        //Cancel button located in filter's modal
        [FindsBy(How = How.CssSelector, Using = ".x-overlay-footer__cancel > button")]
        private IWebElement FiltersModalCancelButton;        

        #region Language
        //Language dropdown element
        [FindsBy(How = How.Id, Using = "gh-eb-Geo")]
        private IWebElement languageDropdown;

        //Current language element
        [FindsBy(How = How.CssSelector, Using = "#gh-eb-Geo-a-default > span.gh-eb-Geo-txt")]
        private IWebElement currentLanguage;
        
        // English option element
        [FindsBy(How = How.CssSelector, Using = "#gh-eb-Geo-o > ul > li")]
        private IWebElement englishOption;        

        /// <summary>
        /// This method changes the current ebay language to english
        /// </summary>
        public void ChangeLanguageToEnglish()
        {
            this.waitManager.Until(d => languageDropdown.Displayed);
            //If the language on Ebay is not English, we will change it
            if ( !this.currentLanguage.Text.Equals("English"))
            {
                this.languageDropdown.Click();

                this.waitManager.Until(d => englishOption.Displayed);
                this.englishOption.Click();
            }
        }
        #endregion

        private OpenQA.Selenium.Support.UI.WebDriverWait waitManager;
        

        /// <summary>
        /// Contructor that receives the driver of the whole application
        /// </summary>
        /// <param name="driver">The driver of the test</param>
        public EbayPage(IWebDriver driver) : base(driver)
        {
            this.waitManager = new OpenQA.Selenium.Support.UI.WebDriverWait(this.driver, TimeSpan.FromSeconds(20));
        }
        
        /// <summary>
        /// Method to search for a product using the upper search bar
        /// </summary>
        public void SearchProduct(String product)
        {
            this.waitManager.Until(d => searchBar.Displayed);
            this.searchBar.SendKeys(product);
            this.waitManager.Until(d => searchButton.Displayed);
            this.searchButton.Click();
        }

        /// <summary>
        /// Method to open more filter modal
        /// </summary>
        public void OpenMoreFiltersOption()
        {
            this.waitManager.Until(d => moreFilters.Displayed);
            this.moreFilters.Click();
        }

        /// <summary>
        /// Method to filter results based on a specific brand of shoes
        /// <param name="brand">The brand the user needs</param>
        /// </summary>
        public void FilterForASpecificBrand(String brand)
        {
            
            bool brandOptionsFound = false;

            /*Sometimes, the filter modal does not load. It stays in an infinite loop 
             * to avoid a timeout, we just simply retry closing and opening the modal
             */
            while(!brandOptionsFound)
            {
                try
                {
                    this.waitManager.Until(d => brandFilter.Displayed);
                    //Clicking on the Brand section
                    this.brandFilter.Click();

                    this.waitManager.Until(d => brandSearchBar.Displayed);

                    //Searching for the PUMA brand
                    this.brandSearchBar.SendKeys(brand);

                    //Searching for the PUMA option and clicking on it
                    this.SelectCheckBoxForSpecificFilter(brand).Click();

                    //If everything goes well, we just simply don't do this step
                    brandOptionsFound = true;
                }
                catch (WebDriverTimeoutException ex)
                {
                    //Clicking on cancel to close the modal and reopen it again
                    this.waitManager.Until(d => FiltersModalCancelButton.Displayed);
                    this.FiltersModalCancelButton.Click();

                    //Opening the modal again
                    OpenMoreFiltersOption();
                }
            }            
        }

        /// <summary>
        /// Method to filter results based on a specific size of shoes
        /// </summary>
        /// <param name="size"></param>
        public void FilterForASpecificSize(int size)
        {
            this.waitManager.Until(d => sizeFilter.Displayed);
            //Clicking on the size filter
            this.sizeFilter.Click();            

            //Selecting the size 10 option
            this.SelectCheckBoxForSpecificFilter(size.ToString()).Click();
        }

        /// <summary>
        /// Method to apply all the filters we have selected
        /// </summary>
        public void ApplyFilters()
        {
            this.waitManager.Until(d => applyFilterButton.Displayed);
            //Clicking the apply button
            this.applyFilterButton.Click();            
        }

        /// <summary>
        /// Method to sort results by price using ascendant order
        /// </summary>
        public void SortResultsByPriceAscendantOrder()
        {
            this.waitManager.Until(d => { return sortButton.Count > 0; });
            this.waitManager.Until(d => { return sortButton[3].Displayed; });

            this.WaitForPageReloadAfterApplyingFilters();

            //We print the number of results
            Console.WriteLine("\n Number of results found: " +
                this.numberOfResults[0].Text + "\n");

            //Placing mouse on top of the sort button
            PeformMouseOver(this.sortButton[3]);

            this.waitManager.Until(d => { return ascendantPrice.Count > 0; });

            //Clicking on the price in ascendant order
            this.ascendantPrice[3].Click();
        }

        /// <summary>
        /// Method to get a list of products
        /// </summary>
        /// <param name="amount">The amount of products we want</param>
        /// <returns>List with products</returns>
        public List<Product> GetListOfProducts(int amount)
        {
            List<Product> ProductsList = new List<Product>();

            foreach(IWebElement aux in GetRawDataOfProducts(5))
            {
                //Getting the raw string where the price is located
                String RawPriceText = aux.FindElement(By.CssSelector("span.s-item__price")).Text;

                //Getting rid off the currency
                List<String> ValuesOfText = Regex.Split(RawPriceText, @"[^0-9\.,]+")
                                    .Where(a => a != "." && a.Trim() != "").ToList();

                Decimal priceAux = Decimal.Parse(ValuesOfText[0].Trim());

                //Getting the raw string where the name is located
                String NameAux = aux.FindElement(By.CssSelector("a.s-item__link > h3")).Text;

                //If shipping has a value, we will updated. Otherwise, we will add 0
                Decimal shippingAux = 0;

                /*If the item has a shipping text, and it is 
                 * indeed a valid price (no free shipping text or any other label)
                 * we will add it as shipping cost
                */
                if (aux.FindElements(By.CssSelector("span.s-item__logisticsCost")).Count > 0
                    && aux.FindElement(By.CssSelector("span.s-item__logisticsCost")).Text.Any(char.IsDigit))
                {
                    String RawShippingText = aux.FindElement(By.CssSelector("span.s-item__logisticsCost")).Text;

                    //Getting rid off the currency
                    List<String> ValuesOfShipping = Regex.Split(RawShippingText, @"[^0-9\.]+")
                                        .Where(a => a != "." && a.Trim() != "").ToList();

                    //Adding to the list the price
                    shippingAux = Decimal.Parse(ValuesOfShipping[0].Trim());
                }               

                //Creating new product
                Product newProduct = DomainFactory.CreateProduct
                    (NameAux, priceAux, shippingAux);

                //adding the product to the list
                ProductsList.Add(newProduct);
            }
            //GetPriceOfResults(GetResults(5));
            //GetNameOfResults(GetResults(5));
            //GetShippingPriceOfResults(GetResults(5));
            return ProductsList;
        }

        /// <summary>
        /// Method that gets the desired number of products without having processed them
        /// </summary>
        /// <param name="number">The number of results we want to get</param>
        /// <returns>List with number of desired result without haven't been processed yet</returns>
        private List<IWebElement> GetRawDataOfProducts(int number)
        {
            //List we will return as result
            List<IWebElement> result = new List<IWebElement>();

            /* Iterating through the number of results we want */
            for (int i = 1; i <= number; i ++)
            {
                this.waitManager.Until(d => this.driver.FindElement(By.Id("srp-river-results-listing" + i)));
                /*Since all the results on Ebay have the same id, we just need to
                 * iterate changing the id name with the corresponding number */
                result.Add(this.driver.FindElement(By.Id("srp-river-results-listing" + i)));
            }

            return result;
        }

        /// <summary>
        /// Method that gets all the prices for the list of results we have
        /// </summary>
        /// <param name="results">The results we have from Ebay</param>
        /// <returns>The list of prices for those results</returns>
        private List<Decimal> GetPriceOfResults(List<IWebElement> results)
        {
            //List of prices we will return
            List<Decimal> prices = new List<Decimal>();

            //Iterating through all the results we want to get their prices
            foreach (IWebElement aux in results)
            {
                //Getting the raw string where the price is located
                String RawPriceText = aux.FindElement(By.CssSelector("span.s-item__price")).Text;

                //Getting rid off the currency
                List<String> ValuesOfText = Regex.Split(RawPriceText, @"[^0-9\.,]+")
                                    .Where(a => a != "." && a.Trim() != "").ToList();                

                //Adding to the list the price
                prices.Add(Decimal.Parse(ValuesOfText[0].Trim()));

            }

            return prices;
        }

        /// <summary>
        /// Method that gets the names for the list of results we have
        /// </summary>
        /// <param name="results">The results we have from Ebay</param>
        /// <returns>The list of names for those results</returns>
        private List<String> GetNameOfResults(List<IWebElement> results)
        {
            //List of prices we will return
            List<String> Name = new List<String>();

            //Iterating through all the results we want to get their prices
            foreach (IWebElement aux in results)
            {
                //Getting the raw string where the name is located
                String NameAux = aux.FindElement(By.CssSelector("a.s-item__link > h3")).Text;

                //Adding to the list the price
                Name.Add(NameAux);

            }

            return Name;
        }        

        /// <summary>
        /// Method that gets the shipping cost of a specific amount of products
        /// </summary>
        /// <param name="list">The list of products we want to get their shipping cost</param>
        /// <returns>The shipping cost of each product</returns>
        private List<decimal> GetShippingPriceOfResults(List<IWebElement> list)
        {
            //List of prices we will return
            List<Decimal> prices = new List<Decimal>();

            //Iterating through all the results we want to get their prices
            foreach (IWebElement aux in list)
            {
                /*If the item has a shipping text, and it is 
                 * indeed a valid price (no free shipping text or any other label)
                 * we will add it as shipping cost
                */
                if (aux.FindElements(By.CssSelector("span.s-item__logisticsCost")).Count > 0
                    && aux.FindElement(By.CssSelector("span.s-item__logisticsCost")).Text.Any(char.IsDigit))
                {
                    String RawShippingText = aux.FindElement(By.CssSelector("span.s-item__logisticsCost")).Text;

                    //Getting rid off the currency
                    List<String> ValuesOfText = Regex.Split(RawShippingText, @"[^0-9\.]+")
                                        .Where(a => a != "." && a.Trim() != "").ToList();                    

                    //Adding to the list the price
                    prices.Add(Decimal.Parse(ValuesOfText[0].Trim()));
                }
                //If there are no logisticsCost we simply add 0
                else
                    prices.Add(0);           

            }

            return prices;
        }

        /// <summary>
        /// Method that gets the checkbox for a specific filter
        /// </summary>
        /// <param name="filter">the filter we want to search</param>
        /// <returns>The checkbox of that particular filter</returns>
        public IWebElement SelectCheckBoxForSpecificFilter(string filter)
        {
            //Checkbox we will return
            IWebElement CheckBox = null;

            //Helping flag
            bool ElementFound = false;

            this.waitManager.Until(d => { return this.FilterOptionsList.Count > 0; });

            //For each option displayed, we will iterate to see if that is the one we want
            foreach(IWebElement aux in FilterOptionsList)
            {
                /* Since the name is attached with an amount and another word, we need to split the string
                 * in order to take only the part of the string we need.
                 * Having taking the name, if it is the one we are looking for, then we get its checkbox*/
                if (aux.FindElement(By.CssSelector
                    ("label > div > div > span.cbx.x-refine__multi-select-cbx")).Text.Split("(")[0].Trim().Equals(filter))
                {
                    CheckBox = aux.FindElement(By.CssSelector("label > div > input"));
                    ElementFound = true;
                    break;
                }

                //Stopping the foreach if we have already found the element
                if (ElementFound)
                    break;
            }            

            //Returning the checkbox
            return CheckBox;
        }

        /// <summary>
        /// This method contains a logic to wait for the filters get applied on the page
        /// </summary>
        private void WaitForPageReloadAfterApplyingFilters()
        {
            bool NotRefreshed = true;

            while(NotRefreshed)
                try
                {
                    this.waitManager.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".x-overlay__wrapper--right")));
                    this.sortButton = this.waitManager.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".x-flyout__button"))); //this.driver.FindElements(By.CssSelector(".x-flyout__button"));
                    this.numberOfResults = this.waitManager.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("h1.srp-controls__count-heading > span.BOLD")));  this.driver.FindElements(By.CssSelector("h1.srp-controls__count-heading > span.BOLD"));
                    
                    NotRefreshed = false;
                }
                catch (StaleElementReferenceException ex)
                {
                    this.numberOfResults = this.driver.FindElements(By.CssSelector("h1.srp-controls__count-heading > span.BOLD"));
                    this.sortButton = this.driver.FindElements(By.CssSelector(".x-flyout__button"));

                }
        }
    }    
}
