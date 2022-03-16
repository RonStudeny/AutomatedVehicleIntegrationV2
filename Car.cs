﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutomatedVehicleIntegrationV2
{
    public class Car
    {
        private delegate void InternalChangeHandler();
        public event CarUpdateHandler CarFinishedEvent;
        private event InternalChangeHandler CarChangedEvent;

        #region fields
        private double speedKmh;
        private double routeProgressPercent;
        #endregion

        public Car(MainTimer t, Guid id, double routeLength, RoadTypes roadType = RoadTypes.Normal)
        {
            CarId = id;
            RouteLength = routeLength;
            RoadType = roadType;
            t.GlobalTickEvent += OnTick; // subscribe timer
            CarChangedEvent += OnChange; // subscribe to internal changes
            EnRoute = true;
        }

        #region properties
        public Guid CarId { get; set; }
        public double SpeedKmh { get { return SpeedMs * 3.6; } } // get only
        public double SpeedMs { get; set; } // for calculation
        public double RouteLength { get; set; }
        public double RouteProgress { get; set; }
        public double RouteProgressPercent { get { return (RouteProgress / RouteLength) * 100; } } // get only
        public bool LightState { get; set; }
        public bool EnRoute { get; set; }
        public RoadTypes RoadType { get; set; }
        #endregion

        public enum RoadTypes { Normal, Highway, Tunnel, Bridge };

        private void OnTick() // Triggers every global tick, caused by MainTimer class
        {
            MoveCar();
            RoadChanger();
            AccidentCheck();
        }

        private void OnChange() // Triggers every time there is an internal change in the car
        {
            CorrectStats();
        }

        private void CorrectStats() // speed and light state correction determined by current road type and additionaly weather conditions
        {
            switch (RoadType)
            {
                case RoadTypes.Normal:
                    SpeedMs = 50 / 3.6;
                    break;
                case RoadTypes.Highway:
                    SpeedMs = 130 / 3.6;
                    break;
                case RoadTypes.Tunnel:
                    SpeedMs = 110 / 3.6;
                    break;
                case RoadTypes.Bridge:
                    SpeedMs = (130 / 3.6) - WeatherCenter.RecommendedSlowDown;
                    break;
            }
            LightState = WeatherCenter.currentWeather.GoodLightConditions == false || RoadType == RoadTypes.Tunnel ? true : false;
        }



        private void RoadChanger() // makes sure the road is changed regularly
        {
            CarChangedEvent?.Invoke();
        }

        private void AccidentCheck() // makes sure that accidents happen... occasionally 
        {
            CarChangedEvent?.Invoke();
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
