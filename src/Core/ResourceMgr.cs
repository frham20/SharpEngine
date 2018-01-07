using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Sharp.Core
{
    class ResourceMgr : Microsoft.Xna.Framework.GameComponent
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private static ResourceMgr sInstance = null;
        private ContentManager contentMgr = null;
        private Dictionary<string, Texture> textureMap = new Dictionary<string, Texture>();
        private Dictionary<string, Sharp.Graphics.Material> materialMap = new Dictionary<string, Sharp.Graphics.Material>();
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public static ResourceMgr Instance
        {
            get { return sInstance; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public ResourceMgr(Game game)
            : base(game)
        {
            this.contentMgr = new ContentManager(Game.Services);
        }

        public static ResourceMgr Create(Game game)
        {
            sInstance = new ResourceMgr(game);
            return sInstance;
        }

        public Texture GetTexture(string name)
        {
            Texture texture = null;
            if (this.textureMap.TryGetValue(name, out texture))
                return texture;

            texture = this.contentMgr.Load<Texture>(name);
            this.textureMap.Add(name, texture);
            return texture;
        }

        public Sharp.Graphics.Material GetMaterial(string name)
        {
            Texture texture = null;
            if (this.materialMap.TryGetValue(name, out texture))
                return texture;

            texture = this.contentMgr.Load<Texture>(name);
            this.textureMap.Add(name, texture);
            return texture;
        }

        public override void Update(GameTime gameTime)
        {
            //removed unused resources...TO DO

            base.Update(gameTime);
        }
        #endregion
    }
}
