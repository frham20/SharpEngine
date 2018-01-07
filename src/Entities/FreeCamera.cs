using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sharp.Core;

namespace Sharp.Entities
{
    public class FreeCamera : Camera
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private float speed = 0.5f; //meters per second
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public override void Update(GameTime gameTime)
        {
            float deltaSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * this.speed;

            Vector3 at = this.xform.Forward;
            Vector3 right = this.xform.Right;

            right.Normalize();
            at.Normalize();

            Vector2 dmouse = InputMgr.Instance.GetMouseDelta();
            float deltaHeading = (float)dmouse.X * -0.003f;
            float deltaPitch = (float)dmouse.Y * -0.003f;

            //transform world Up in local space...costly, can do better than that
            Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f);
            worldUp = Vector3.TransformNormal(worldUp, Matrix.Invert(this.worldXForm));
            worldUp.Normalize();

            Matrix rotMatrixY = Matrix.CreateFromAxisAngle(worldUp, deltaHeading);
            Matrix.Multiply(ref rotMatrixY, ref this.xform, out this.xform);

            Matrix rotMatrixX = Matrix.CreateRotationX(deltaPitch);
            Matrix.Multiply(ref rotMatrixX, ref this.xform, out this.xform);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                this.xform.Translation += at * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                this.xform.Translation -= at * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                this.xform.Translation -= right * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                this.xform.Translation += right * deltaSpeed;

            UpdateWorldTransform();            
        }
        #endregion
    }
}
