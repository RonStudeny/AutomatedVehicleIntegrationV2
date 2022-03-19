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
        public MainWindow() {
            InitializeComponent();
            MainTimer mainTimer = new MainTimer();
            WeatherCenter weatherCenter = new WeatherCenter(mainTimer);
            ControlCenter controlCenter = new ControlCenter();
            controlCenter.Init(mainTimer, ControlCenter.GetCars(3, mainTimer));
            Binding Listviewbinding = new Binding();
            Listviewbinding.Source = ControlCenter.fullCarList;
            CarListView.SetBinding(ListView.ItemsSourceProperty, Listviewbinding);
            
        }
        private Car selectedCar;
        private void listView_Click(object sender, RoutedEventArgs e) {
            Random random = new Random();
            var item = (sender as ListView).SelectedItem;
            if(item != null) {
                selectedCar = ControlCenter.fullCarList[(sender as ListView).SelectedIndex];
                MessageBox.Show(selectedCar.CarId.ToString());
            }

        }
        
    }
}
