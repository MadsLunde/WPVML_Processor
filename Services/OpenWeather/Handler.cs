using System;
using System.IO;
using System.Linq;
using System.Net;
using static WPVML_Processor.Services.OpenWeather.Models.WeatherModel;
using Weather = WPVML_Processor.Models.Weather;
using Newtonsoft.Json;

namespace WPVML_Processor.Services.OpenWeather
{
    public class Handler
    {
        private static string apiKey = "e4cf29ac45255eae67667884dd4b595c";

        public Weather MapWeather(WeatherObject newWeather)
        {
            var wList = newWeather.list.FirstOrDefault();
            return new Weather()
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
            var weatherFromApi = GetWeatherFromApi(latitude, longitude);
            if (weatherFromApi != null)
            {
                return MapWeather(weatherFromApi);    
            }
            else
            {
                return null;
            }
            
        }

        public WeatherObject DeserializeWeather(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<WeatherObject>(value);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        private WeatherObject GetWeatherFromApi(double latitude, double longitude)
        {
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
            return newWeather;

        }

    }
}
