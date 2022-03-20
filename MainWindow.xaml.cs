﻿using System;
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
        MainTimer MainTimer; // Maintimer for CreateCar
        public MainWindow() {
            InitializeComponent();
            MainTimer mainTimer = new MainTimer();
            MainTimer = mainTimer; // adding mainTimer to MainTimer for CreateCar
            WeatherCenter weatherCenter = new WeatherCenter(mainTimer);
            ControlCenter controlCenter = new ControlCenter();
            controlCenter.Init(mainTimer, ControlCenter.GetCars(3, mainTimer));
            
            MainTimer.GlobalTickEvent += ChangeUItoCar;
            ChangeUItoCar();
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
                //CarListView.ItemsSource = ControlCenter.fullCarList;
                Binding Listviewbinding = new Binding();
                Listviewbinding.Source = ControlCenter.fullCarList;
                CarListView.SetBinding(ListView.ItemsSourceProperty, Listviewbinding);
                CarNamelbl.Content = "Car " + (Carindex + 1);
                Binding Speedbinding = new Binding();
                Speedbinding.Source = ControlCenter.fullCarList[Carindex].SpeedKmh;
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
                CarProgBar.Value = ControlCenter.fullCarList[Carindex].RouteProgressPercent;
		}

		//private void Button_Click(object sender, RoutedEventArgs e) {
  //          ControlCenter.CreateCar(MainTimer);
		//}
	}
}
