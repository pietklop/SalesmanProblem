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

            int i=0;
            int min = 9999;
            int lastImprovement = 0;


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
                    lastImprovement = i;
                    //Console.ReadLine();
                }

                // stop when it does not improve anymore
                if (i > lastImprovement + 100)
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
