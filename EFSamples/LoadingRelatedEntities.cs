using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
                // Load all Products, all related Work Orders, and all related Work Order Routings
                var products = context.Products.Include(x => x.WorkOrders.Select(w => w.WorkOrderRoutings)).ToList();

                foreach (var product in products)
                {
                    // Select Work Order Routings Count for each Work Order
                    // Work Order Routing count is obtained from the products object graph in the conceptual model
                    var worCount = product.WorkOrders.SelectMany(o => o.WorkOrderRoutings).Count();

                    Console.WriteLine("Product Id: {0}", product.ProductID);
                    Console.WriteLine("Product Name: {0}", product.Name);
                    Console.WriteLine("Work Orders: {0}", product.WorkOrders.Count);
                    Console.WriteLine("Work Order Routings: {0}", worCount);
                    Console.WriteLine("--------------------------------");
                }
            }
        }

        public static void ShowLazyLoading()
        {
            Console.WriteLine("Load related entities using Lazy Loading at multiple levels:\n");

            using (var context = new AdventureWorksEntities())
            {
                if (!context.Configuration.LazyLoadingEnabled)
                {
                    Console.WriteLine("LazyLoading is disabled...related entities will not be loaded!");
                    Thread.Sleep(5000);
                }

                // Load all Products
                var products = context.Products.ToList();

                foreach (var product in products)
                {
                    // Select Work Order Routings Count for each Work Order
                    // Work Order Routing count is obtained from the storage model; results in multiple trips to Db
                    var worCount = product.WorkOrders.SelectMany(o => o.WorkOrderRoutings).Count();

                    Console.WriteLine("Product Id: {0}", product.ProductID);
                    Console.WriteLine("Product Name: {0}", product.Name);
                    Console.WriteLine("Work Orders: {0}", product.WorkOrders.Count);
                    Console.WriteLine("Work Order Routings: {0}", worCount);
                    Console.WriteLine("--------------------------------");
                }
                
            }
        }

        //
        // Reminder: Ensure LazyLoading is disabled
        //
        public static void ShowExplicitLoading()
        {
            Console.WriteLine("Load related entities using Explicit Loading:\n");

            using (var context = new AdventureWorksEntities())
            {
                // Load a Product
                var product = context.Products.Find(942);

                // Explicitly load the work orders
                context.Entry(product).Collection(x => x.WorkOrders).Load();

                var worCount = 0;
                foreach (var workOrder in product.WorkOrders)
                {
                    // Explicitly load the work order routings
                    context.Entry(workOrder).Collection(x => x.WorkOrderRoutings).Load();
                    worCount += workOrder.WorkOrderRoutings.Count;
                }

                Console.WriteLine("Product Id: {0}", product.ProductID);
                Console.WriteLine("Product Name: {0}", product.Name);
                Console.WriteLine("Work Orders: {0}", product.WorkOrders.Count);
                Console.WriteLine("Work Order Routings: {0}", worCount);
            }
        }

        //
        // Reminder: Ensure LazyLoading is disabled
        //
        public static void ShowExplicitLoadingWithFilter()
        {
            Console.WriteLine("Load related entities using Explicit Loading with a query filter:\n");

            using (var context = new AdventureWorksEntities())
            {
                // Load a Product
                var product = context.Products.Find(942);

                // Explicitly load the work orders
                context.Entry(product).Collection(x => x.WorkOrders).Query().Load();

                var worCount = 0;
                foreach (var workOrder in product.WorkOrders)
                {
                    // Explicitly load the work order routings
                    context.Entry(workOrder).Collection(x => x.WorkOrderRoutings).Query().Load();
                    worCount += workOrder.WorkOrderRoutings.Count;
                }
                
                Console.WriteLine("Product Id: {0}", product.ProductID);
                Console.WriteLine("Product Name: {0}", product.Name);
                Console.WriteLine("Work Orders: {0}", product.WorkOrders.Count);
                Console.WriteLine("Work Order Routings: {0}", worCount);
                Console.WriteLine("Time elapsed: {0}", _stopwatch.Elapsed);
            }
        }
    }
}