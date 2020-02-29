using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.SavedCreations
{
    class SavedCreationsModel : INotifyPropertyChanged
    {
        #region Members

        private ObservableCollection<Creation> savedEncounters = new ObservableCollection<Creation>();
        private ObservableCollection<Creation> savedLocations = new ObservableCollection<Creation>();
        private ObservableCollection<Creation> savedNpcs = new ObservableCollection<Creation>();

        private ObservableCollection<string> savedEncountersNames = new ObservableCollection<string>();
        private ObservableCollection<string> savedLocationsNames = new ObservableCollection<string>();
        private ObservableCollection<string> savedNpcsNames = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public ObservableCollection<string> SavedEncounters
        {
            get { return savedEncountersNames; }
        }

        public ObservableCollection<string> SavedLocations
        {
            get { return savedLocationsNames; }
        }

        public ObservableCollection<string> SavedNpcs
        {
            get { return savedNpcsNames; }
        }
        #endregion

        #region Constructor
        public SavedCreationsModel()
        {
            Dictionary<string, string> sampleCreation = new Dictionary<string, string>();
            sampleCreation.Add("Sample", "data");

            Creation sampleEncounter = new Creation("Encounter 1", GeneratorTypesEnum.Encounter, sampleCreation);
            savedEncounters.Add(sampleEncounter);
            savedEncountersNames.Add(sampleEncounter.Name);

            Creation sampleLocation = new Creation("Location 1", GeneratorTypesEnum.Location, sampleCreation);
            savedLocations.Add(sampleLocation);
            savedLocationsNames.Add(sampleLocation.Name);

            Creation sampleNpc = new Creation("NPC 1", GeneratorTypesEnum.NPC, sampleCreation);
            savedNpcs.Add(sampleNpc);
            savedNpcsNames.Add(sampleNpc.Name);
        }
        #endregion

        #region Methods
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
