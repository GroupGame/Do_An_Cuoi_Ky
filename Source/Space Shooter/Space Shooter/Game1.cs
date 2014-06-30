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

        public static Random random= new Random();
        Player p = new Player1();
        Player p2 = new Player2();
        Starfield sf = new Starfield();
        //Diem player
        HUD hud = new HUD(50, 80, 50, 650);
        HUD hud2 = new HUD(600, 80, 600, 650);
        public int enemyBulletDamage;
        SoundManager sm = new SoundManager();

        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Emergency> emergencyList = new List<Emergency>();
        List<Explosion> explosionList = new List<Explosion>();
        List<Enemy> enemyList = new List<Enemy>();

        EnemyBoss enb = new EnemyBoss();
        public Texture2D menuImage;
        public Texture2D gameOver;

        public bool twoPlayer = false;

        public Texture2D AboutScreen;
        public Texture2D Help;
        public Texture2D Option;
        public Texture2D On_Sound;
        public Texture2D Off_Sound;
        public Texture2D easy;
        public Texture2D normal;
        public Texture2D hard;
        List<My2DSprite> sprites = new List<My2DSprite>();
        List<My2DSprite> btBack = new List<My2DSprite>();
        List<Texture2D> On_Off_Sound = new List<Texture2D>();
        List<Texture2D> Level = new List<Texture2D>();

        public enum State
        {
            Menu,
            Single,
            Multi,
            Option,
            Help,
            About,
            GameOver
        }

        public enum StateLevel
        {
            Easy,
            Normal,
            Hard
        }

        //set first state
        State gameState = State.Menu;
        StateLevel stateLevel = StateLevel.Easy;

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
            this.IsMouseVisible = true;

            hud.LoadContent(Content);
            hud2.LoadContent(Content);
            p.LoadContent(Content);
            p2.LoadContent(Content);
            sf.LoadContent(Content);
            sm.LoadContent(Content);
            MediaPlayer.Play(sm.bgMusic);

            menuImage = Content.Load<Texture2D>("MenuBackground");
            Help = Content.Load<Texture2D>("HelpScreen");
            AboutScreen = Content.Load<Texture2D>("AboutScreen");
            gameOver = Content.Load<Texture2D>("GameOver");

            enb.LoadContent(Content);


            Option = Content.Load<Texture2D>("OptionScreen");
            On_Sound = Content.Load<Texture2D>("On");
            Off_Sound = Content.Load<Texture2D>("Off");
            easy = Content.Load<Texture2D>("easy");
            normal = Content.Load<Texture2D>("normal");
            hard = Content.Load<Texture2D>("hard");
            On_Off_Sound.Add(On_Sound);
            On_Off_Sound.Add(Off_Sound);
            Level.Add(easy);
            Level.Add(normal);
            Level.Add(hard);
            CreateMenu();
            //CreateMenuButton(260, 150, "SinglePlayer");
            //CreateMenuButton(260, 220, "MultiPlayer");
            //CreateMenuButton(260, 290, "Option");
            //CreateMenuButton(260, 360, "Help");
            //CreateMenuButton(260, 430, "About");
            //CreateMenuButton(260, 500, "Exit");
            CreateButtonBack(650, 620, "Back");
        }
        public void CreateMenu()
        {
            CreateMenuButton(260, 150, "SinglePlayer");
            CreateMenuButton(260, 220, "MultiPlayer");
            CreateMenuButton(260, 290, "Option");
            CreateMenuButton(260, 360, "Help");
            CreateMenuButton(260, 430, "About");
            CreateMenuButton(260, 500, "Exit");
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void CreateMenuButton(int left, int top, string strTexture)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(strTexture));
            My2DSprite menuButtonSprite = new My2DSprite(left, top, textures);
            sprites.Add(menuButtonSprite);
        }
        private void CreateButtonBack(int left, int top, string strTexture)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(strTexture));
            My2DSprite menuButtonSprite = new My2DSprite(left, top, textures);
            btBack.Add(menuButtonSprite);
        }
        private int GetSelectedButtonIndex(Vector2 WorldPosition)
        {
            int idx = -1;
            for (int i = 0; i < sprites.Count; i++)
                if (sprites[i].IsSelected(WorldPosition))
                {
                    idx = i;
                    break;
                }
            return idx;
        }
        private int GetSelectedButtonBack(Vector2 WorldPosition)
        {
            int idx = -1;
            for (int i = 0; i < btBack.Count; i++)
                if (btBack[i].IsSelected(WorldPosition))
                {
                    idx = i;
                    break;
                }
            return idx;
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keyState = Keyboard.GetState();

            #region Menu Button Click
            if (MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
            {
                //duplicated code ==> should be optimized.
                Vector2 MousePosition = MouseEventHelper.GetInstance().GetMousePosition();

                int idx = GetSelectedButtonIndex(MousePosition);
                if (idx != -1)
                {
                    for (int i = 0; i < sprites.Count; i++)
                        sprites[i].Select(i == idx);
                    switch (idx)
                    {
                        case 0:
                           
                            gameState = State.Single;
                            sprites.Clear();
                            break;
                        case 1:
                            gameState = State.Multi;
                            sprites.Clear();
                            break;
                        case 2:
                            gameState = State.Option;
                            sprites.Clear();
                            break;
                        case 3:
                            gameState = State.Help;
                            sprites.Clear();
                            break;
                        case 4:
                            gameState = State.About;
                            sprites.Clear();
                            break;
                        case 5:
                            this.Exit();
                            break;
                         
                    }
                    
                    

                }
                int idxb = GetSelectedButtonBack(MousePosition);
                if (idxb != -1)
                {
                    for (int i = 0; i < btBack.Count; i++)
                        btBack[i].Select(i == idxb);
                    switch (idxb)
                    {
                        case 0:
                            CreateMenu();
                            gameState = State.Menu;
                            break;
                    }
                }

            }
            else if (MouseEventHelper.GetInstance().IsLeftButtonUp())
                {
                    Vector2 MousePosition =
                        MouseEventHelper.GetInstance().GetMousePosition();

                    //Vector2 WorldPosition = Global.Screen2World(MousePosition);
                    int idx = GetSelectedButtonIndex(MousePosition);
                    if (idx != -1)
                    {
                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].Select(i == idx);
                      
                    }
                    int idxb = GetSelectedButtonBack(MousePosition);
                    if (idxb != -1)
                    {
                        for (int i = 0; i < btBack.Count; i++)
                            btBack[i].Select(i == idxb);
                       
                    }
                }


            #endregion

            if (gameState == State.Multi)
            {
                this.IsMouseVisible = false;
                Global.twoPlayer = true;
            }
            else
            {
                this.IsMouseVisible = true;
                Global.twoPlayer = false;
            }
                
            //upadating playing state
            switch (gameState)
            {
                case State.Menu:
                    p.health = 200;
                    p2.health = 200;
                    hud.playerScore = 0;
                    hud2.playerScore = 0;
                    p.level = 0;
                    p2.level = 0;
                    p.live = 3;
                    p2.live = 3;
                    //CreateMenu();
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        gameState = State.Single;
                        MediaPlayer.Play(sm.bgMusic);
                    }
                    sf.Update(gameTime);
                    sf.speed = 1;
                           
                    break;

                case State.Single:
                    sf.speed = 2;
                    
                    if(p.score >=400 && p.score <=500)
                    {
                        if (enb.boundingBox.Intersects(p.boundingBox))
                        {
                            p.health -= 5;
                            sm.explodeSound.Play();
                        }
                        for (int i = 0; i < enb.bulletList.Count; i++)
                        {
                            if (p.boundingBox.Intersects(enb.bulletList[i].boundingBox))
                            {
                                p.health -= enemyBulletDamage;
                                sm.explodeSound.Play();
                                enb.bulletList[i].isVisible = false;
                            }
                        }
                        //check player bullet collision to enemyshio
                        for (int i = 0; i < p.bulletList.Count; i++)
                        {
                            if (p.bulletList[i].boundingBox.Intersects(enb.boundingBox))
                            {
                                sm.explodeSound.Play();
                                p.bulletList[i].isVisible = false;
                                enb.health -= 40;

                            }
                        }
                        if (enb.health <= 0)
                            enb.isVisible = false;
                    }


                    enb.Update(gameTime);
                    
                    
                    #region Player
                    //updating enemy's and checking collision of enemyship to player ship
                    foreach (Enemy e in enemyList)
                    {
                        //check if enemyship is colliding with player
                        if (e.boundingBox.Intersects(p.boundingBox))
                        {
                            p.health -= 40;
                            sm.explodeSound.Play();
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                            e.isVisible = false;
                        }

                        // check enemy bullet collision with player ship 
                        for (int i = 0; i < e.bulletList.Count; i++)
                        {
                            if (p.boundingBox.Intersects(e.bulletList[i].boundingBox))
                            {
                                p.health -= enemyBulletDamage;
                                sm.explodeSound.Play();
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
                                p.score += 20;
                                p.bulletList[i].isVisible = false;
                                e.isVisible = false;

                            }
                        }
                        e.Update(gameTime);
                    }

                    foreach (Emergency em in emergencyList)
                    {
                        if (em.boundingBox.Intersects(p.boundingBox))
                        {
                            sm.explodeSound.Play();
                            if (p.health <= (200 - 20))
                                p.health += 20;
                            em.isVisible = false;

                        }

                        // interate through our bulletlist if any asteroid come in contact with these bulllets, destroy bullet and asteroid
                        em.Update(gameTime);
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
                            sm.explodeSound.Play();
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
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
                                p.score += 5;
                                a.isVisible = false;
                                p.bulletList.ElementAt(i).isVisible = false;
                            }
                        }

                        a.Update(gameTime);
                    }

                    //hud.Update(gameTime);

                    // if health <=0 go to game over

                    Global.levelOfPlayer1 = p.level;
                    hud.playerLive = p.live;
                    hud.playerScore = p.score;
                    hud.playerLevel = p.level;
                    p.Update(gameTime);
                    sf.Update(gameTime);
                    ManageExposion();

                    LoadAsteroids();
                    LoadEmergency();

                    LoadEnemies();
                    if (p.health <= 0)
                    {
                        p.live = p.live - 1;

                        if (p.health <= 0 && p.live <= 0)
                            gameState = State.GameOver;
                        p.health = 200;
                    }
                    #endregion
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        CreateMenu();
                        gameState = State.Menu;
                    }
                            
                    break;
               
                case State.Multi:
                        sf.speed = 2;
                        

                        #region player
                        //updating enemy's and checking collision of enemyship to player ship
                        foreach (Enemy e in enemyList)
                        {
                            #region p1
                            //check if enemyship is colliding with player
                            if (e.boundingBox.Intersects(p.boundingBox))
                            {
                                p.health -= 40;
                                sm.explodeSound.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                e.isVisible = false;
                            }

                            // check enemy bullet collision with player ship 
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    p.health -= enemyBulletDamage;
                                    sm.explodeSound.Play();
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
                                    p.score += 20;
                                    p.bulletList[i].isVisible = false;
                                    e.isVisible = false;

                                }
                            }

                            #endregion
                            ///p2
                            #region p2
                            //check if enemyship is colliding with player
                            if (e.boundingBox.Intersects(p2.boundingBox))
                            {
                                p2.health -= 40;
                                sm.explodeSound.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                e.isVisible = false;
                            }

                            // check enemy bullet collision with player ship 
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p2.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    p2.health -= enemyBulletDamage;
                                    sm.explodeSound.Play();
                                    e.bulletList[i].isVisible = false;
                                }
                            }
                            //check player bullet collision to enemyshio
                            for (int i = 0; i < p2.bulletList.Count; i++)
                            {
                                if (p2.bulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    p2.score += 20;
                                    p2.bulletList[i].isVisible = false;
                                    e.isVisible = false;

                                }
                            }

                            #endregion
                            e.Update(gameTime);
                        }

                        foreach (Emergency em in emergencyList)
                        {
                            if (em.boundingBox.Intersects(p.boundingBox))
                            {
                                sm.explodeSound.Play();
                                if (p.health <= (200 - 20))
                                    p.health += 20;
                                em.isVisible = false;

                            }
                            if (em.boundingBox.Intersects(p2.boundingBox))
                            {
                                sm.explodeSound.Play();
                                if (p2.health <= (200 - 20))
                                    p2.health += 20;
                                em.isVisible = false;

                            }
                            // interate through our bulletlist if any asteroid come in contact with these bulllets, destroy bullet and asteroid
                            em.Update(gameTime);
                        }
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            #region p1
                            //check to see if any of the asteroid are collingding with our playership, if they are... set issvisible to flase(remove them from asretorid list)
                            if (a.boundingBox.Intersects(p.boundingBox))
                            {
                                sm.explodeSound.Play();                                
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
                                    p.score += 5;
                                    a.isVisible = false;
                                    p.bulletList.ElementAt(i).isVisible = false;
                                }
                            }
                            #endregion p1
                            //p2
                            #region p2
                            //check to see if any of the asteroid are collingding with our playership, if they are... set issvisible to flase(remove them from asretorid list)
                            if (a.boundingBox.Intersects(p2.boundingBox))
                            {
                                sm.explodeSound.Play();
                                p2.health -= 20;
                                a.isVisible = false;

                            }

                            // interate through our bulletlist if any asteroid come in contact with these bulllets, destroy bullet and asteroid
                            for (int i = 0; i < p2.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(p2.bulletList[i].boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    p2.score += 5;
                                    a.isVisible = false;
                                    p2.bulletList.ElementAt(i).isVisible = false;
                                }
                            }

                            #endregion
                            a.Update(gameTime);
                        }

                        #endregion

                        // if health <=0 go to game over
                        hud.playerLive = p.live;
                        hud2.playerLive = p2.live;
                        hud.playerScore = p.score;
                        hud.playerLevel = p.level;
                        hud2.playerScore = p2.score;
                        hud2.playerLevel = p2.level;

                        Global.levelOfPlayer1 = p.level;
                        Global.levelOfPlayer2 = p2.level;


                        p.Update(gameTime);
                        p2.Update(gameTime);
                        sf.Update(gameTime);
                        ManageExposion();
                        LoadAsteroids();
                        LoadEmergency();

                        LoadEnemies();
                        if (p.health <= 0)
                        {
                            if (p.live >= 0)
                                p.live = p.live - 1;
                                
                            if(p.live>0)
                                p.health = 200;
                        }
                        if (p2.health <= 0)
                        {
                            if (p2.live > 0)
                                p2.live = p2.live - 1;
                                
                            if(p2.live>0)
                                p2.health = 200;
                            
                        }

                        if (p.health <= 0 && p2.health <= 0 && p.live <= 0 && p2.live <= 0)
                            gameState = State.GameOver;
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            CreateMenu();
                            gameState = State.Menu;
                        }
                            
                    break;
                case State.Option:

                    break;
                case State.Help:
                    break;
                case State.About:
                    break;
                case State.GameOver:
                    {

                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            gameState = State.Menu;
                            MediaPlayer.Stop();

                            enemyList.Clear();
                            asteroidList.Clear();
                            p.health = 200;
                            p2.health = 200;
                            hud.playerScore = 0;
                            hud2.playerScore = 0;
                            p.level = 0;
                            p2.level = 0;
                            p.live = 3;
                            p2.live = 3;
                        }
                        if(sprites.Count==0)
                            CreateMenu();
                        break;
                    }
            }

            switch (stateLevel)
            {
                case StateLevel.Easy:
                    foreach (Enemy e in enemyList)
                    {
                        e.speed = 5;
                        e.nBullet = 40;
                    }
                    foreach (Asteroid a in asteroidList)
                        a.speed = 4;
                    break;
                case StateLevel.Normal:
                    foreach (Enemy e in enemyList)
                    {
                        e.speed = 7;
                        e.nBullet = 30;
                    }
                    foreach (Asteroid a in asteroidList)
                        a.speed = 10;
                    break;
                case StateLevel.Hard:
                    foreach (Enemy e in enemyList)
                    {
                        e.speed = 8;
                        e.nBullet = 35;
                    }
                    foreach (Asteroid a in asteroidList)
                        a.speed = 14;
                    break;
            }  


            MouseEventHelper.GetInstance().Update(gameTime);
            UpdateManChoi();
            base.Update(gameTime);
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].Update(gameTime);
            for (int i = 0; i < btBack.Count; i++)
                btBack[i].Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            
            switch (gameState)
            {
                case State.Menu:
                    sf.Draw(spriteBatch);
                    spriteBatch.Draw(menuImage, Vector2.Zero, Color.White);
                    //spriteBatch.Draw(singlePlayer, new Vector2(260, 200), Color.White);
                    //spriteBatch.Draw(multiPlayer, new Vector2(260, 280), Color.White);
                    //spriteBatch.Draw(Help, new Vector2(260, 360), Color.White);
                    //spriteBatch.Draw(About, new Vector2(260, 440), Color.White);
                    for (int i = 0; i < sprites.Count; i++)
                        sprites[i].Draw(gameTime, spriteBatch);
                    break;
                case State.Single:
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
                    foreach (Emergency em in emergencyList)
                    {
                        em.Draw(spriteBatch);
                    }
                    foreach (Enemy e in enemyList)
                    {
                        e.Draw(spriteBatch);
                    }
                    if (p.score >= 400 && p.score <= 500 && enb.isVisible ==true)
                       
                            enb.Draw(spriteBatch);



                    hud.Draw(spriteBatch);
                    break;
                case State.Multi:

                    sf.Draw(spriteBatch);
                    if (p.health > 0 && p.live > 0)
                    {
                        p.Draw(spriteBatch);
                    }
                    if (p2.health > 0 && p2.live > 0)
                    {
                        p2.Draw(spriteBatch);
                        
                    }
                    foreach (Explosion ex in explosionList)
                    {
                        ex.Draw(spriteBatch);
                    }

                    foreach (Asteroid a in asteroidList)
                    {
                        a.Draw(spriteBatch);
                    }
                    foreach (Emergency em in emergencyList)
                    {
                        em.Draw(spriteBatch);
                    }
                    foreach (Enemy e in enemyList)
                    {
                        e.Draw(spriteBatch);
                    }
                   

                       
                    hud.Draw(spriteBatch);
                    hud2.Draw(spriteBatch);
                    break;
                case State.Option:
                    #region option
                    Vector2 r_sound = new Vector2();
                    Vector2 MousePosition = MouseEventHelper.GetInstance().GetMousePosition();
                    r_sound.X = 300;
                    r_sound.Y = 280;
                    spriteBatch.Draw(Option, Vector2.Zero, Color.White);
                    if (SoundManager.boolSound == true)
                    {
                        spriteBatch.Draw(On_Off_Sound[0], r_sound, Color.White);
                        if (IsSelected(MousePosition, r_sound, On_Off_Sound[0]) && MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
                            SoundManager.boolSound = false;
                    }
                    else if (SoundManager.boolSound == false)
                    {
                        spriteBatch.Draw(On_Off_Sound[1], r_sound, Color.White);
                        if (IsSelected(MousePosition, r_sound, On_Off_Sound[1]) && MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
                            SoundManager.boolSound = true;
                    }

                    Vector2 r_level = new Vector2();
                    r_level.X = 300;
                    r_level.Y = 330;
                    if (stateLevel == StateLevel.Easy)
                    {
                        spriteBatch.Draw(Level[0], r_level, Color.White);
                        if (IsSelected(MousePosition, r_level, Level[0]) && MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
                            stateLevel = StateLevel.Normal;
                    }
                    else if (stateLevel == StateLevel.Normal)
                    {
                        spriteBatch.Draw(Level[1], r_level, Color.White);
                        if (IsSelected(MousePosition, r_level, Level[1]) && MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
                            stateLevel = StateLevel.Hard;
                    }
                    else if (stateLevel == StateLevel.Hard)
                    {
                        spriteBatch.Draw(Level[2], r_level, Color.White);
                        if (IsSelected(MousePosition, r_level, Level[2]) && MouseEventHelper.GetInstance().HasLeftButtonDownEvent())
                            stateLevel = StateLevel.Easy;
                    }

                    for (int i = 0; i < btBack.Count; i++)
                        btBack[i].Draw(gameTime, spriteBatch);
                    #endregion
                    break;
                case State.Help:
                    spriteBatch.Draw(Help, Vector2.Zero, Color.White);
                    for (int i = 0; i < btBack.Count; i++)
                        btBack[i].Draw(gameTime, spriteBatch);
                    break;
                case State.About:
                    spriteBatch.Draw(AboutScreen, Vector2.Zero, Color.White);
                    for (int i = 0; i < btBack.Count; i++)
                        btBack[i].Draw(gameTime, spriteBatch);
                    break;
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
            if (asteroidList.Count() < Global.numOfAsteroid)
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

        public void LoadEmergency()
        {
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);
            int a4 = random.Next(1, 200);
            //if (emergencyList.Count() < 1)

            if (a4 == 50)
            {
                
                emergencyList.Add(new Emergency(Content.Load<Texture2D>("red_heart"), new Vector2(randX, randY)));
            }
            for (int i = 0; i < emergencyList.Count; i++)
            {
               
                if (!emergencyList[i].isVisible)
                {
                    emergencyList.RemoveAt(i);
                    i--;
                }
            }
            if(emergencyList.Count>1)
            {
                emergencyList.RemoveAt(1);
            }
        }
        public void LoadEnemies()
        {
            int randY = random.Next(-600, -50);
            int randX = random.Next(0,750);
            int a = random.Next(1, 5);
            //if there are less than 3 enemies on the screen, then create more until there is 3 again
            if(enemyList.Count()<Global.numOfEnemy){
                if(a==3)
                    enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyship"), new Vector2(randX, randY), Content.Load<Texture2D>("EnemyBullet")));
                if(a==4)
                    enemyList.Add(new Enemy(Content.Load<Texture2D>("starshipdark"), new Vector2(randX, randY), Content.Load<Texture2D>("torpedodark")));

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
        public bool IsSelected(object obj, Vector2 r, Texture2D textture)
        {
            Vector2 pos = (Vector2)obj;
            if (pos.X >= r.X && pos.X <= r.X + textture.Width &&
                pos.Y >= r.Y && pos.Y <= r.Y + textture.Height)
                return true;
            return false;
        }
        public void UpdateManChoi()
        {
            if (Global.levelOfPlayer1 == 1 || Global.levelOfPlayer2 == 1)
            {
                Global.numOfAsteroid = 6;
                Global.speedOfAsteroid = 5;
                Global.numOfEnemy = 4;
                Global.speedOfEnemy = 4;
            }
            if (Global.levelOfPlayer1 == 2 || Global.levelOfPlayer2 == 2)
            {
                Global.numOfAsteroid = 7;
                Global.speedOfAsteroid = 5;
                Global.numOfEnemy = 4;
                Global.speedOfEnemy = 6;
            }


            if (Global.levelOfPlayer1 == 3 || Global.levelOfPlayer2 == 3)
            {
                Global.numOfAsteroid = 7;
                Global.speedOfAsteroid = 5;
                Global.numOfEnemy = 12;
                Global.speedOfEnemy = 10;
            }
        }
    }
}