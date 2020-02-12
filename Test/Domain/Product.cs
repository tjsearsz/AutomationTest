using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain
{
    /// <summary>
    /// Class that represent the Ebay Product
    /// </summary>
    public class Product
    {
        //Attributes of the class
        private String name;
        private Decimal price;
        private Decimal shippingCost;

        /// <summary>
        /// Empty constructor of the class
        /// </summary>
        public Product()
        {

        }

        /// <summary>
        /// Constructor that receives all the data that a product has
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <param name="price">The price of the product</param>
        /// <param name="shippingCost">The shipping cost asociated to this product</param>
        public Product(String name, Decimal price, Decimal shippingCost)
        {
            this.name = name;
            this.price = price;
            this.shippingCost = shippingCost;
        }

        /// <summary>
        /// Property for the name of the product
        /// </summary>
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Property for the price of the product
        /// </summary>
        public Decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        /// <summary>
        /// Property for the Shipping cost of the product
        /// </summary>
        public Decimal ShippingCost
        {
            get { return this.shippingCost; }
            set { this.shippingCost = value; }
        }

        /// <summary>
        /// Property for the final price (including shipping cost) of the product
        /// </summary>
        public Decimal FinalPrice
        {
            get { return this.price + this.shippingCost; }
        }

        /// <summary>
        /// Override ToString method to print the information of this product
        /// </summary>
        /// <returns>A string representation of this product</returns>
        public override string ToString()
        {
            return String.Format("Name: {0} | Price (Including Shipping): {1}"
                , this.name, this.FinalPrice);
        }
    }
}
