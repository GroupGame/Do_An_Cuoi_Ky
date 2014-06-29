using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public class Player
    {
        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition;
        public int speed, health;
        public int live, level, score;

        public float bulletDelay;
        //collision Variables
        public Rectangle boundingBox, healthRectangle;
        public bool isCollinding;
        public SoundManager sm = new SoundManager();


        public List<Bullet> bulletList;
        //constructor
        public Player()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(300, 300);
            bulletDelay = 20;
            speed = 10;
            isCollinding = false;
            health = 200;
            live = 3;
            healthBarPosition = new Vector2(50, 50);
            score =0;
            level =0;

        }

        //Load content
        public virtual void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
            healthTexture = Content.Load<Texture2D>("healthbar");
            sm.LoadContent(Content);
        }


        //Update
        public virtual void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();


            //boudingbox for our player ship
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 25);
            //fire bullets
            //if (keyState.IsKeyDown(Keys.Space))
            //{
            //    Shoot();
            //}
            UpdateBullets();
            UpLevel();
            //if (keyState.IsKeyDown(Keys.Up))
            //{
            //    position.Y = position.Y - speed;
            //}
            //if(keyState.IsKeyDown(Keys.Down))
            //{
            //    position.Y = position.Y + speed;
            //}
            //if (keyState.IsKeyDown(Keys.Left))
            //{
            //    position.X = position.X - speed;
            //}
            //if (keyState.IsKeyDown(Keys.Right))
            //{
            //    position.X = position.X + speed;
            //}

            //keep ship in screen Background

            if (position.X <= 0) position.X = 0;
            if (position.X >= 800 - texture.Width) position.X = 800 - texture.Width;
            if (position.Y <= 0) position.Y = 0;
            if (position.Y >= 700 - texture.Height) position.Y = 700 - texture.Height;
        }

        //draw
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

            foreach (Bullet b in bulletList)
                b.Draw(spriteBatch);
        }

        //shoot: used to set starting position of our bullet
        public void Shoot()
        {
            //shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;
            //if bullet delay is at 0; create new bullet at player position, make it visible on the screen, then add that bullet to the List
            if (bulletDelay <= 0)
            {
                sm.playerShootSound.Play();
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);

            }
            //reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 20;
        }

        //upadate bullet
        public void UpdateBullets()
        {
            //foreach bullet in our bulletlis: update the movement and if the bullet hiet the top og the screen remove form the list
            foreach (Bullet b in bulletList)
            {
                //boungdingbox for our every bullet in our bulletlist
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                //set movement for bullet
                b.position.Y = b.position.Y - b.speed;

                //if bullet hit the top of the screen, then make visible false
                if (b.position.Y <= 0)
                    b.isVisible = false;

            }
            //iterate throught bullList and see if any of the bullets are not visible, if they arent then bullet from our bullet list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void UpLevel()
        {
            if(score >= 200)
                level =1;
            if(score >= 500)
                level =2;
            if (score >= 1000)
                level = 3;
            if (score >= 1600)
                level = 4;
        }

    }
}
