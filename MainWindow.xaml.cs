using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutomatedVehicleIntegrationV2
{
    public partial class MainWindow : Window {
        Random rnd = new Random();
        public MainWindow() {
            InitializeComponent();
            MainTimer mainTimer = new MainTimer();
            WeatherCenter weatherCenter = new WeatherCenter();
            ControlCenter controlCenter = new ControlCenter();
            controlCenter.Init(ControlCenter.GetCars(5));
            MainTimer.GlobalTickEvent += ChangeUItoCar;
            ChangeUItoCar();
            selectedCar = ControlCenter.fullCarList[0];
        }
        private Car selectedCar;
        private void listView_Click(object sender, RoutedEventArgs e) {
            Random random = new Random();
            var item = (sender as ListView).SelectedItem;
            if(item != null) {
                selectedCar = ControlCenter.fullCarList[(sender as ListView).SelectedIndex];
            }
            ChangeUItoCar();
        }
        public void ChangeUItoCar() {
            int Carindex = 0;
            if(selectedCar != null) {
                Carindex = selectedCar.CarNumber;
            }
            #region UI
            CarListView.ItemsSource = ControlCenter.fullCarList;
			    Binding Listviewbinding = new Binding();
			    Listviewbinding.Source = ControlCenter.fullCarList;
			    CarListView.SetBinding(ListView.ItemsSourceProperty, Listviewbinding);
			    CarNamelbl.Content = "Car " + (Carindex + 1);
                Binding Speedbinding = new Binding();
                Speedbinding.Source = ControlCenter.fullCarList[Carindex].SpeedKmh + " km/h";
                SpeedTxBlk.SetBinding(TextBlock.TextProperty, Speedbinding);
                Binding Statusbinding = new Binding();
                Statusbinding.Source = ControlCenter.fullCarList[Carindex].CarStatus;
                StatusTxBlk.SetBinding(TextBlock.TextProperty, Statusbinding);
                Binding Roadbinding = new Binding();
                Roadbinding.Source = ControlCenter.fullCarList[Carindex].RoadType;
                RoadTypeTxBlk.SetBinding(TextBlock.TextProperty, Roadbinding);
                Binding Lightbinding = new Binding();
                Lightbinding.Source = ControlCenter.fullCarList[Carindex].LightState;
                LightsTxBlk.SetBinding(TextBlock.TextProperty, Lightbinding);
			    Binding WeatherBinding = new Binding();
			    WeatherBinding.Source = WeatherCenter.currentWeather.WeatherType.ToString();
			    WeatherTxBlk.SetBinding(TextBlock.TextProperty, WeatherBinding);
                RouteLenghtTxBlk.Text = Math.Round(ControlCenter.fullCarList[Carindex].RouteLength/1000, 2).ToString() + " km";
                Progresslbl.Content = Math.Round(ControlCenter.fullCarList[Carindex].RouteProgressPercent).ToString() + " / 100";
                CarProgBar.Value = ControlCenter.fullCarList[Carindex].RouteProgressPercent;
            #endregion
        }

        private void CreateNewRoute_Click(object sender, RoutedEventArgs e) {
			if(selectedCar.EnRoute == false) {
                selectedCar.RouteLength = rnd.Next(1, 10) * 1000;
                selectedCar.RouteProgress = 0;
                selectedCar.EnRoute = true;
			}
		}

		//private void Button_Click(object sender, RoutedEventArgs e) {
		//          ControlCenter.CreateCar(MainTimer);
		//}
	}
}
