using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Test.Page
{
    /// <summary>
    /// Class that contains all the element on Ebay's webpage
    /// </summary>
    public class EbayPage
    {
        
        //Attributes of the class
        private readonly DriverManager Manager;

        /// <summary>
        /// Contructor that receives the driver of the whole application
        /// </summary>
        /// <param name="driver">The driver of the test</param>
        public EbayPage(DriverManager driver)
        {
            this.Manager = driver;            
        }

        /// <summary>
        /// Property for getting the main search bar
        /// </summary>
        public IWebElement SearchBar
        {
            get { return this.Manager.Driver.FindElement(By.Id("gh-ac")); }
        }
        

        /// <summary>
        /// Property for getting the searchbutton next to the search bar
        /// </summary>
        public IWebElement SearchButton
        {
            get { return this.Manager.Driver.FindElement(By.Id("gh-btn")); }
        }

        /// <summary>
        /// Property to get the brand filter in the filters' modal
        /// </summary>
        public IWebElement BrandFilter
        {
            get { return this.Manager.Driver.FindElement(By.Id("refineOverlay-mainPanel-Brand"));}
        }

        /// <summary>
        /// Property for getting the size filter in the filter's modal
        /// </summary>
        public IWebElement SizeFilter
        {
            get { return this.Manager.Driver.FindElement
                    (By.Id("refineOverlay-mainPanel-US%20Shoe%20Size%20%28Men%27s%29")); }
        }

        /// <summary>
        /// Property for getting the brand search bar on top of the modal
        /// </summary>
        public IWebElement BrandSearchBar
        {
            get { return this.Manager.Driver.FindElement(By.Id("refineOverlay-subPanel-_x-searchable-0[0]")); }
        }

        /// <summary>
        /// Property for getting the apply filter button on the modal
        /// </summary>
        public IWebElement ApplyFilterButton
        {
            get { return this.Manager.Driver.FindElement(By.CssSelector(".x-overlay-footer__apply > button"));}
        }

        /// <summary>
        /// Property for getting the sort button
        /// </summary>
        public IWebElement SortButton
        {
            get { return this.Manager.Driver.FindElements(By.CssSelector(".x-flyout__button"))[3];}
        }

        /// <summary>
        /// Property for getting a string reflecting all the results found
        /// </summary>
        public String GetNumberOfResults
        {
            get { return this.Manager.Driver.FindElements
                    (By.CssSelector("h1.srp-controls__count-heading > span.BOLD"))[0].Text; }
        }

        /// <summary>
        /// Property to get the price in ascendant order
        /// </summary>
        public IWebElement AscendantPrice
        {
            get { return this.Manager.Driver.FindElements(By.CssSelector(".srp-sort__menu > li"))[3];}
        }

        /// <summary>
        /// Property to get the price in descendant order
        /// </summary>
        public IWebElement DescendantPrice
        {
            get { return this.Manager.Driver.FindElements(By.CssSelector(".srp-sort__menu > li"))[4]; }
        }

        #region LanguageElements
        /// <summary>
        /// Property for getting the language already selected
        /// </summary>
        public IWebElement LanguageDropdown
        {
            get { return this.Manager.Driver.FindElement(By.Id("gh-eb-Geo")); }
        }

        /// <summary>
        /// Property for getting the current displayed language
        /// </summary>
        public String CurrentLanguage
        {
            get { return this.Manager.Driver.FindElement
                    (By.CssSelector("#gh-eb-Geo-a-default > span.gh-eb-Geo-txt")).Text;}
        }

        /// <summary>
        /// Property for getting a the english language in the dropdown
        /// </summary>
        public IWebElement EnglishOption
        {
            get { return this.Manager.Driver.FindElement(By.CssSelector("#gh-eb-Geo-o > ul > li")); }
        }
        #endregion

        /// <summary>
        /// Method that gets the desired number of results
        /// </summary>
        /// <param name="number">The number of results we want to get</param>
        /// <returns>List with number of desired results</returns>
        public List<IWebElement> GetResults(int number)
        {
            //List we will return as result
            List<IWebElement> result = new List<IWebElement>();

            /* Iterating through the number of results we want */
            for (int i = 1; i <= number; i ++)
            {
                /*Since all the results on Ebay have the same id, we just need to
                 * iterate changing the id name with the corresponding number */
                result.Add(this.Manager.Driver.FindElement(By.Id("srp-river-results-listing" + i)));
            }

            return result;
        }

        /// <summary>
        /// Method that gets all the prices for the list of results we have
        /// </summary>
        /// <param name="results">The results we have from Ebay</param>
        /// <returns>The list of prices for those results</returns>
        public List<Decimal> GetPriceOfResults(List<IWebElement> results)
        {
            //List of prices we will return
            List<Decimal> prices = new List<Decimal>();

            //Iterating through all the results we want to get their prices
            foreach (IWebElement aux in results)
            {
                //Getting the raw string where the prices is located
                String RawPriceText = aux.FindElement(By.CssSelector("span.s-item__price")).Text;

                //Getting rid off the currency
                List<String> ValuesOfText = Regex.Split(RawPriceText, @"[^0-9\.,]+")
                                    .Where(a => a != "." && a.Trim() != "").ToList();

                Console.WriteLine("The price is: " + ValuesOfText[0].Trim());

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
        public List<String> GetNameOfResults(List<IWebElement> results)
        {
            //List of prices we will return
            List<String> Name = new List<String>();

            //Iterating through all the results we want to get their prices
            foreach (IWebElement aux in results)
            {
                //Getting the raw string where the prices are located
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
        public List<decimal> GetShippingPriceOfResults(List<IWebElement> list)
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

                    Console.WriteLine("The shipping is: " + ValuesOfText[0].Trim());

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
        public IWebElement SelectChecBoxForSpecificFilter(string filter)
        {
            //Checkbox we will return
            IWebElement CheckBox = null;

            //Helping flag
            bool ElementFound = false;

            //For each option displayed, we will iterate to see if that is the one we want
            foreach(IWebElement aux in this.Manager.Driver.FindElements
                (By.CssSelector("div.x-overlay-sub-panel__col > div")))
            {
                Console.WriteLine("brand name: " + aux.FindElement(By.CssSelector
                    ("label > div > div > span.cbx.x-refine__multi-select-cbx")).Text.Split("(")[0].Trim());

                /* Since the name is attached with an amount and another word, we need to split the string
                 * int order to take only the part of the string we need.
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
        /// Property for getting the "More Filters..." button
        /// </summary>
        public IWebElement MoreFilters
        {
            get
            {
                return this.Manager.Driver.FindElement(By.CssSelector(".x-refine__main__list--more button"));
            }
        }
    }
}
