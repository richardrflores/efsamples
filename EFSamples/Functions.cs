using System.Data.Objects;
using System.Linq;

namespace EFSamples
{
    public class Functions
    {
        public static void GetDaysfromOrders()
        {
            System.Console.WriteLine("Number of Days from Order - Function Example:\n");

            using (var context = new AdventureWorksEntities())
            {
                var days = (from o in context.SalesOrderHeaders
                           where EntityFunctions.DiffDays(o.OrderDate, o.ShipDate) > 3
                           select o).Take(4);

                foreach (var day in days)
                {
                    System.Console.WriteLine("OrderNumber: 000{0}", day.SalesOrderID);
                    System.Console.WriteLine("Took {0} Days", (day.ShipDate - day.OrderDate).Value.Days);
                    System.Console.WriteLine("Was Ordered on {0} and Shipped on {1}", day.OrderDate.Date.ToShortDateString(), day.ShipDate.Value.Date.ToShortDateString());
                    System.Console.WriteLine("===============================================");
                }
            }
        }
    }
}