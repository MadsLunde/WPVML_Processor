using Raven.Client.Documents;
using System;
using System.IO;
using System.Linq;
using System.Net;

using WPVML_Processor.Services.RavenDB;
using static WPVML_Processor.Services.OpenWeather.Models.WeatherModel;

using Weather = WPVML_Processor.Models.Weather;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WPVML_Processor.Services.OpenWeather
{
    public class Handler
    {
        private static string apiKey = "e4cf29ac45255eae67667884dd4b595c";
        IDocumentStore store = DocumentStoreSingleton.Instance.GetStore("OpenWeather");

        private WeatherObject GetWeatherFromApi(double latitude, double longitude)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            string weather;
            string uri =
                $"http://api.openweathermap.org/data/2.5/find?lat={latitude}&lon={longitude}&cnt=1&units=metric&appid={apiKey}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream apiStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(apiStream))
            {
                weather = reader.ReadToEnd();
            }

            
            var newWeather = DeserializeWeather(weather);
            /*
            using (IDocumentSession documentSession = store.OpenSession())
            {
                documentSession.Store(newWeather);
                documentSession.SaveChanges();
            }
            
            stopwatch.Stop();
            Console.WriteLine("GetWeatherFromAPI " + stopwatch.ElapsedMilliseconds);
            */

            return newWeather;

        }

        public Weather MapWeather(WeatherObject newWeather)
        {
            var wList = newWeather.list.FirstOrDefault();
            return new WPVML_Processor.Models.Weather()
            {
                Cloudiness = wList.clouds.all,
                Humidity = wList.main.humidity,
                Pressure = wList.main.pressure,
                Rain = wList.rain != null,
                Snow = wList.snow != null,
                Temperature = wList.main.temp,
                WindDegrees = wList.wind.deg,
                WindGust = wList.wind.gust,
                WindSpeed = wList.wind.speed
            };
        }

        public Weather GetWeatherForLocation(double latitude, double longitude)
        {
            /*
            using (IDocumentSession documentSession = store.OpenSession())
            {
                documentSession.Load<W>()
            }
            */
            return MapWeather(GetWeatherFromApi(latitude, longitude));
        }


        public WeatherObject DeserializeWeather(string value)
        {
            return JsonConvert.DeserializeObject<WeatherObject>(value);
        }

        private double DistanceBetweenTwoPoints(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3;
            var phi1 = ToRadians(lat1);
            var phi2 = ToRadians(lat2);
            var deltaPhi = ToRadians(lat2 - lat1);
            var deltaLambda = ToRadians(lon2 - lon1);

            var a = Math.Pow(Math.Sin(deltaPhi / 2), 2) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Pow(Math.Sin(deltaLambda / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

    }
}
