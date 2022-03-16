using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // t.GlobalTick += // subscribe timer
        }

        #region properties
        public Guid CarId { get; set; }
        public double SpeedKmh
        {
            get { return speedKmh; }
            set { speedKmh = SpeedMs * 3.6; }
        }
        public double SpeedMs { get; set; } // for calculation
        public double RouteLength { get; set; }
        public double RouteProgress { get; set; }

        public double RouteProgressPercent
        {
            get { return routeProgressPercent; }
            set { routeProgressPercent = (RouteProgress / RouteLength) * 100; }
        }
        public bool LightState { get; set; }
        public RoadTypes RoadType { get; set; }
        #endregion

        public enum RoadTypes { Normal, Highway, Tunnel, Bridge };

        private void OnTick()
        {
            MoveCar();
        }



        private void MoveCar()
        {
            RouteProgress += RouteProgress < RouteLength ? SpeedMs : 0;
            if (RouteProgress >= RouteLength) CarFinishedEvent(this.CarId);


        }

    }
}
