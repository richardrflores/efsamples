using System;
using System.Collections.Generic;
using System.Linq;

namespace EFSamples
{
    public static class GettingStarted
    {
        public static IEnumerable<Product> Products { get; set; }

        public static void ShowProducts()
        {
            Console.WriteLine("Adventure Works Products:\n");
            using (var ctx = new AdventureWorksEntities())
            {
                var products = ctx.Products.Take(4);

                foreach (var product in products)
                {
                    Console.WriteLine("ID: {0}", product.ProductID);
                    Console.WriteLine("Name: {0}", product.Name);
                    Console.WriteLine("Number: {0}", product.ProductNumber);
                    Console.WriteLine("Price: {0}", product.ListPrice);
                    Console.WriteLine("----------------------------------");
                }
            }
            Console.ReadKey();
        }
    }
}
