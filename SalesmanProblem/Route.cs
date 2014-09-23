using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesmanProblem
{
    public class Route
    {
        private readonly DistanceMatrix distanceMatrix;
        private int nCities { get { return DistanceMatrix.AMOUNT; } }
        public City[] Cities = new City[DistanceMatrix.AMOUNT];
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
                tot += distanceMatrix.CityToCity[Cities[i].Nr, Cities[i - 1].Nr];
            tot += distanceMatrix.CityToCity[Cities[DistanceMatrix.AMOUNT - 1].Nr, Cities[0].Nr]; // complete the circle, attach end to start point
            
            total = tot;
            return tot;
        }

        /// <summary>
        /// Generate the connections between linked cities in the route
        /// </summary>
        private void GenerateConnections()
        {
            // check if all cities occur 1 time
            for (int i = 0; i < nCities; i++)
            {
                if (Cities.Count(x => x.Nr == i) != 1)
                    throw new Exception("Invalid route");

                int a = (i == 0 ? nCities - 1 : i - 1);
                int b = (i == nCities - 1 ? 0 : i + 1);
                Cities[i].Connections = new List<int>{Cities[a].Nr, Cities[b].Nr};
            }
        }

        /// <summary>
        /// Mutate the route
        /// </summary>
        public void Mutate()
        {
           // randomly swap two cities
            int a = random.Next(DistanceMatrix.AMOUNT);
            int b = random.Next(DistanceMatrix.AMOUNT);
            int temp = Cities[a].Nr;
            Cities[a] = new City(Cities[b].Nr);
            Cities[b] = new City(temp);
            total = 0;
        }

        public override string ToString()
        {
            return string.Format("Total {0} route:{1}", Total(), string.Join("-", Cities.Select(x => x.Nr)));
        }

        /// <summary>
        /// Generate a random route
        /// </summary>
        /// <param name="distanceMatrix"></param>
        /// <returns></returns>
        public static Route GenerateRandom(DistanceMatrix distanceMatrix)
        {
            Route route = new Route(distanceMatrix);
            List<int> r = new List<int>();
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
                r.Add(i);

            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
                int index = random.Next(r.Count);
                route.Cities[i] = new City(r[index]);
                r.RemoveAt(index);
            }

            route.GenerateConnections();

            return route;
        }

        public static Route Clone(Route route)
        {
            Route clone = new Route(route.distanceMatrix);
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
                clone.Cities[i] = new City(route.Cities[i].Nr);

            return clone;
        }
    }
}