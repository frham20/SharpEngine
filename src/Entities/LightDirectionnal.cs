using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sharp.Entities
{
    class LightDirectionnal : Light
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private Vector3 direction = new Vector3(0.0f, -1.0f, 0.0f);
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public Vector3 Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        #endregion
    }
}
