using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sharp.Entities
{
    public class Camera : Entity
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private float hFOV = 0.75f;
        private float nearDist = 1.0f;
        private float farDist = 5000.0f;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public float HFOV
        {
            get { return this.hFOV; }
            set { this.hFOV = value; }
        }

        public float DistanceNear
        {
            get { return this.nearDist; }
            set { this.nearDist = value; }
        }

        public float DistanceFar
        {
            get { return this.farDist; }
            set { this.farDist = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        #endregion
    }
}
