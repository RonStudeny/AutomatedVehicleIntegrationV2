using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutomatedVehicleIntegrationV2
{
    #region delegates
    public delegate void GlobalTickHandler();
    public delegate void CarUpdateHandler(Guid carId);
    public delegate void CarAccidentHandler(Car.CarStatusTypes accidentType, Guid carID);
    #endregion

    public class ControlCenter
    {
        public static List<Car> fullCarList;
        public ControlCenter(MainTimer t, List<Car> cars)
        {
            fullCarList = cars;
            t.GlobalTickEvent += OnTick; // subscribe timer
            foreach (var c in fullCarList) // subscribe to car events
            {
                c.CarFinishedEvent += OnCarFinished;
                c.CarAccidentEvent += OnCarAccident;
            }
        }

        private void OnTick()
        {
            Debug.WriteLine(WeatherCenter.RecommendedSlowDown);
        }

        private void OnCarFinished(Guid carID)
        {
            Debug.WriteLine("Caught car finished event");
        }

        private void OnCarAccident(Car.CarStatusTypes accidentType, Guid carId) // triggers
        {

        }

        public static List<Car> GetCars(int numOfCars, MainTimer t) // WIP - creates a desired ammount of car instances
        {
            List<Car> res = new List<Car>();

            for (int i = 0; i < numOfCars; i++)
            {
                Car newCar = new Car(t, new Guid(), 3500);
                res.Add(newCar);
            }
            return res;
        }

    }
}
