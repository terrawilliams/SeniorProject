using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation
{
    class Creation
    {
        #region Members
        private string name;
        private GeneratorTypesEnum type;
        private Dictionary<string, string> generation;
        #endregion

        #region Getters and Setters
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public GeneratorTypesEnum Type
        {
            get { return type; }
            set { type = value; }
        }

        public Dictionary<string, string> Generation
        {
            get { return generation; }
            set { generation = value; }
        }
        #endregion

        #region Constructor
        public Creation(string newName, GeneratorTypesEnum newType, Dictionary<string, string> newGeneration)
        {
            name = newName;
            type = newType;
            generation = newGeneration;
        }
        #endregion

        #region Methods

        #endregion
    }
}
