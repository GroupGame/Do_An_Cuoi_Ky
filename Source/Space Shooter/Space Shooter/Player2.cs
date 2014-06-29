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
    public class Player2 : Player
    {
        public Player2()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(500, 600);
            bulletDelay = 20;
            speed = 10;
            isCollinding = false;
            health = 200;
            healthBarPosition = new Vector2(800 - health, 50);

        }
        public override void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("starship");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
            healthTexture = Content.Load<Texture2D>("healthbar");
            sm.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 25);
            //healthRectangle = new Rectangle()

            if (keyState.IsKeyDown(Keys.P))
            {
                Shoot();
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X = position.X - speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X = position.X + speed;
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

            base.Draw(spriteBatch);
        }
    }
}
