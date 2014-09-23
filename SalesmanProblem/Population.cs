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
            if (take%2 == 1) take++; // so sad if one can not make any childs, add one
            Routes = Routes.Take(take).ToList();
            Routes.AddRange(temp);
            Mix();
            List<Route> childs = new List<Route>();
            for (int i = 0; i < Routes.Count; i += 2)
                childs.AddRange(Routes[i].CrossOver(Routes[i + 1]));
            
            Routes = childs;
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

        /// <summary>
        /// Mix the list, to prevent inbreed :)
        /// </summary>
        private void Mix()
        {
            List<Route> temp = new List<Route>();
            Random random = new Random();
            int n = Routes.Count;
            for (int i = 0; i < n; i++)
            {
                int index = random.Next(Routes.Count);
                temp.Add(Routes[index]);
                Routes.RemoveAt(index);
            }
            Routes = temp;
        }

    }
}