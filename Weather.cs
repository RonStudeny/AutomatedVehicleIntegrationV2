using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedVehicleIntegrationV2
{
    public class Weather
    { 
        public int Wind { get; set; }
        public int Temperature { get; set; }
        public bool GoodLightConditions { get; set; }
        public WeatherTypes WeatherType { get; set; }
        public enum WeatherTypes { Sunny, Raining, Storm, Snowing}

    }
}
