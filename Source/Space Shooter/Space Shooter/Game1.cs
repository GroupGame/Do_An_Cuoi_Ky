using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random= new Random();
        Player p = new Player();
        Starfield sf = new Starfield();
        HUD hud = new HUD();
        public int enemyBulletDamage;
        SoundManager sm = new SoundManager();

        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Explosion> explosionList = new List<Explosion>();
        List<Enemy> enemyList = new List<Enemy>();

        public Texture2D menuImage;
        public Texture2D gameOver;


        public enum State
        {
            Menu,
            Playing,
            GameOver
        }

        //set first state
        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 700;
            this.Window.Title = "Space Shooter";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 10;
            menuImage = null;
            gameOver = null;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            hud.LoadContent(Content);
            p.LoadContent(Content);
            sf.LoadContent(Content);
            sm.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("MenuBackground");
            gameOver = Content.Load<Texture2D>("GameOver");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //upadating playing state
            switch (gameState)
            {
                case State.Playing:
                    {
                        sf.speed = 5;
                        //updating enemy's and checking collision of enemyship to player ship
                        foreach (Enemy e in enemyList)
                        {
                            //check if enemyship is colliding with player
                            if (e.boundingBox.Intersects(p.boundingBox))
                            {
                                p.health -= 40;
                                e.isVisible = false;
                            }

                            // check enemy bullet collision with player ship 
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    p.health -= enemyBulletDamage;
                                    e.bulletList[i].isVisible = false;
                                }
                            }
                            //check player bullet collision to enemyshio
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (p.bulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    hud.playerScore += 20;
                                    p.bulletList[i].isVisible = false;
                                    e.isVisible = false;

                                }
                            }
                            e.Update(gameTime);
                        }

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            //check to see if any of the asteroid are collingding with our playership, if they are... set issvisible to flase(remove them from asretorid list)
                            if (a.boundingBox.Intersects(p.boundingBox))
                            {
                                p.health -= 20;
                                a.isVisible = false;

                            }

                            // interate through our bulletlist if any asteroid come in contact with these bulllets, destroy bullet and asteroid
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(p.bulletList[i].boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    hud.playerScore += 5;
                                    a.isVisible = false;
                                    p.bulletList.ElementAt(i).isVisible = false;
                                }
                            }

                            a.Update(gameTime);
                        }

                        //hud.Update(gameTime);

                        // if health <=0 go to game over
                        if (p.health <= 0)
                            gameState = State.GameOver;

                        p.Update(gameTime);
                        sf.Update(gameTime);
                        ManageExposion();
                        LoadAsteroids();
                        LoadEnemies();
                        break;
                    }
                case State.Menu:
                    {
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        sf.Update(gameTime);
                        sf.speed = 1;
                        break;

                    }
                case State.GameOver:
                    {
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            gameState = State.Menu;
                            MediaPlayer.Stop();

                            enemyList.Clear();
                            asteroidList.Clear();
                            p.health = 200;
                            hud.playerScore = 0;
                        }
                        
                        break;
                    }
            }




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();


            switch (gameState)
            {
                case State.Playing:
                    {
                        
                        
                        sf.Draw(spriteBatch);
                        p.Draw(spriteBatch);

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }
                        hud.Draw(spriteBatch);
                        break;
                    }
                case State.Menu:
                    {
                        sf.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, Vector2.Zero, Color.White);
                        break;
                    }
                case State.GameOver:
                    {
                        spriteBatch.Draw(gameOver, Vector2.Zero, Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Your final score was : " + hud.playerScore.ToString(), new Vector2(235, 100), Color.Red);
                        break;
                    }
            }

            spriteBatch.End();                        

            base.Draw(gameTime);
        }

        public void LoadAsteroids()
        {
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);
            if (asteroidList.Count() < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randX, randY)));
            }
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void LoadEnemies()
        {
            int randY = random.Next(-600, -50);
            int randX = random.Next(0,750);

            //if there are less than 3 enemies on the screen, then create more until there is 3 again
            if(enemyList.Count()<3){
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyship"),new Vector2(randX, randY), Content.Load<Texture2D>("EnemyBullet")));
            }
            for(int i=0; i<enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        //manage explosions
        public void ManageExposion()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}