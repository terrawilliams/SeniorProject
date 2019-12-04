using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.LocationGenerator
{
    class LocationGeneratorModel : INotifyPropertyChanged
    {
        #region Members
        private List<string> locationType = new List<string>();
        private List<string> populationSize = new List<string>();

        private string currentLocationType;
        private string currentPopulationSize;
        private string locationName;
        private string locationDescription;

        private Random rand = new Random();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public List<string> LocationType
        {
            get { return locationType; }
        }
        public List<string> PopulationSize
        {
            get { return populationSize; }
        }

        public string CurrentLocationType
        {
            get { return currentLocationType; }
            set { currentLocationType = value; }
        }
        public string CurrentPopulationSize
        {
            get { return currentPopulationSize; }
            set { currentPopulationSize = value; }
        }
        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value;
                OnPropertyChanged("LocationName");
            }
        }
        public string LocationDescription
        {
            get { return locationDescription; }
            set
            {
                locationDescription = value;
                OnPropertyChanged("LocationDescription");
            }
        }
        #endregion

        #region Constructor
        public LocationGeneratorModel()
        {
            locationType.Add("Any");
            locationType.Add("Brothel");
            locationType.Add("Castle");
            locationType.Add("Home");
            locationType.Add("Keep");
            locationType.Add("Mansion");
            locationType.Add("Orphanage");
            locationType.Add("Shop");
            locationType.Add("Shrine");
            locationType.Add("Tavern");
            locationType.Add("Temple");
            locationType.Add("Warehouse");

            populationSize.Add("Any");
            populationSize.Add("Tiny");
            populationSize.Add("Small");
            populationSize.Add("Medium");
            populationSize.Add("Large");
            populationSize.Add("Huge");

            currentLocationType = locationType.First();
            currentPopulationSize = populationSize.First();

            locationName = ("Location Name Here");
            locationDescription = ("Location Description Here");
        }
        #endregion

        public void GenerateLocation()
        {
            string type = (currentLocationType.Equals("Any") ? locationType.ElementAt(rand.Next(1, locationType.Count)) : currentLocationType);
            string size = (currentPopulationSize.Equals("Any") ? populationSize.ElementAt(rand.Next(1, populationSize.Count)) : currentPopulationSize);

            LocationName = type;
            LocationDescription = size;

            List<string> arguments = new List<string>();

            arguments.Add(type);
            arguments.Add(size);

            //GenerateLocation(arguments);

            Console.WriteLine("Generate Location Button Pressed");
        }

        public void SaveLocation()
        {
            Console.WriteLine("Save Location Button Pressed");
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
