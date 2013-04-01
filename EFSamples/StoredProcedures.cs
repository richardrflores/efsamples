using System;
using System.Linq;

namespace EFSamples
{
    public static class StoredProcedures
    {
         public static void GetEmployeeManagers()
         {
             Console.WriteLine("Employee Managers using Stored Procedures:\n");

             using (var context = new AdventureWorksEntities())
             {
                 var entityId = 6;
                 var managerResults = context.uspGetEmployeeManagers(entityId).ToList();
                 var firstEmployee = managerResults.ElementAt(0);

                 Console.WriteLine("Managers for {0} {1}\n", firstEmployee.FirstName, firstEmployee.LastName);

                 foreach (var result in managerResults)
                 {
                     Console.WriteLine("Manager Name: {0} {1}", result.ManagerFirstName, result.ManagerLastName);
                 }
             }

         }   
    }
}