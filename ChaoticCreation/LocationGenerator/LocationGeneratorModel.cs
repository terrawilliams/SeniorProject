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
            //Pull location options from database
            LocationQuery_Gen DatabaseToUI = new LocationQuery_Gen();

            var dictionary = DatabaseToUI.Query(DatabaseToUI.QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT * FROM LocationTypesTbl;"));
            foreach (KeyValuePair<string, string> iterate in dictionary)
            {
                locationType.Add(iterate.Key);
            }

            //Pull population size options from database
            dictionary = DatabaseToUI.Query(DatabaseToUI.QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT * FROM LocationSizesTbl;"));
            foreach (KeyValuePair<string, string> iterate in dictionary)
            {
                populationSize.Add(iterate.Key);
            }

            currentLocationType = locationType.First();
            currentPopulationSize = populationSize.First();
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

            
            LocationQuery_Gen generator = new LocationQuery_Gen();
            var generatedLocation = generator.LocationQuery(arguments);
            
            LocationName = generatedLocation["name"];
            LocationDescription = generatedLocation["description"];

            RecentCreations.Instance.AddCreation(LocationName, GeneratorTypesEnum.Location, generatedLocation);            
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
