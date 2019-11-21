using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.LocationGenerator
{
    class LocationGeneratorModel
    {
        #region Members
        private List<string> locationType = new List<string>();
        private List<string> populationSize = new List<string>();

        private string currentLocationType;
        private string currentPopulationSize;
        private string locationName;
        private string locationDescription;
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
        }
        public string LocationDescription
        {
            get { return locationDescription; }
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
            Console.WriteLine("Generate Location Button Pressed");
        }

        public void SaveLocation()
        {
            Console.WriteLine("Save Location Button Pressed");
        }
    }
}
