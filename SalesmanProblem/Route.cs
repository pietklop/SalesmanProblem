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

        public void Validate()
        {
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
                if (Cities.Count(x => x.Nr == i) != 1)
                    throw new Exception("Invalid route"); 
            }
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
        /// Goal is to create childs with identical direct city connections from both parents and
        /// see if it does bring anything.
        /// </summary>
        /// <param name="extRoute"></param>
        /// <returns></returns>
        public List<Route> CrossOver(Route extRoute)
        {
            // at start, childs are clones of the parents
            Route child1 = Clone(this);
            Route child2 = Clone(extRoute);
            bool crossOver = false;

            // find matching connections
            // put matching connection in both of the childs
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
               int connectedCity = Cities[i].EqualConnection(extRoute.Cities.First(x => x.Nr == Cities[i].Nr)); 
               if (connectedCity >= 0)
               {
                   crossOver = true;
                   child1.SetCrossOverCity(i, Cities[i].Nr);
                   child1.SetCrossOverCity(i + 1, connectedCity);

                   int index = extRoute.IndexOfCity(Cities[i].Nr);
                   child2.SetCrossOverCity(index, Cities[i].Nr);
                   child2.SetCrossOverCity(index + 1, connectedCity);
               }
            }

            if (crossOver)
            {
                child1.FixRoute();
                child2.FixRoute();
            }
            else
            {   // no crossover, let at least mutate then
                child1.Mutate();
                child2.Mutate();
            }

            return new List<Route>{child1, child2, this, extRoute}; // keep the parents in the population
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

        /// <summary>
        /// Crossovers are messing up the routes, fix them here
        /// </summary>
        public void FixRoute()
        {
            // count cities
            int[] freq = new int[DistanceMatrix.AMOUNT];
            bool[] cross = new bool[DistanceMatrix.AMOUNT]; // if the same city is crossed multiple times, then we still can allow only one
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
                freq[Cities[i].Nr] += 1;

            // remove duplicate cities
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
                if (freq[Cities[i].Nr] > 1 && (!Cities[i].CrossOver || cross[Cities[i].Nr]))
                    Cities[i] = null;
                else if (Cities[i].CrossOver) // possibly not optimal in case of more crossovers
                    cross[Cities[i].Nr] = true;
            }

            // add missing
            List<int> toAdd = new List<int>();
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
                if (freq[i] == 0)
                    toAdd.Add(i);
            }

            Random random = new Random();

            while (toAdd.Count > 0)
            {
                int index = random.Next(toAdd.Count);
                int c=0;
                for (int j = 0; j < DistanceMatrix.AMOUNT; j++)
                {
                    if (Cities[j] == null)
                    {
                        c = j;
                        break;
                    }
                }
                Cities[c] = new City(toAdd[index]);
                toAdd.RemoveAt(index);
            }

//            Validate(); // should not be necessary
            total = 0;
        }

        public void SetCrossOverCity(int index, int city)
        {
            if (index == DistanceMatrix.AMOUNT)
                index = 0;
            if (index == -1)
                index = DistanceMatrix.AMOUNT -1;

            Cities[index] = new City(city);
            Cities[index].CrossOver = true;
        }

        public int IndexOfCity(int nr)
        {
            for (int i = 0; i < DistanceMatrix.AMOUNT; i++)
            {
                if (Cities[i].Nr == nr)
                    return i;
            }
            return -1;
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

            //            route.Validate(); // Only for checking, shouldn't be necessary

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