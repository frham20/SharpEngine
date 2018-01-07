using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Sharp.Entities;

namespace Sharp.Core
{
    public class EngineMgr : Microsoft.Xna.Framework.Game
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private GraphicsMgr graphicsMgr = null;
        private InputMgr inputMgr = null;
        private GraphicsDeviceManager graphicsDeviceMgr = null;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public GraphicsDeviceManager GraphicsDeviceMgr
        {
            get { return this.graphicsDeviceMgr; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public EngineMgr()
        {
            this.graphicsDeviceMgr = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            this.Window.Title = "SharpEngine v0.1 [F1 for Editor]";
            this.Window.AllowUserResizing = true;
            this.IsFixedTimeStep = false;
            this.IsMouseVisible = false;

            //create and register components
            this.Components.Add(InputMgr.Create(this));
            this.Components.Add(EntityMgr.Create(this));
            this.Components.Add(GraphicsMgr.Create(this));
            this.Components.Add(ResourceMgr.Create(this));
            this.Components.Add(Editor.Create(this));

            base.Initialize();

            //HACK create our main free camera
            FreeCamera freecam = new FreeCamera();
            freecam.Name = "MainCamera";

            EntityMgr entityMgr = EntityMgr.Instance;
            entityMgr.AddEntity(freecam);
            GraphicsMgr.Instance.Camera = freecam;

            Light light = new LightPoint();
            light.Name = "Light0";
            light.ID = 1;
            entityMgr.AddEntity(light);
            light = new LightPoint();
            light.Name = "Light1";
            light.ID = 2;
            entityMgr.AddEntity(light);
            light = new LightPoint();
            light.Name = "Light2";
            light.ID = 3;
            entityMgr.AddEntity(light);
            light = new LightDirectionnal();
            light.Name = "Light3";
            light.ID = 4;
            entityMgr.AddEntity(light);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);          
        }


        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        #endregion
    }
}