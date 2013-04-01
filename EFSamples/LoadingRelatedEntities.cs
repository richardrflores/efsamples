using System;
using System.Data.Entity;
using System.Linq;

namespace EFSamples
{
    public class LoadingRelatedEntities
    {
        public static void ShowEagerLoading()
        {
            Console.WriteLine("Load related entities using Eager Loading:\n");

            using (var context = new AdventureWorksEntities())
            {
                var product = context.Products.Include(x => x.ProductInventories).FirstOrDefault(x => x.ProductID == 1);

                Console.WriteLine("Product: {0}\n", product.Name);
                Console.WriteLine("{0, 10} {1, 10} {2, 10} {3, 10} {4, 10}", "ProductID", "LocationID", "Shelf", "Bin", "Quantity");
                Console.WriteLine("--------------------------------------------------------");

                foreach (var item in product.ProductInventories)
                {
                    Console.WriteLine("{0, 10} {1, 10} {2, 10} {3, 10} {4, 10}", item.ProductID, item.LocationID, item.Shelf, item.Bin, item.Quantity);
                }
            }
        }

        public static void ShowEagerLoadingMultipleLevels()
        {
            Console.WriteLine("Load related entities using Eager Loading at multiple levels:\n");

            using (var context = new AdventureWorksEntities())
            {
                var product = context.Products.Include(x => x.WorkOrders.Select(w => w.ScrapReason)).FirstOrDefault(x => x.ProductID == 1);

                Console.WriteLine("Product: {0}\n", product.Name);

                foreach (var item in product.WorkOrders)
                {
                    Console.WriteLine("Srap Id: {0}\nScrap Reason: {1}", item.ScrapReason.ScrapReasonID, item.ScrapReason.Name);
                }
            }
        }
    }
}