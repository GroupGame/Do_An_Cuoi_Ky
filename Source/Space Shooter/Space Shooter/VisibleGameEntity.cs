using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public abstract class VisibleGameEntity : GameEntity
    {
        protected MyModel _MainModel;

        public MyModel MainModel
        {
            get { return _MainModel; }
            set { _MainModel = value; }
        }


        public override void Update(GameTime gameTime)
        {
            _MainModel.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, Object obj)
        {
            _MainModel.Draw(gameTime, obj);
        }

        public virtual bool IsSelected(Object obj)
        {
            return _MainModel.IsSelected(obj);
        }

        public virtual void Select(bool bSelected)
        {
            _MainModel.Select(bSelected);
        }
    }
}
