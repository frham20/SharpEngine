using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sharp.Graphics
{
    public class Material : Sharp.Core.Resource
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private string name;
        private SamplerState[] samplerStateArray = null;
        private Texture[] textureArray = null;
        private PixelShader pixelShader = null;
        private VertexShader vertexShader = null;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public void Apply(GraphicsDevice device)
        {
            //setup textures
            for(int i = 0; i < this.textureArray.Length; i++)
            {
                //device.SamplerStates[i] = this.samplerStateArray[i];
                device.Textures[i] = this.textureArray[i];
            }

            //setup pixel and vertex shader
            device.PixelShader = this.pixelShader;
            device.VertexShader = this.vertexShader;            
        }

        public override void Load(System.IO.Stream stream)
        {
            XmlTextReader reader = new XmlTextReader(stream);
            reader.Read();
            if (!reader.Name.Equals("MATERIAL", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid material node!");

            //read material name
            this.name = reader.GetAttribute("name");

            //read child nodes
            while (reader.Read())
            {
                if(reader.Name.Equals("TEXTURELIST", StringComparison.OrdinalIgnoreCase))
                {
                    XmlReader subReader = reader.ReadSubtree();
                    List<Texture> textureList = new List<Texture>();
                    while (subReader.Read())
                    {
                        /*
                        if (subReader.Name.Equals("TEXTURE", StringComparison.OrdinalIgnoreCase))
                        {
                            string texName = subReader.GetAttribute("name");
                            
                            textureList.Add(
                        }*/
                    }
                }
                else if(reader.Name.Equals("PIXELSHADER", StringComparison.OrdinalIgnoreCase))
                {
                }
                else if(reader.Name.Equals("VERTEXSHADER", StringComparison.OrdinalIgnoreCase))
                {
                }
            }

            base.Load(stream);
        }
        #endregion
    }
}
