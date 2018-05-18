using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicSample
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic rFile = new ReadOnlyFile(@"..\..\TextFile1.txt");

            foreach (string line in rFile.List)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine("----------------------------");

            foreach (string line in rFile.And(StringSearchOption.Contains, true))
            {
                Console.WriteLine(line);
            }

            Console.ReadKey();
        }
    }
}
