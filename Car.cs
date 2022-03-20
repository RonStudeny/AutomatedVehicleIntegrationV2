﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutomatedVehicleIntegrationV2
{
    public class Car {
        private delegate void InternalChangeHandler();
        public event CarUpdateHandler CarFinishedEvent;
        public event CarUpdateHandler CarAccidentEvent;

        private event InternalChangeHandler CarChangedEvent;
        Random rng = new Random();

        #region fields
        private double speedKmh;
        private double routeProgressPercent;
        private int roadChangeCounter;
        #endregion

        public Car(MainTimer t, Guid id, double routeLength, int number, RoadTypes roadType = RoadTypes.Normal ) {
            CarId = id;
            CarNumber = number;
            RouteLength = routeLength;
            RoadType = roadType;
            CarStatus = CarStatusTypes.Operational;
            t.GlobalTickEvent += OnTick; // subscribe timer
            CarChangedEvent += OnChange; // subscribe to internal changes
            WeatherCenter.WeatherUpdateEvent += OnChange;
            CorrectStats();
            roadChangeCounter = defaultRoadChangeChance;
            EnRoute = true;
            BeingTowed = false;
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
        public bool BeingTowed { get; set; }
        public RoadTypes RoadType { get; set; }
        public CarStatusTypes CarStatus { get; set; }
        public int CarNumber {get; set;}
        #endregion

        public enum RoadTypes { Normal, Highway, Tunnel, Bridge };
        public enum CarStatusTypes { Operational, LightAccident, HeavyAccident };

        private const int defaultRoadChangeChance = 50, accidentChance = 1000;

        private void OnTick() // Triggers every global tick, caused by MainTimer class
        {
            if (EnRoute)
            {
                MoveCar();
                RoadChanger();
                AccidentCheck();
            }
        }

        private void OnChange() // Triggers every time there is a change regarding the car
        {
            CorrectStats();
        }

        private void CorrectStats() // speed and light state correction determined by current road type and additionaly weather conditions
        {
            if (CarStatus == CarStatusTypes.Operational && EnRoute)
            {
                switch (this.RoadType)
                {
                    case RoadTypes.Normal:
                        SpeedMs = 50 / 3.6;
                        break;
                    case RoadTypes.Highway:
                        SpeedMs = (130 / 3.6) - (WeatherCenter.RecommendedSlowDown / 2);
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
            else if (CarStatus == CarStatusTypes.LightAccident && EnRoute)
            {
                SpeedMs = RoadType == RoadTypes.Normal ? 50 : 80;
            }

        }

        #region On Tick Functions
        private void RoadChanger() // makes sure the road is changed regularly
        {
            if (RandomTick.NewTick(roadChangeCounter))
            {
                RoadType = (RoadTypes)rng.Next(0, 4);
                roadChangeCounter = defaultRoadChangeChance;
          //      Debug.WriteLine($"Car {CarId} now on {RoadType}");
                CarChangedEvent();
            }
            else roadChangeCounter--;
        }



        private void AccidentCheck() // makes sure that accidents happen... occasionally 
        {
            if (RandomTick.NewTick(accidentChance))
            {
                CarStatus = RandomTick.NewTick(5) ? CarStatusTypes.HeavyAccident : CarStatusTypes.LightAccident;
                EnRoute = false;
                SpeedMs = 0;
                if (CarStatus == CarStatusTypes.LightAccident)
                {
                    RouteLength = Math.Round(RouteLength / 2);
                    CarChangedEvent();
                    EnRoute = true;
                }
                else CarAccidentEvent(this.CarId);
            }
        }

        private void MoveCar()
        {
            RouteProgress += RouteProgress < RouteLength ? SpeedMs : 0;
            Debug.WriteLine($"car {CarId} speed currently {SpeedKmh}");
            if (RouteProgress >= RouteLength)
            {
                EnRoute = false;
                CarFinishedEvent(CarId);
            }
        }
        #endregion
    }
}
