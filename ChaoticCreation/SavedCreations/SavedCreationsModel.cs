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
        private string selectedCreationName;
        private string selectedCreationDescription;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public ObservableCollection<string> SavedEncounters
        {
            get { return Save.SavedEncounterNames; }
        }

        public ObservableCollection<string> SavedLocations
        {
            get { return Save.SavedLocationNames; }
        }

        public ObservableCollection<string> SavedNpcs
        {
            get { return Save.SavedNpcNames; }
        }

        public string SelectedCreationName
        {
            get { return selectedCreationName; }
            set 
            { 
                selectedCreationName = value;
                OnPropertyChanged("SelectedCreationName");
                DisplaySelectedCreation();
            }
        }

        public string SelectedCreationDescription
        {
            get { return selectedCreationDescription; }
        }
        #endregion

        #region Constructor
        public SavedCreationsModel()
        {
            
        }
        #endregion

        #region Methods
        private void DisplaySelectedCreation()
        {
            foreach (Creation creation in Save.CurrentlySavedCreations)
            {
                if(creation.Name == selectedCreationName)
                {
                    if(creation.Type == GeneratorTypesEnum.Encounter)
                    {
                        selectedCreationDescription = string.Empty;

                        foreach (KeyValuePair<string, string> monster in creation.Generation)
                        {
                            selectedCreationDescription += monster.Key + " " + monster.Value + "\n";
                        }
                    }
                    else
                    {
                        selectedCreationDescription = creation.Generation["description"];
                    }
                    OnPropertyChanged("SelectedCreationDescription");
                    return;
                }
            }
        }

        public void DeleteSelectedCreation()
        {
            if (selectedCreationName == string.Empty)
                return;

            foreach (Creation creation in Save.CurrentlySavedCreations)
            {
                if(creation.Name == selectedCreationName)
                {
                    Save.Instance.deleteCreation(creation);
                    return;
                }
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
