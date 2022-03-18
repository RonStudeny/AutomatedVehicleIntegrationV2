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
        static Random rng = new Random();
        public static List<Car> fullCarList; // keep all active instances of Car class here for easy access 
        public void Init(MainTimer t, List<Car> cars)
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
           // Debug.WriteLine(WeatherCenter.RecommendedSlowDown);
        }

        private void OnCarFinished(Guid carID) // triggers when the given car has finished it's assigned route
        {
            Debug.WriteLine($"car {carID} has finished");
        }

        private void OnCarAccident(Car.CarStatusTypes accidentType, Guid carId) // triggers when a car crashes
        {
            Debug.WriteLine($" car {carId} has crashed, status: {accidentType}");
        }

        public static List<Car> GetCars(int numOfCars, MainTimer t) // creates a desired ammount of car instances, configure the distance range in the .Next() function (km)
        {
            List<Car> res = new List<Car>();

            for (int i = 0; i < numOfCars; i++)
            {
                Car newCar = new Car(t, Guid.NewGuid(), rng.Next(10, 150) * 1000); // car id and route lenght is generated here
                res.Add(newCar);
            }
            return res;
        }

    }
}
