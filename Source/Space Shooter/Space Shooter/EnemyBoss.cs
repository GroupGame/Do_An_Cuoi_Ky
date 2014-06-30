using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public class EnemyBoss
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultyLevel, nBullet = 50;
        public bool isVisible;
        public List<Bullet> bulletList;
        public EnemyBoss()
        {
            nBullet = 5;
            bulletList = new List<Bullet>();
            health = 5;
            currentDifficultyLevel = 1;
            position = new Vector2(400, 30);
            bulletDelay = 40;
            speed = 3;
            isVisible = true;
            health = 400;
        }
        public EnemyBoss(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletText)
        {
            nBullet = 5;
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletText;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 40;
            speed = 3;
            isVisible = true;
            health = 400;

        }
        public  void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("boss");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
        }
        public  void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            //move enemy back to top of the screen if he fly's off bottom
            if (position.Y >= 700)
            {
                isVisible = false;
                //position.Y = -75;
            }

            if (health <= 0)
                isVisible = false;

            EnemyShoot();
            UpdateBullets();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }
        public void UpdateBullets()
        {
            //foreach bullet in our bulletlis: update the movement and if the bullet hiet the top og the screen remove form the list
            foreach (Bullet b in bulletList)
            {
                //boungdingbox for our every bullet in our bulletlist
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                //set movement for bullet
                b.position.Y = b.position.Y + b.speed;

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

        public void EnemyShoot()
        {
            if (bulletDelay >= 0)
            {
                bulletDelay--;
            }
            if (bulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);
                newBullet.isVisible = true;
                if (bulletList.Count() < 15)
                    bulletList.Add(newBullet);
            }
            if (bulletDelay == 0)
            {
                bulletDelay = 90;
            }
           
           
       
        }
    }
}
