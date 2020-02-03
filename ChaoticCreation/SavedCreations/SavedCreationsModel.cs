using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.SavedCreations
{
    class SavedCreationsModel
    {
        #region Members
        private List<Creation> savedEncounters = new List<Creation>();
        private List<Creation> savedLocations = new List<Creation>();
        private List<Creation> savedNpcs = new List<Creation>();
        #endregion

        #region Getters and Setters
        public List<Creation> SavedEncounters
        {
            get { return savedEncounters; }
        }

        public List<Creation> SavedLocations
        {
            get { return savedLocations; }
        }

        public List<Creation> SavedNpcs
        {
            get { return savedNpcs; }
        }
        #endregion

        #region Constructor

        #endregion

        #region Methods

        #endregion
    }
}
