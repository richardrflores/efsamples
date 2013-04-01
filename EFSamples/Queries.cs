using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;

namespace EFSamples
{
    public static class Queries
    {
        public static void FindEntityUsingPk()
        {
            System.Console.WriteLine("\nFinding Identity with PK");

            using (var context = new AdventureWorksEntities())
            {
                var order = context.SalesOrderHeaders.Find(43661);

                System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate);
            }
        }

        public static void RunLinqExample1()
        {
            System.Console.WriteLine("\nUsing LINQ");

            using (var context = new AdventureWorksEntities())
            {
                var order = (from o in context.SalesOrderHeaders
                            where o.SalesOrderID == 43661
                            select o).FirstOrDefault();
                
                System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate);
            }
        }

        public static void RunLinqExample2()
        {
            System.Console.WriteLine("\nUsing LINQ Expression");

            using (var context = new AdventureWorksEntities())
            {
                var order = context.SalesOrderHeaders.FirstOrDefault(x => x.SalesOrderID == 43661);

                System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate);
            }
        }

        public static void RunRawSQL()
        {
            System.Console.WriteLine("\nUsing Raw SQL");

            using (var context = new AdventureWorksEntities())
            {
                var order = context.SalesOrderHeaders.SqlQuery("SELECT * FROM Sales.SalesOrderHeader").FirstOrDefault(x => x.SalesOrderID == 43661);

                System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate);
            }
        }

        public static void RunESQLExample()
        {
            System.Console.WriteLine("\nUsing Entity SQL");

            var esqlQuery = @"SELECT order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate FROM AdventureWorksEntities.SalesOrderHeaders AS order where order.SalesOrderID = 43661";

            using (var conn = new EntityConnection("name=AdventureWorksEntities"))
            {
                conn.Open();

                // Create an EntityCommand.
                using (EntityCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = esqlQuery;

                    // Execute the command.
                    using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        // Start reading results.
                        while (rdr.Read())
                        {
                            System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", rdr[0], rdr[1], rdr[2], rdr[3]);
                        }
                    }
                }
                conn.Close();
            }
        }

        /*
         * Use ObjectQuery(T) Class or ObjectSet<T> on the object context to access data using Entity SQL Builder methods. 
         * Both methods requires an instance of type ObjectContext. As a result, Entity SQL Builder methods are not supported
         * on objects of type DbContext.
         */
        
        public static void RunESQLBuilderExample()
        {
            using (var context = new AdventureWorksEntities())
            {
                System.Console.WriteLine("\nUsing Entity SQL Builder");

                if (context.GetType() == typeof (ObjectContext))
                {
                    // An example of ESQL Builder using an ObjectSet<T> on the ObjectContext
                    // var order = context.CreateObjectSet<SalesOrderHeader>("SalesOrderHeader").Where("it.SalesOrderID = 43661");
                    // System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}", order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate); 

                    var objContext = new ObjectContext("name=AdventureWorksEntities");
                    var queryString =
                        @"SELECT VALUE salesOrders FROM AdventureWorksEntities.SalesOrderHeader AS salesOrders";
                    var salesQuery1 = new ObjectQuery<SalesOrderHeader>(queryString, objContext, MergeOption.NoTracking);
                    var salesQuery2 = salesQuery1.Where("it.SalesOrderID = 43661");

                    foreach (var order in salesQuery2)
                    {
                        System.Console.WriteLine("\nSalesOrderID: {0} \nOrderDate: {1} \nDueDate: {2} \nShipDate: {3}",
                                                 order.SalesOrderID, order.OrderDate, order.DueDate, order.ShipDate);
                    }
                }
                else
                {
                    System.Console.WriteLine("\nSorry! Context is not of type ObjectContext. ESQL Builder is not supported.");
                }

            }
        }
    }
}