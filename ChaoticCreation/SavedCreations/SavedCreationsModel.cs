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
        private Creation selectedCreation;
        private bool editingCreation;

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
                
                if(Save.SavedNpcNames.Contains(selectedCreationName) ||
                   Save.SavedLocationNames.Contains(selectedCreationName) ||
                   Save.SavedEncounterNames.Contains(selectedCreationName))
                {
                    foreach(Creation creation in Save.CurrentlySavedCreations)
                    {
                        if (creation.Name == selectedCreationName)
                            selectedCreation = creation;
                    }
                }
                else
                {
                    selectedCreationName = string.Empty;
                }

                OnPropertyChanged("SelectedCreationName");
                DisplaySelectedCreation();
            }
        }

        public string SelectedCreationDescription
        {
            get { return selectedCreationDescription; }
        }

        public bool EditingCreation
        {
            get { return editingCreation; }
        }
        #endregion

        #region Constructor
        public SavedCreationsModel()
        {
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Displays information from the selected creation
        /// </summary>
        private void DisplaySelectedCreation()
        {
            if(selectedCreationName == string.Empty)
            {
                selectedCreationDescription = string.Empty;
                OnPropertyChanged("SelectedCreationDescription");
                return;
            }

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

        /// <summary>
        /// Deletes the selected creation from the database
        /// </summary>
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

        /// <summary>
        /// Alerts the application that a creation is being edited
        /// </summary>
        public void EditCreation()
        {
            if (selectedCreation.Type != GeneratorTypesEnum.Encounter)
            {
                editingCreation = true;
                OnPropertyChanged("EditingCreation");
            }
        }

        /// <summary>
        /// Deletes the original instance of the creation from the database and saves the new, edited version
        /// </summary>
        /// <param name="newName">new name of the creation entered by the user</param>
        /// <param name="newDescripiton">new description of the creation entered by the user</param>
        public void SaveNewVersionOfCreation(string newName, string newDescripiton)
        {
            Dictionary<string, string> newGeneration = new Dictionary<string, string>();
            newGeneration.Add("name", newName);
            newGeneration.Add("description", newDescripiton);
            Creation currentCreation = new Creation(newName, selectedCreation.Type, newGeneration);
            Save.Instance.deleteCreation(selectedCreation);
            Save.Instance.Creation(currentCreation);
        }

        /// <summary>
        /// Alerts the application that the creation is no longer being edited
        /// </summary>
        public void StopEditingCreation()
        {
            editingCreation = false;
            OnPropertyChanged("EditingCreation");
        }

        /// <summary>
        /// Allerts the front end that a property has been changed and needs to update
        /// </summary>
        /// <param name="property">The property that needs to be updated</param>
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
