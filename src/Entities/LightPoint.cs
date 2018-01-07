using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.Entities
{
    public class LightPoint : Light
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private float radius = 100.0f;
        private float attenuationStart = 80.0f;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public float Radius
        {
            get { return this.radius; }
            set { this.radius = value; }
        }

        public float AttenuationStart
        {
            get { return this.attenuationStart; }
            set { this.attenuationStart = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        #endregion
    }
}
