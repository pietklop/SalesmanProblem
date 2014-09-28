using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesmanProblem
{
    public class Route
    {
        private readonly DistanceMatrix distanceMatrix;
        private int nCities { get { return DistanceMatrix.AMOUNT; } }
        public int[] Cities = new int[DistanceMatrix.AMOUNT];
        private int total;

        public static Random random = new Random();

        public Route(DistanceMatrix distanceMatrix)
        {
            this.distanceMatrix = distanceMatrix;
        }

        // Calculate the total distance of the route
        public int Total()
        {
            if (total > 0)
                return total;

            int tot = 0;

            for (int i = 1; i < nCities; i++)
                tot += distanceMatrix.CityToCity[Cities[i], Cities[i - 1]];
            tot += distanceMatrix.CityToCity[Cities[DistanceMatrix.AMOUNT - 1], Cities[0]]; // complete the circle, attach end to start point
            
            total = tot;
            return tot;
        }

        /// <summary>
        /// Mutate the route
        /// </summary>
        public void Mutate()
        {
           // randomly swap two cities
            int a = random.Next(DistanceMatrix.AMOUNT);
            int b = random.Next(DistanceMatrix.AMOUNT);
            int temp = Cities[a];
            Cities[a] = Cities[b];
            Cities[b] = temp;
            if (a == 0 || b == 0)
                OrderRoute();
            total = 0;
        }

        /// <summary>
        /// Order the route in that way that it starts with city=0
        /// </summary>
        private void OrderRoute()
        {
            int offset=0;
            int n = DistanceMatrix.AMOUNT;
            // find city=0
            for (int i = 0; i < n; i++)
            {
                if (Cities[i] == 0)
                {
                    offset = i;
                    break;
                }
            }

            int[] orderedCities = new int[n];
            // put them in order
            for (int i = 0; i < Cities.Count(); i++)
                orderedCities[(n - offset + i) % n] = (Cities[i]);

            Cities = orderedCities;
        }

        public override string ToString()
        {
            return string.Format("Total {0} route:{1}", Total(), string.Join("-", Cities));
        }

        /// <summary>
        /// Generate a random route starts with city=0
        /// </summary>
        /// <param name="distanceMatrix"></param>
        /// <returns></returns>
        public static Route GenerateRandom(DistanceMatrix distanceMatrix)
        {
            Route route = new Route(distanceMatrix);
            route.Cities[0] = 0;
            List<int> r = new List<int>();
            for (int i = 1; i < DistanceMatrix.AMOUNT; i++)
                r.Add(i);

            for (int i = 1; i < DistanceMatrix.AMOUNT; i++)
            {
                int index = random.Next(r.Count);
                route.Cities[i] = r[index];
                r.RemoveAt(index);
            }

            return route;
        }

        public static Route Clone(Route route)
        {
            Route clone = new Route(route.distanceMatrix);
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
                clone.Cities[i] = route.Cities[i];

            return clone;
        }
    }
}