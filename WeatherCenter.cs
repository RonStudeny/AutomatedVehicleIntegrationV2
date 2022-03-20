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
        public static event WeatherChangeHandler WeatherUpdateEvent;
        public WeatherCenter(MainTimer t)
        {
            t.GlobalTickEvent += OnTick; // subscribe timer
            currentWeather = GetWeather();
        }

        public static double RecommendedSlowDown // the speed (m/s) by which the car should slow down if on a bridge, determined by current weather conditions
        {
            get
            {
                double temp =
                    currentWeather.WeatherType == Weather.WeatherTypes.Sunny ? 0 :
                    currentWeather.WeatherType == Weather.WeatherTypes.Snowing ? 35 : 20;
                temp += currentWeather.Wind / 10;
                return Math.Round(temp / 3.6, 1);
            }
        }
        private void OnTick()
        {
            currentWeather = RandomTick.NewTick(8) == true ? GetWeather() : currentWeather;
            WeatherUpdateEvent();
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
