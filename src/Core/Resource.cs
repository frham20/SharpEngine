using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sharp.Core
{
    public class Resource
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private string name;
        private uint id = 0;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public uint ID
        {
            get { return this.id; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public virtual void Load(Stream stream)
        {
            
        }
        #endregion
    }
}
