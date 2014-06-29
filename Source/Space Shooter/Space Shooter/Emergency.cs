using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Shooter
{
    class Emergency
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;

        public Rectangle boundingBox;

        public bool isVisible;
        Random random = new Random();
        public float randX, randY;
        private int a;
        //constructor
        public Emergency(Texture2D newTexture, Vector2 newPosition)
        {
            a = random.Next(1, 100);
            position = newPosition;
            texture = newTexture;
            speed = Global.speedOfAsteroid;
            isVisible = true;
            randX = random.Next(0, 750);
            randY = random.Next(-600, -50);
        }

        public void LoadContent(ContentManager Content)
        {
            //texture = Content.Load<Texture2D>("asteroid");

        }

        public void Update(GameTime gameTime)
        {
            //set bounding box for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            //origin.X = texture.Width / 2;
            //origin.Y = texture.Height / 2;
            //update Movement
            position.Y = position.Y + speed;
            //if (position.Y >= 700)
            //    position.Y = -50;
            if (position.Y > 700)
                isVisible = false;
            // rotate asteroid
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //rotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, Color.White);/*, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);*/
            }
        }
    }
}
