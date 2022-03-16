using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutomatedVehicleIntegrationV2
{
    public class Car
    {
        public event CarUpdateHandler CarFinishedEvent;

        #region fields
        private double speedKmh;
        private double routeProgressPercent;
        #endregion

        public Car(MainTimer t, Guid id, double routeLength, RoadTypes roadType = RoadTypes.Normal)
        {
            CarId = id;
            RouteLength = routeLength;
            RoadType = roadType;
            t.GlobalTick += OnTick; // subscribe timer
            EnRoute = true;
        }

        #region properties
        public Guid CarId { get; set; }
        public double SpeedKmh
        {
            get { return SpeedMs * 3.6; }
            set { speedKmh = value; }
        }
        public double SpeedMs { get; set; } // for calculation
        public double RouteLength { get; set; }
        public double RouteProgress { get; set; }

        public double RouteProgressPercent
        {
            get { return (RouteProgress / RouteLength) * 100;}
            set { routeProgressPercent = value; }
        }
        public bool LightState { get; set; }
        public bool EnRoute { get; set; }
        public RoadTypes RoadType { get; set; }
        #endregion

        public enum RoadTypes { Normal, Highway, Tunnel, Bridge };

        private void OnTick()
        {
            CorrectSpeed();
            MoveCar();
        }

        private void CorrectSpeed()
        {
            SpeedMs = 130;
        }


        private void MoveCar()
        {
            if (EnRoute)
            {
                RouteProgress += RouteProgress < RouteLength ? SpeedMs : 0;
                Debug.WriteLine(RouteProgressPercent);
                if (RouteProgress >= RouteLength)
                {
                    EnRoute = false;
                    CarFinishedEvent(CarId);
                }
            }
        }

    }
}
