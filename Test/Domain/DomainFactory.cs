using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain
{
    /// <summary>
    /// Factory class for all the core elements of Ebay
    /// </summary>
    public class DomainFactory
    {
        /// <summary>
        /// Method that instanciates a product with empty data
        /// </summary>
        /// <returns>An empty product</returns>
        public static Product CreateProduct()
        {
            return new Product();
        }

        /// <summary>
        /// Method that instanciates a product with all the data
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <param name="price">The price of the product</param>
        /// <param name="shippingCost">The shipping cost associated to this product</param>
        /// <returns>The product with all its data</returns>
        public static Product CreateProduct(String name, Decimal price, Decimal shippingCost)
        {
            return new Product(name, price, shippingCost);
        }
    }
}
