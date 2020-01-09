using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation
{
    class RecentCreations
    {
        #region Members
        private static RecentCreations instance = new RecentCreations();
        private static ObservableCollection<Creation> mostRecentCreations = new ObservableCollection<Creation>();
        #endregion

        #region Getters and Setters
        public static RecentCreations Instance
        {
            get { return instance; }
        }

        public static ObservableCollection<Creation> MostRecentCreations
        {
            get { return mostRecentCreations; }
        }
        #endregion

        #region Constructor
        private RecentCreations() { }
        #endregion

        #region Methods
        public void AddCreation(String creationName, GeneratorTypesEnum creationType, Dictionary<string, string> creation)
        {
            Creation newCreation = new Creation(creationName, creationType, creation);
            mostRecentCreations.Add(newCreation);

            if (mostRecentCreations.Count > 10)
                mostRecentCreations.RemoveAt(0);
        }
        #endregion
    }
}
