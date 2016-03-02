using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace UWP_04
{
    public class LocationManager
    {
        public async static Task<Geoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();

            var geolocator = new Geolocator { DesiredAccuracyInMeters = 2500 };

            var position = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(30),
            timeout: TimeSpan.FromSeconds(10)
            );

            return position;
        }
    }

}
