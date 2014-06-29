using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Space_Shooter
{
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight, playerLive, playerLevel;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos, playerLivePos, levelPos;
        public bool showHud;

        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 700;
            screenWidth = 800;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth / 2, 50);
            levelPos.X = playerLivePos.X;
            levelPos.Y = playerLivePos.Y - 40;
        }
        public HUD(float x, float y, float xlive, float ylive)
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 700;
            screenWidth = 800;
            playerScoreFont = null;
            playerScorePos = new Vector2(x, y);
            playerLivePos = new Vector2(xlive, ylive);
            levelPos.X = playerLivePos.X;
            levelPos.Y = playerLivePos.Y - 40;
        }
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("georgia");
        }

        public void Update(GameTime gameTime)
        {
            //Get keyboard state
            KeyboardState keyState = Keyboard.GetState();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if we are showing our Hud(if showhud = true) then display our hud
            if (showHud)
            {
                spriteBatch.DrawString(playerScoreFont, "Score: " + playerScore, playerScorePos, Color.Red);
                spriteBatch.DrawString(playerScoreFont, "Live: " + playerLive, playerLivePos, Color.Red);
                spriteBatch.DrawString(playerScoreFont, "Level : " + playerLevel, levelPos, Color.Red);
            }

        }
    }
}
