using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Sharp.Entities;

namespace Sharp.Core
{
    public class EntityMgr : Microsoft.Xna.Framework.GameComponent
    {
        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        public Dictionary<uint, Entity> entityMap = new Dictionary<uint, Entity>();
        private static EntityMgr sInstance = null;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public static EntityMgr Instance
        {
            get { return sInstance; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public EntityMgr(Game game)
            : base(game)
        {
        }

        public static EntityMgr Create(Game game)
        {
            sInstance = new EntityMgr(game);
            return sInstance;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void AddEntity(Entity entity)
        {
            this.entityMap.Add(entity.ID, entity);
        }

        public void RemoveEntity(Entity entity)
        {
            this.entityMap.Remove(entity.ID);
        }

        public override void Update(GameTime gameTime)
        {
            //update all active entities
            foreach (KeyValuePair<uint,Entity> pair in this.entityMap)
            {
                Entity entity = pair.Value;
                if (entity.Active)
                {
                    entity.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }
        #endregion
    }
}
