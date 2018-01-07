using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sharp.Core;

namespace Sharp.Graphics
{
    public class Model : Sharp.Core.Resource
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        //private BoundingBox bbox = new BoundingBox();
        private Mesh[] meshArray = null;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/

        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public override void Load(System.IO.Stream stream)
        {
            GraphicsDevice device = GraphicsMgr.Instance.Device;
            if (device == null)
                throw new Exception("Invalid graphic device");

            BinaryReader reader = new BinaryReader(stream);
            
            //read magic number
            uint magicNumber = reader.ReadUInt32();

            //read number of meshes
            uint meshCount = reader.ReadUInt32();
            this.meshArray = new Mesh[meshCount];

            //read each mesh
            for(uint i = 0; i < meshCount; i++)
            {
                this.meshArray[i] = new Mesh();
                this.meshArray[i].Load(stream);
            }
        }

        public void Render(GraphicsDevice device)
        {
            for (int i = 0; i < this.meshArray.Length; i++)
            {
                this.meshArray[i].Render(device);
            }
        }
        #endregion
    }
}
