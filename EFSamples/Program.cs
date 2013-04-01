using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //GettingStarted.ShowProducts();
            Queries.FindEntityUsingPK();
            Queries.RunLinqExample1();
            Queries.RunLinqExample2();
            Queries.RunRawSQL();
            Queries.RunESQLExample();
            Queries.RunESQLBuilderExample();

            Console.ReadKey();
        }
    }
}
