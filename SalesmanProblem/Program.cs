using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesmanProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            DistanceMatrix DM = new DistanceMatrix();
            Population pop = new Population(DM);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            pop.GenerateRandom(100000);
            const int nGenerations = 100;

            int i=0;
            int min = 9999;

            // iterate
            while(true)
            {
                i++;
                Console.WriteLine("Iteration {0} [{1}]", i, pop.Routes.Count);
                Console.WriteLine("Best {0}", pop.Routes.First());
                pop.CreateNextGeneration();

                if (pop.Routes.Count < 100)
                    foreach (Route route in pop.Routes)
                        Console.WriteLine("     " + route); 

                 //if you like to monitor the improvements
                if (i > 10 && pop.Routes.First().Total() < min)
                {
                    min = pop.Routes.First().Total();
                    //Console.ReadLine();
                }

                if (i >= nGenerations)
                {
                    Console.WriteLine();
                    Console.WriteLine("Finished in {0} msec", watch.ElapsedMilliseconds);
                    break;
                }
            }



            Console.ReadLine();
        }
    }
}
