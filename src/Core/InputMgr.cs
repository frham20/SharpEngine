using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sharp.Core
{
    public class InputMgr : Microsoft.Xna.Framework.GameComponent
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private static InputMgr sInstance = null;
        private float centerMouseX = 0;
        private float centerMouseY = 0;
        private bool captureMouse = true;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public static InputMgr Instance
        {
            get { return sInstance; }
        }

        public bool CaptureMouse
        {
            get 
            { 
                return this.captureMouse; 
            }
            set 
            { 
                this.captureMouse = value;
                this.Game.IsMouseVisible = !value;
            }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public InputMgr(Game game)
            : base(game)
        {
        }

        public static InputMgr Create(Game game)
        {
            sInstance = new InputMgr(game);
            return sInstance;
        }

        public Vector2 GetMouseDelta()
        {
            if (!this.captureMouse)
                return new Vector2(0.0f, 0.0f);
          
            MouseState ms = Mouse.GetState();
            float dx = (float)ms.X - this.centerMouseX;
            float dy = (float)ms.Y - this.centerMouseY;
            return new Vector2(dx, dy);
        }

        public override void Initialize()
        {
            //get center of window
            this.centerMouseX = (float)this.Game.Window.ClientBounds.Width * 0.5f;
            this.centerMouseY = (float)this.Game.Window.ClientBounds.Height * 0.5f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //put mouse cursor in middle of screen
            if (this.Game.IsActive && this.captureMouse)
            {
                Mouse.SetPosition(this.Game.Window.ClientBounds.Width / 2,
                                    this.Game.Window.ClientBounds.Height / 2);
            }

            base.Update(gameTime);
        }
        #endregion
    }
}
