using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    class Starfield
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        public Starfield ()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -700);
            speed = 5;
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("space");

        }


        public void Update(GameTime gameTime)
        {
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //setting speed for background
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);

            //scrolling background(Repeating)
            if(bgPos1.Y>=700)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -700;
            }
        }
    }
}
