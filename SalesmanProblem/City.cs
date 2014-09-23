using System.Collections.Generic;
using System.Linq;

namespace SalesmanProblem
{
    public class City
    {
        public readonly int Nr;
        public List<int> Connections = new List<int>();
        public bool CrossOver;

        public City(int nr)
        {
            Nr = nr; 
        }
        public City(int nr, int connection1, int connection2) : this(nr)
        {
            Connections.Add(connection1);
            Connections.Add(connection2);
        }

        public int EqualConnection(City city)
        {
            if (Nr != city.Nr)
                return -1;
            foreach (int connection in city.Connections)
            {
                if (ContainsConnection(connection)) 
                    return connection;
            }
            return -1;
        }

        private bool ContainsConnection(int toCity)
        {
            return Connections.Any(c => c == toCity);
        }
    }
}