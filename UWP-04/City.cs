using System;
using System.Collections.Generic;
using System.Linq;

namespace UWP_04
{
    public class City
    {
        public string CityName { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", CityName);
        }
    }

    public static class CitiesSampleSource
    {
        private static List<City> _cities = new List<City>()
        {
            new City(){CityName="Barcelona"},
            new City(){CityName="Irakleion"},
            new City(){CityName="Moscow"},
            new City(){CityName="Toronto"},
            new City(){CityName="Vienna"}
        }.OrderBy(c => c.CityName).ToList();
        public static List<City> City
        {
            get { return _cities; }
        }

        public static IEnumerable<City> GetMatchingCities(string query)
        {
            return City
                .Where(c => c.CityName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.CityName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
