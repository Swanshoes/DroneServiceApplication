using System.Globalization;

namespace DroneServiceApplication.Models
{
    // 6.1 Drone class file
    // Holds the data items for a drone service request.
    // Attributes are private and accessor methods are public.
    public class Drone
    {
        private string? clientName;
        private string? droneModel;
        private string? serviceProblem;
        private decimal serviceCost;
        private int serviceTag;

        public string ClientName
        {
            get
            {
                return clientName;
            }
            set
            {
                clientName = ToTitleCase(value);
            }
        }

        public string DroneModel
        {
            get
            {
                return droneModel;
            }
            set
            {
                droneModel = value;
            }
        }

        public string ServiceProblem
        {
            get
            {
                return serviceProblem;
            }
            set
            {
                serviceProblem = ToSentenceCase(value);
            }
        }

        public decimal ServiceCost
        {
            get
            {
                return serviceCost;
            }
            set
            {
                serviceCost = value;
            }
        }

        public string ServiceCostDisplay
        {
            get
            {
                return serviceCost.ToString("C2");
            }
        }

        public int ServiceTag
        {
            get
            {
                return serviceTag;
            }
            set
            {
                serviceTag = value;
            }
        }

        public Drone()
        {
        }

        public Drone(string clientName, string droneModel, string serviceProblem, decimal serviceCost, int serviceTag)
        {
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
        }

        // Returns the Client Name and Service Cost for the finished service ListBox.
        public string Display()
        {
            return $"{ClientName} - ${ServiceCost:F2}";
        }

        private string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            input = input.Trim().ToLower();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input);
        }

        private string ToSentenceCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            input = input.Trim().ToLower();

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}