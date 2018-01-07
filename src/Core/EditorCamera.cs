using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sharp.Entities;

namespace Sharp.Core
{
    class EditorCamera : Camera
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

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                InputMgr.Instance.CaptureMouse = true;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                {
                    //transform world Up in local space...costly, can do better than that
                    Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f);
                    worldUp = Vector3.TransformNormal(worldUp, Matrix.Invert(this.worldXForm));

                    Matrix rotMatrixY = Matrix.CreateFromAxisAngle(worldUp, deltaHeading);
                    Matrix.Multiply(ref rotMatrixY, ref this.xform, out this.xform);

                    Matrix rotMatrixX = Matrix.CreateRotationX(deltaPitch);
                    Matrix.Multiply(ref rotMatrixX, ref this.xform, out this.xform);
                }
                else
                {
                    Vector3 up = this.xform.Up;
                    up.Normalize();
                    this.xform.Translation += right * deltaHeading * deltaSpeed * 10.0f;
                    this.xform.Translation += up * deltaPitch * deltaSpeed * -10.0f;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                this.xform.Translation += at * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                this.xform.Translation -= at * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                this.xform.Translation -= right * deltaSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                this.xform.Translation += right * deltaSpeed;


            if (Mouse.GetState().MiddleButton == ButtonState.Released)
                InputMgr.Instance.CaptureMouse = false;

            UpdateWorldTransform();
        }
        #endregion
    }
}
