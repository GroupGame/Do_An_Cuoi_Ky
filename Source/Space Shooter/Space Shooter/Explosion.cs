﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Space_Shooter
{
    class Explosion
    {
        public Texture2D texture;
        public Vector2 position;
        public float timer;
        public float interval;
        public Vector2 origin;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;
        
        public Explosion(Texture2D newTexture, Vector2 newPosition){
            
            position = newPosition;
            texture = newTexture;
            timer =0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisible = true;

        }

        public void LoadContent(ContentManager Content)
        {
        }

        public void Update(GameTime gameTime)
        {

            //increate the timer by number of millioseconds since update was last called
            timer += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);

            //check the time is more than the chosen interval
            if (timer > interval)
            {
                //show next frame
                currentFrame++;
                //reset timer
                timer = 0f;
            }
            //if were on  the last frame,make the explosion isvisible, resest back to the one before the first frame(because currentfarme++ is called next so the next frame will be 1
            if(currentFrame == 16)
            {
                isVisible = false;
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
