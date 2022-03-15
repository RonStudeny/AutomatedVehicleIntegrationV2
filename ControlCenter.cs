using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutomatedVehicleIntegrationV2
{
    public delegate void GlobalTickHandler();

    public class ControlCenter
    {
        public static List<Car> fullCarList;

        public ControlCenter(MainTimer t)
        {
            t.GlobalTick += DebugFunc; // subscribe timer
        }

        private void DebugFunc()
        {
             Debug.WriteLine("Tick heared");
        }
        
    }
}
