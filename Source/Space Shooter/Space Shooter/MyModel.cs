using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public abstract class MyModel
    {
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime, Object obj)
        {

        }

        public virtual bool IsSelected(Object obj)
        {
            return false;
        }

        public virtual void Select(bool bSelected)
        {

        }
    }
}
