using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesmanProblem
{
    public class Population
    {
        private readonly DistanceMatrix distanceMatrix;
        public List<Route> Routes= new List<Route>();

        public Population(DistanceMatrix distanceMatrix)
        {
            this.distanceMatrix = distanceMatrix;
        }

        public void GenerateRandom(int nRoutes)
        {
            for (int i = 0; i < nRoutes; i++)
                Routes.Add(Route.GenerateRandom(distanceMatrix));
            Routes = Routes.OrderBy(x => x.Total()).ToList();
        }

        /// <summary>
        /// Generate next generation
        /// Order them and only keep the top part
        /// </summary>
        public void CreateNextGeneration()
        {
            List<Route> temp = new List<Route>();
            for (int i = 0; i < 100; i++)
                temp.Add(Routes[Route.random.Next(Routes.Count)]);
            int take = 1000 - temp.Count;
            Routes = Routes.Take(take).ToList();
            Routes.AddRange(temp);

            List<Route> childs = new List<Route>();
            foreach (Route t in Routes) 
            {
                Route tmp = Route.Clone(t);
                tmp.Mutate();
                childs.Add(tmp);
            }

            Routes.AddRange(childs);
            Distinct();
            Routes = Routes.OrderBy(x => x.Total()).ToList();
        }

        /// <summary>
        /// Only keep the unique routes, twins are useless
        /// </summary>
        private void Distinct()
        {
            Routes = Routes.GroupBy(x => x.ToString()).Select(grp => grp.First()).ToList();
        }

    }
}