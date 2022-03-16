using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedVehicleIntegrationV2
{
    public static class RandomTick
    {
        static Random rng = new Random();
        public static bool NewTick(int chances) // rolls a 1 to input number of chances with an increased randomnes
        { 
            int temp = chances * 100;
            bool res = rng.Next(0, temp) % chances == 0 ? true : false;
            return res;
        }
    }
}
