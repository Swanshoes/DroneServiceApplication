using DroneServiceApplication.Models;
using System.Text;
using System.Text.RegularExpressions;
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
        private Queue<Drone> _RegularService = new Queue<Drone>(); // Queue to hold the drone service requests
        private Queue<Drone> _ExpressService = new Queue<Drone>(); // Queue to hold the drone service requests
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
            AddNewItem();
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
            if (_RegularService.Count == 0)
            {
                MessageBox.Show("No drones in the regular service queue.", "Queue Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Drone nextDrone = _RegularService.Dequeue();
            _finishedList.Add(nextDrone);
            RegularQueueLV.Items.RemoveAt(0);
            MessageBox.Show($"Drone with Service Tag {nextDrone.ServiceTag} has been moved to completed services.", "Drone Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            FinishedServicesLB.Items.Add(nextDrone.Display());
        }

        private void DequeueExpressBTN_Click(object sender, RoutedEventArgs e)
        {
            if (_ExpressService.Count == 0)
            {
                MessageBox.Show("No drones in the express service queue.", "Queue Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Drone nextDrone = _ExpressService.Dequeue();
            _finishedList.Add(nextDrone);
            ExpressQueueLV.Items.RemoveAt(0);
            MessageBox.Show($"Drone with Service Tag {nextDrone.ServiceTag} has been moved to completed services.", "Drone Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            FinishedServicesLB.Items.Add(nextDrone.Display());
        }

        // Returns the selected priority from the service priority radio buttons.
        private string GetServicePriority()
        {
            if (RegularServiceRB.IsChecked == true)
            {
                return "Regular";
            }
            else
            {
                return "Express";
            }
        }

        private void AddNewItem()
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

            string servicePriority = GetServicePriority();

            if (servicePriority == "Express")
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
                _RegularService.Enqueue(newDrone);
                RegularQueueLV.Items.Add(newDrone);
            }
            else if (ExpressServiceRB.IsChecked == true)
            {
                _ExpressService.Enqueue(newDrone);
                ExpressQueueLV.Items.Add(newDrone);
            }

            IncrementServiceTag();
            ClearTextBoxes();
        }

        private void MarkCompletedBTN_Click(object sender, RoutedEventArgs e)
        {
            CompleteDroneService();
        }

        private void FinishedServicesLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CompleteDroneService();
        }

        private void RegularQueueLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearTextBoxes();

            ExpressQueueLV.SelectedItem = null; // Deselect any item in the express queue list view

            if (RegularQueueLV.SelectedItem is Drone selectedDrone)
            {
                ClientNameTB.Text = selectedDrone.ClientName;
                DroneModelTB.Text = selectedDrone.DroneModel;
                ServiceProblemTB.Text = selectedDrone.ServiceProblem;
                ServiceCostTB.Text = selectedDrone.ServiceCost.ToString("F2");
            }
        }

        private void ExpressQueueLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearTextBoxes();

            RegularQueueLV.SelectedItem = null; // Deselect any item in the regular queue list view

            if (ExpressQueueLV.SelectedItem is Drone selectedDrone)
            {
                ClientNameTB.Text = selectedDrone.ClientName;
                DroneModelTB.Text = selectedDrone.DroneModel;
                ServiceProblemTB.Text = selectedDrone.ServiceProblem;
                ServiceCostTB.Text = selectedDrone.ServiceCost.ToString("F2");
            }
        }

        private void CompleteDroneService()
        {
            if (_finishedList.Count == 0)
            {
                MessageBox.Show("No completed services to mark.", "No Completed Services", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (FinishedServicesLB.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a completed service to mark.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedIndex = FinishedServicesLB.SelectedIndex;
            Drone selectedDrone = _finishedList[selectedIndex];
            _finishedList.RemoveAt(selectedIndex);
            FinishedServicesLB.Items.RemoveAt(selectedIndex);
            MessageBox.Show($"Drone with Service Tag {selectedDrone.ServiceTag} has been marked as completed.", "Drone Completed", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        //Custom Method to handle only allowing 2 decimal point numbers and nothing else. Works alongside the event handler below.
        private bool IsValidServiceCostInput(string currentText, string newInput)
        {
            string proposedText = currentText + newInput;
            Regex pattern = new Regex(@"^\d*\.?\d{0,2}$");

            return pattern.IsMatch(proposedText);
        }

        private void ServiceCostTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidServiceCostInput(ServiceCostTB.Text, e.Text);
        }
    }
}