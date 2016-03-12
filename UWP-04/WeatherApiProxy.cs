using Newtonsoft.Json;
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
    class WeatherApiProxy
    {
        public async static Task<RootObjectApi> GetWeather(string lat, string lon)
        {
            var http = new HttpClient();
            http.Timeout = TimeSpan.FromMilliseconds(15000);

            var url = String.Format("http://weatherap1.azurewebsites.net/?lat={0}&lon={1}", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<RootObjectApi>>(result);
            
            return data[0];
        }
        
        [DataContract]
        public class Forecastlist
        {
            [DataMember]
            public int temp { get; set; }
            [DataMember]
            public string descr { get; set; }
            [DataMember]
            public string icon { get; set; }
        }
        [DataContract]
        public class RootObjectApi
        {
            [DataMember]
            public string city { get; set; }
            [DataMember]
            public string time { get; set; }
            [DataMember]
            public List<Forecastlist> forecastlist { get; set; }
        }
    }
}