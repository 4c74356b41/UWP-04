using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UWP_04
{
    class OpenWeatherMapProxyForecast
    {
        public async static Task<RootObjectForecast> GetWeatherForecast(double lat, double lon)
        {
            var http = new HttpClient();
            // Wow, my API key in plain text! Don't hack me please!
            var url = String.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid=5cac47538f90d19879ecaa7c8c7fab67&units=metric", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObjectForecast));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObjectForecast)serializer.ReadObject(ms);

            return data;
        }

        [DataContract]
        public class CoordForecast
        {
            [DataMember]
            public double lon { get; set; }
            [DataMember]
            public double lat { get; set; }
        }
        [DataContract]
        public class SysForecast
        {
            [DataMember]
            public int population { get; set; }
        }
        [DataContract]
        public class CityForecast
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public CoordForecast coord { get; set; }
            [DataMember]
            public string country { get; set; }
            [DataMember]
            public int population { get; set; }
            [DataMember]
            public SysForecast sys { get; set; }
        }
        [DataContract]
        public class MainForecast
        {
            [DataMember]
            public double temp { get; set; }
            [DataMember]
            public double temp_min { get; set; }
            [DataMember]
            public double temp_max { get; set; }
            [DataMember]
            public double pressure { get; set; }
            [DataMember]
            public double sea_level { get; set; }
            [DataMember]
            public double grnd_level { get; set; }
            [DataMember]
            public int humidity { get; set; }
            [DataMember]
            public double temp_kf { get; set; }
        }
        [DataContract]
        public class WeatherForecast
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public string main { get; set; }
            [DataMember]
            public string description { get; set; }
            [DataMember]
            public string icon { get; set; }
        }
        [DataContract]
        public class ListForecast
        {
            [DataMember]
            public MainForecast main { get; set; }
            [DataMember]
            public List<WeatherForecast> weather { get; set; }
            [DataMember]
            public string dt_txt { get; set; }
        }
        [DataContract]
        public class RootObjectForecast
        {
            [DataMember]
            public CityForecast city { get; set; }
            [DataMember]
            public List<ListForecast> list { get; set; }
        }
    }
}
