using DroneServiceApplication.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DroneServiceApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currentServiceTag = 100; // Starting point for service tags
        private Queue<Drone> _regularDroneQueue = new Queue<Drone>(); // Queue to hold the drone service requests
        private Queue<Drone> _expressDroneQueue = new Queue<Drone>(); // Queue to hold the drone service requests
        private List<Drone> _finishedList = new List<Drone>(); // List to hold drones that are awaiting pickup
        public MainWindow()
        {
            InitializeComponent();
        }

        private void IncrementServiceTag()
        {
            if (_currentServiceTag < 900)
            {
                _currentServiceTag += 10;
            }
            else
            {
                _currentServiceTag = 100;
            }
        }

        private void AddToQueueBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ClientNameTB.Text == "" || DroneModelTB.Text == "" || ServiceProblemTB.Text == "" || ServiceCostTB.Text == "")
            {
                MessageBox.Show("Please fill in all fields before adding to the queue.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(ServiceCostTB.Text, out decimal serviceCost))
            {
                MessageBox.Show("Please enter a valid service cost.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ExpressServiceRB.IsChecked == true)
            {
                serviceCost = serviceCost * 1.15m;
            }

            Drone newDrone = new Drone
            {
                ClientName = ClientNameTB.Text,
                DroneModel = DroneModelTB.Text,
                ServiceProblem = ServiceProblemTB.Text,
                ServiceCost = serviceCost,
                ServiceTag = _currentServiceTag
            };

            if (RegularServiceRB.IsChecked == true)
            {
                _regularDroneQueue.Enqueue(newDrone);
                RegularQueueLV.Items.Add(newDrone);
            }
            else if (ExpressServiceRB.IsChecked == true)
            {
                _expressDroneQueue.Enqueue(newDrone);
                ExpressQueueLV.Items.Add(newDrone);
            }

            IncrementServiceTag();
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            ClientNameTB.Clear();
            DroneModelTB.Clear();
            ServiceProblemTB.Clear();
            ServiceCostTB.Clear();
        }

        private void ClearInputsBTN_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
        }

        private void DequeueRegularBTN_Click(object sender, RoutedEventArgs e)
        {
            if (_regularDroneQueue.Count == 0)
            {
                MessageBox.Show("No drones in the regular service queue.", "Queue Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Drone nextDrone = _regularDroneQueue.Dequeue();
            _finishedList.Add(nextDrone);
            RegularQueueLV.Items.RemoveAt(0);
            MessageBox.Show($"Drone with Service Tag {nextDrone.ServiceTag} has been moved to completed services.", "Drone Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            FinishedServicesLB.Items.Add(nextDrone.Display());
        }

        private void DequeueExpressBTN_Click(object sender, RoutedEventArgs e)
        {
            if (_expressDroneQueue.Count == 0)
            {
                MessageBox.Show("No drones in the express service queue.", "Queue Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Drone nextDrone = _expressDroneQueue.Dequeue();
            _finishedList.Add(nextDrone);
            ExpressQueueLV.Items.RemoveAt(0);
            MessageBox.Show($"Drone with Service Tag {nextDrone.ServiceTag} has been moved to completed services.", "Drone Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            FinishedServicesLB.Items.Add(nextDrone.Display());
        }
    }
}