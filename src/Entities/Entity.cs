using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sharp.Entities
{
    public class Entity
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        public Matrix xform = Matrix.Identity;
        public Matrix worldXForm = Matrix.Identity;
        private uint id = 0;
        private Entity parent = null;
        private List<Entity> childList = new List<Entity>();
        private bool isActive = true;
        private string name = "Unknown";
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public bool Active
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        public uint ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

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
        public void AttachChild(Entity entity)
        {
            /*
            if (childList == null)          
                childList = new List<Entity>();
             * */

            entity.Detach();
            entity.parent = this;
            this.childList.Add(entity);
        }

        public void Detach()
        {
            if (this.parent == null)
                return;

            this.parent.childList.Remove(this);

            /*
            //nore more children in list? try to garbage collect it
            if (this.parent.childList.Count == 0)
                this.parent.childList = null;
             * */

            this.parent = null;
        }

        public void UpdateWorldTransform()
        {
            //no parent so just copy local matrix
            if (this.parent == null)
            {
                worldXForm = xform;
                return;
            }

            //else multiply local with parent's world
            worldXForm = this.parent.worldXForm * xform;

            //update children
            for (int i = 0; i < this.childList.Count; i++)
                this.childList[i].UpdateWorldTransform();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        /*
        public void LookAt(Vector3 pt, Vector3 up)
        {
            //extract scale in order to preserve it
            Vector3 scale = new Vector3(this.xform.Right.Length(),
                                        this.xform.Up.Length(),
                                        this.xform.Forward.Length());

            //get AT vector from object to point
            Vector3 at = pt - this.xform.Forward;
            at.Normalize();

            //find right vector
            Vector3 right = Vector3.Cross(at, up);

            //scale basis
            right *= scale.X;
            up *= scale.Y;
            at *= scale.Z;

            //put new basis back in matrix
            this.xform.Right = right;
            this.xform.Up = up;
            this.xform.Forward = at;
        }*/
        #endregion
    }

}
