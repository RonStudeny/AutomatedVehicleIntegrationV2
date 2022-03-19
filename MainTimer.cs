using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedVehicleIntegrationV2
{

    public class MainTimer
    {
        public event GlobalTickHandler GlobalTickEvent;

        private const int tickInterval = 1000; // ms

        public MainTimer() => StartTicking();

        private async void StartTicking()
        {
            while (true)
            {
                await Task.Delay(tickInterval);
                GlobalTickEvent();
            }

        }

    }
}
 