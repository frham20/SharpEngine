using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sharp.Entities
{
    public class Light : Entity
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private Color color = new Color(255, 255, 255, 255);
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        #endregion
    }
}
