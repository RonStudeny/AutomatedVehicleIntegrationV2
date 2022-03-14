using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedVehicleIntegrationV2
{
    public class Car
    {
        public double SpeedMs { get; set; }
        public double SpeedKmh { get; set; } // for user only
        public double RouteLength { get; set; }
        public double RouteProgress { get; set; }
        public bool LightState { get; set; }
        public RoadTypes RoadType { get; set; }

        public enum RoadTypes { Normal, Highway, Tunnel, Bridge };

    }
}
