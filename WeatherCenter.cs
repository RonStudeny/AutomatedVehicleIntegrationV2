using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedVehicleIntegrationV2
{
    public class WeatherCenter
    {
        Random rng = new Random();
        public static Weather currentWeather;

        private const int chances = 100;

        public WeatherCenter(MainTimer t)
        {
            t.GlobalTick += OnTick; // subscribe timer
        }
        
        private void OnTick()
        {

        }

        private Weather GetWeather()
        {
            Weather res = new Weather();

            res.Wind = rng.Next(5, 40);
            res.WeatherType = (Weather.WeatherTypes)rng.Next(0, 4);
            res.Temperature = res.WeatherType == Weather.WeatherTypes.Snowing ? rng.Next(-10, 3) : rng.Next(5, 35);
            res.GoodLightConditions = res.WeatherType == Weather.WeatherTypes.Sunny ? true : false;
            return res;
        }
    }
}
