#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace PlatformerTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //attributes
        bool drawDebug;

        //File IO objects
        // levels
        FileIO fio;
        int levelNum;

        // images
        Texture2D hero;
        Texture2D helogi_sheet;
        Texture2D babyChicka_sheet;
        Texture2D amibee_sheet;
        Texture2D sky;
        Texture2D citySkyline;
        Texture2D healthGraphic;
        Texture2D healthContainer;

        // sound
        SoundEffect grassTheme;
        SoundEffect grassViolin;
        SoundEffect grassTuba;
        SoundEffectInstance grassObj;
        SoundEffectInstance grassViolinObj;
        SoundEffectInstance grassTubaObj;

        // heroes
        Hero h1;
        Hero h2;
        Hero h3;
        Hero h4;

        DebugHUD dbHud = new DebugHUD();
        Texture2D block;
        Texture2D guySprite;
        Texture2D toughGuySprite;
        Texture2D wiseGuySprite;
        Texture2D smallGuySprite;
        Block[] blocks;
        KeyboardState currentkState = new KeyboardState();
        KeyboardState previouskState = new KeyboardState();
        SpriteFont font;
        string debug;
        Rectangle screen;

        List<Hero> listOfHeroes;
        List<Enemy> listOfEnemies;

        Texture2D projectileSheet;

        // menus
        Menu menuObj;

        enum MenuReader { GamePlay, GameOver };
        MenuReader menu;

       
        // players
        Player p1;
        Player p2;
        Player p3;
        Player p4;
        int numOfPlayers;

        //gamepadStates
        GamePadState gState;
        GamePadState PrevgState;
        
        // item stack
        ItemStack itemStack;
        int itemStackSize;
        List<Item> listOfItems;
        Texture2D itemSpriteSheet;

        // Room specific
        bool checkpointBool;
        Rectangle checkpoint;
        int deathPlane;
        int topPlane;
        int leftWall;
        int rightWall;
        Point startingPos;
        Goal goal;
        int nextLevel;
        HiddenBoundary hiddenBoundary; // not all rooms will use this

        int goalTimer;
        int goalCallMethod;
        int goalLimit;

        // temp list Items
        List<Item> tempList;

        // tiles
        Texture2D conveyorTile;
        Texture2D hazardTile;
        Texture2D groundTile;
        Texture2D treasureBoxTile;
        Texture2D movingTile;

        // test switch list
        //List<Switch> listOfSwitches;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            drawDebug = false;

            //set the camera 
            screen = new Rectangle(0, 0, 1280, 720);
            listOfHeroes = new List<Hero>();
            
            // menus
            menuObj = new Menu();
            menu = MenuReader.GamePlay;           

            // players
            p1 = new Player();
            p2 = new Player();
            p3 = new Player();
            p4 = new Player();

            // set up test
            fio = new FileIO();
            levelNum = 0;

            // STACK CREATION
            itemStack = new ItemStack();
            itemStackSize = 50;
            for (int i = 0; i < itemStackSize; i++)
            {
                itemStack.Push(new Item(0, blocks));
            }
            listOfItems = new List<Item>();
            
            //hidden boundary creation-- way out of the way by default.
            hiddenBoundary = new HiddenBoundary(new Rectangle(-1000 * 32, -1000 * 32, 1, 1));

            // goal obj
            goal = new Goal();
            nextLevel = 0;
            goalTimer = 0;
            goalCallMethod = 250;
            goalLimit = 18; //originally 16
            
            // generate beginning room
            GenerateRoom();
            h1 = new BabyChicko(blocks, PlayerIndex.One);
           
            listOfHeroes.Add(h1);

            gState = GamePad.GetState(PlayerIndex.One);
            PrevgState = gState;

            // setting keyboard states
            currentkState = Keyboard.GetState();
            previouskState = currentkState;
                        
            // enemies
            listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
            //listOfSwitches = fio.ReadSwitch(levelNum);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //we want to figure out a way around this -- i think
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hero = Content.Load<Texture2D>("block");
            block = Content.Load<Texture2D>("block");
            font = Content.Load<SpriteFont>("SpriteFont1");
            guySprite = Content.Load<Texture2D>("guy_spriteSheet");
            toughGuySprite = Content.Load<Texture2D>("toughguy_spriteSheet");
            wiseGuySprite = Content.Load<Texture2D>("wiseguy_spriteSheet");
            smallGuySprite = Content.Load<Texture2D>("smallGuy_spriteSheet");
            helogi_sheet = Content.Load<Texture2D>("helogi_spriteSheet");
            projectileSheet = Content.Load<Texture2D>("fx_spriteSheet_temp");
            babyChicka_sheet = Content.Load<Texture2D>("babyChickaSheet");
            amibee_sheet = Content.Load<Texture2D>("amibeeSheet");

            // item
            itemSpriteSheet = Content.Load<Texture2D>("itemsheet");

            // backgrounds
            sky = Content.Load<Texture2D>("sky");
            citySkyline = Content.Load<Texture2D>("citySkyline");

            //healthbar
            healthGraphic = Content.Load<Texture2D>("healthGraphic");
            healthContainer = Content.Load<Texture2D>("healthContainer");

            // tiles
            conveyorTile = Content.Load<Texture2D>("conveyerSheet");
            hazardTile = Content.Load<Texture2D>("hazardSheet");
            groundTile = Content.Load<Texture2D>("groundSheet");
            movingTile = Content.Load<Texture2D>("MovingBlockSheet");
            treasureBoxTile = Content.Load<Texture2D>("treasureBox");

            // music
            grassTheme = Content.Load<SoundEffect>("GrassTheme.wav");
            grassViolin = Content.Load<SoundEffect>("GrassThemeViolin.wav");
            grassTuba = Content.Load<SoundEffect>("GrassThemeTuba.wav");

            grassObj = grassTheme.CreateInstance();
            grassObj.IsLooped = true;
            grassObj.Play();
            grassObj.Volume = 1;

            grassViolinObj = grassViolin.CreateInstance();
            grassViolinObj.IsLooped = true;
            grassViolinObj.Play();
            grassViolinObj.Volume = 0;

            grassTubaObj = grassTuba.CreateInstance();
            grassTubaObj.IsLooped = true;
            grassTubaObj.Play();
            grassTubaObj.Volume = 0;
            //MediaPlayer.Play(grassTheme);
            //MediaPlayer.IsRepeating = true;
            //if (MediaPlayer.State != MediaState.Playing)
            //{
                
                
                
            //}
            // music
            //grassObj = grassTheme.CreateInstance();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (menu)
            {
                case MenuReader.GamePlay: UpdateGamePlay(gameTime); break;
                case MenuReader.GameOver: UpdateGameOver(gameTime); break;
            }
            base.Update(gameTime);
        }

        // other updates
        
        public void UpdateGamePlay(GameTime gameTime)
        {           
            // music
            // added the canfall check so the music doesn't switch mid jump.
            // the music only switches once you land on a platform
            if (h1.HeroPos.Y <= -(7 * 32) && !h1.CanFall)
            {
                grassObj.Volume = 0;
                grassViolinObj.Volume = 1;
                grassTubaObj.Volume = 0;
            }
            else if (h1.HeroPos.Y >= (7 * 32) && !h1.CanFall)
            {
                grassObj.Volume = 0;
                grassViolinObj.Volume = 0;
                grassTubaObj.Volume = 1;
            }
            else if (h1.HeroPos.Y < (7 * 32) && h1.HeroPos.Y > -(7 * 32) && !h1.CanFall)
            {
                grassObj.Volume = 1;
                grassViolinObj.Volume = 0;
                grassTubaObj.Volume = 0;
            }
            // TODO: Add your update logic here
            //set the input for the player
            //debug
            //debug = dbHud.CalcFps(gameTime, h1, h2, h3, h4);

            // TEST
            p1.PlayerSheet = babyChicka_sheet;

            currentkState = Keyboard.GetState();

            // debug
            if (currentkState.IsKeyDown(Keys.LeftShift) && previouskState.IsKeyUp(Keys.LeftShift)) drawDebug = !drawDebug;

            //update the player
            foreach (Hero h in listOfHeroes)
            {
                h.Update(blocks, currentkState, previouskState, listOfEnemies, gameTime, listOfItems, itemStack);

                    foreach (Projectile p in h.ListOfProj)
                    {
                        p.GetList(listOfHeroes);
                        if (!p.MarkedForRemoval) p.Update(gameTime);
                    }
            }

            // enemy
            // check for dead
            Enemy deadEnemy = null;
            foreach(Enemy e in listOfEnemies)
            {
                // if it doesn't exist or is dead, don't draw
                if (e.StateProp != Enemy.State.dead)
                {
                    e.Update(gameTime, listOfEnemies, itemStack, listOfItems);
                }

                if(e.state == Enemy.State.dead)
                {
                    deadEnemy = e;
                    break;
                }
            }           
            if(deadEnemy != null)
            {
                listOfEnemies.Remove(deadEnemy);
            }
            
            // update live items
            foreach(Item i in listOfItems)
            {
                i.Update();
            }


            // update moving blocks
            foreach(Block b in blocks)
            {
                if(b is MovingBlock)
                {
                    b.Update();
                }
            }


            //camera
            // if hero is alive
            if (h1 != null)
            {
                screen.X = h1.HeroPos.Center.X - (screen.Width / 2);
                screen.Y = h1.HeroPos.Center.Y - (screen.Height / 2);

                // stop at deathplane
                if(screen.Y + screen.Height >= deathPlane)
                {
                    screen.Y = deathPlane - screen.Height;
                }
                // stop at topPlane
                if(screen.Y < topPlane)
                {
                    screen.Y = topPlane;
                }
                // stop at left wall
                if(screen.X < leftWall)
                {
                    screen.X = leftWall;
                }
                // stop at right wall
                if(screen.X + screen.Width > rightWall)
                {
                    screen.X = rightWall - screen.Width;
                }
            }
            
            // checkpoint
            if(h1.HeroPos.Intersects(checkpoint) && checkpointBool == false)
            {                
                checkpointBool = true;
                h1.SetInventory();
                
            }

            //hiddenBoundary
            if(h1.HeroPos.Intersects(hiddenBoundary.BoundaryPos) && !hiddenBoundary.BoundaryCollide)
            {
                hiddenBoundary.BoundaryCollide = true;
            }
            if(hiddenBoundary.BoundaryCollide && rightWall != hiddenBoundary.NewBoundary.X)
            {
                rightWall += 8;
            }
            if (hiddenBoundary.BoundaryCollide && topPlane != hiddenBoundary.NewBoundary.Y)
            {
                topPlane -= 8;
            }

            // goal
            if(h1.HeroPos.Intersects(goal.GoalPos) && !goal.GoalCollide)
            {
                //ResetItemStack();
                goal.GoalCollide = true;               
            }
            if(goal.GoalCollide)
            {
                
                goalTimer += gameTime.ElapsedGameTime.Milliseconds;

                // every half second call method
                if(goalTimer > goalCallMethod)
                {
                    goal.DropItem(itemStack, listOfItems, 0, 1, 6, 4, 5); //gold, jelly, goldBars, blue jelly, spikes
                    goalTimer = 0;
                }

                // done dropping items
                if (goal.TimesCalled > goalLimit)
                {
                    levelNum = nextLevel;
                    GenerateRoom();
                    h1.SetInventory();
                    goal.TimesCalled = 0;
                    goal.GoalCollide = false;
                    hiddenBoundary.BoundaryCollide = false;
                }
            }

            // respawn
            if(h1.HeroPos.Y > deathPlane)
            {
                if(checkpointBool && !hiddenBoundary.BoundaryCollide)
                {
                    h1.Health = h1.MaxHealth;
                    h1.HeroPos = new Rectangle(checkpoint.X, checkpoint.Y, h1.HeroPos.Width, h1.HeroPos.Height);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                }
                else if(hiddenBoundary.BoundaryCollide)
                {
                    h1.Health = h1.MaxHealth;
                    h1.HeroPos = new Rectangle(hiddenBoundary.BoundaryPos.X, hiddenBoundary.BoundaryPos.Y, h1.HeroPos.Width, h1.HeroPos.Height);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                }
                else
                {
                    h1.Health = h1.MaxHealth;
                    h1.HeroPos = new Rectangle(startingPos.X, startingPos.Y, h1.HeroPos.Width, h1.HeroPos.Height);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                }
                // empty temporary inventory
                h1.TempInventory = new int[40, 2];
                for (int i = 0; i < h1.TempInventory.Length / 2; i++)
                {
                    h1.TempInventory[i, 0] = -1;
                }
            }
            
            //post update
            previouskState = currentkState;

            

            // check for dead players           
            if (h1 != null && h1.Health <= 0) 
            { 
                /*listOfHeroes.Remove(h1); 
                numOfPlayers--;
                h1 = null;*/
                if (checkpointBool)
                {
                    
                    //listOfHeroes.Remove(h1);
                    //h1 = new BabyChicka(blocks, PlayerIndex.One);
                    h1.Health = h1.MaxHealth;
                    h1.HeroPos = new Rectangle(checkpoint.X, checkpoint.Y, h1.HeroPos.Width, h1.HeroPos.Height);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                    
                    //listOfHeroes.Add(h1);
                    
                }
                else
                {
                    //listOfHeroes.Remove(h1);
                    //h1 = new BabyChicka(blocks, PlayerIndex.One);
                    //listOfHeroes.Add(h1);
                    h1.Health = h1.MaxHealth;
                    h1.HeroPos = new Rectangle(startingPos.X, startingPos.Y, h1.HeroPos.Width, h1.HeroPos.Height);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                }
                // empty temporary inventory
                h1.TempInventory = new int[40, 2];
                for (int i = 0; i < h1.TempInventory.Length / 2; i++)
                {
                    h1.TempInventory[i, 0] = -1;
                }

            }
            
            //if (h2 != null && h2.Health <= 0) 
            //{ 
            //    listOfHeroes.Remove(h2);
            //    numOfPlayers--;
            //    h2 = null;
            //}
            
            //if (h3 != null && h3.Health <= 0) 
            //{ 
            //    listOfHeroes.Remove(h3); 
            //    numOfPlayers--;
            //    h3 = null;
            //}

            //if (h4 != null && h4.Health <= 0) 
            //{
            //    listOfHeroes.Remove(h4);
            //    numOfPlayers--;
            //    h4 = null;
            //}

            
            /*if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                levelNum = 0;
                blocks = fio.ReadLevel(levelNum);
                listOfEnemies.Clear();
                

                // remove and add again to keep camera on hero
                listOfHeroes.Remove(h1);
                h1 = new BabyChicka(blocks, PlayerIndex.One);
                listOfHeroes.Add(h1);

                listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                listOfHeroes.Remove(h1);
                h1 = new BabyChicka(blocks, PlayerIndex.One);
                listOfHeroes.Add(h1);
            }*/

        }

        public void UpdateGameOver(GameTime gameTime)
        {
            gState = GamePad.GetState(PlayerIndex.One);
            if (h1 != null) { gState = GamePad.GetState(PlayerIndex.One); }
            if (h2 != null) { gState = GamePad.GetState(PlayerIndex.Two); }
            if (h3 != null) { gState = GamePad.GetState(PlayerIndex.Three); }
            if (h4 != null) { gState = GamePad.GetState(PlayerIndex.Four); }
            if (gState.IsButtonUp(Buttons.Start) == true && PrevgState.IsButtonDown(Buttons.Start)) { listOfHeroes.Clear(); }
            PrevgState = gState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Matrix cameraMatrix = Matrix.CreateTranslation(-screen.X, -screen.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraMatrix);
            //spriteBatch.Begin();

            switch (menu)
            {
                case MenuReader.GamePlay: DrawGamePlay(gameTime); break;
                case MenuReader.GameOver: DrawGameOver(gameTime); break;
            }
        

            spriteBatch.End();
            base.Draw(gameTime);
        }

        

        public void DrawGamePlay(GameTime gameTime)
        {
            
            // test
            if (levelNum >= 0) spriteBatch.Draw(sky, new Rectangle(screen.X, screen.Y, 1280, 720), Color.White);
            

            //spriteBatch.Draw(block, h1.HeroPos, Color.White);
            // load each hero's graphics
            if (h1 != null) { h1.LoadGraphics(p1.PlayerSheet, projectileSheet);  }
            if (h2 != null) { h2.LoadGraphics(p2.PlayerSheet, projectileSheet); spriteBatch.DrawString(font, "P2", new Vector2(h2.HeroPos.X, h2.HeroPos.Y - 30), Color.Black); }
            if (h3 != null) { h3.LoadGraphics(p3.PlayerSheet, projectileSheet); spriteBatch.DrawString(font, "P3", new Vector2(h3.HeroPos.X, h3.HeroPos.Y - 30), Color.Black); }
            if (h4 != null) { h4.LoadGraphics(p4.PlayerSheet, projectileSheet); spriteBatch.DrawString(font, "P4", new Vector2(h4.HeroPos.X, h4.HeroPos.Y - 30), Color.Black); }

            // calculate total health of all players
            int totalHealth = 0;
            for (int i = 0; i < listOfHeroes.Count; i++)
            {  
               totalHealth += listOfHeroes[i].Health; 
            }
            
            // creating rectangles length
            for (int i = 0; i < listOfHeroes.Count; i++)
            {
                // fractional version
                //double healthDouble = ((double)listOfHeroes[i].Health / (double)totalHealth) * graphics.PreferredBackBufferWidth - 20;
                // normal version
                double healthDouble = ((double)h1.Health/(double)h1.MaxHealth) * screen.Width; // 100 MAX HEALTH
                if (i == 0) listOfHeroes[i].HealthBar = new Rectangle(screen.X, screen.Y + 8, (int)healthDouble, 25);
                else
                {
                    listOfHeroes[i].HealthBar = new Rectangle(listOfHeroes[i - 1].HealthBar.Right, screen.Y + 8, (int)healthDouble, 25);
                }
                
            }
            // draw the blocks
            foreach (Block b in blocks)
            {
                
                if (b.Damage > 0)
                {
                    //spriteBatch.Draw(block, b.BlockPos, Color.Red);
                    b.Draw(gameTime, spriteBatch, hazardTile);

                }
                else if (b is SpeedBlock)
                {
                    if(b.XSpeed != 0)
                    {
                        b.Draw(gameTime, spriteBatch, conveyorTile);
                        
                    }
                    //spriteBatch.Draw(block, b.BlockPos, Color.Cyan);
                }
                else if (b is AssemblyBlock)
                {
                    spriteBatch.Draw(block, b.BlockPos, Color.Yellow);
                }
                else if (b is MovingBlock)
                {
                    if (b is SwitchBlock)
                    {
                        b.Draw(gameTime, spriteBatch, block);
                    }
                    else
                    {
                        b.Draw(gameTime, spriteBatch, movingTile);
                    }
                }
               
                else
                {
                    //spriteBatch.Draw(block, b.BlockPos, Color.White);
                    b.Draw(gameTime, spriteBatch, groundTile);
                }
            }

            // draw goal
            goal.Draw(gameTime, spriteBatch, treasureBoxTile);

            // draw switch
            /*foreach(Switch s in listOfSwitches)
            {
                s.Draw(gameTime, spriteBatch, block);
            }*/

            // draw each hero
            foreach (Hero h in listOfHeroes)
            {
                //spriteBatch.Draw(block, h.HeroPos, Color.White);
                h.Draw(gameTime, spriteBatch);              
                
                // draw coordinates
                if (drawDebug)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        spriteBatch.DrawString(font, "" + i, new Vector2(i * 32, h.HeroPos.Y), Color.Red); // draw X+
                        spriteBatch.DrawString(font, "" + i * -1, new Vector2(i * -32, h.HeroPos.Y), Color.Red); // draw X-
                        spriteBatch.DrawString(font, "" + i, new Vector2(h.HeroPos.X, i * 32), Color.Green); // draw Y+
                        spriteBatch.DrawString(font, "" + i * -1, new Vector2(h.HeroPos.X, i * -32), Color.Green); // draw Y-
                    }
                }

                // draw inventory              
                spriteBatch.DrawString(font, h.Inventory[0, 0] + ", " + h.Inventory[0, 1], new Vector2(screen.X, screen.Y + 60), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[1, 0] + ", " + h.Inventory[1, 1], new Vector2(screen.X, screen.Y + 80), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[2, 0] + ", " + h.Inventory[2, 1], new Vector2(screen.X, screen.Y + 100), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[3, 0] + ", " + h.Inventory[3, 1], new Vector2(screen.X, screen.Y + 120), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[4, 0] + ", " + h.Inventory[4, 1], new Vector2(screen.X, screen.Y + 140), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[5, 0] + ", " + h.Inventory[5, 1], new Vector2(screen.X, screen.Y + 160), Color.Black);
                spriteBatch.DrawString(font, h.Inventory[6, 0] + ", " + h.Inventory[6, 1], new Vector2(screen.X, screen.Y + 180), Color.Black);

                // draw temp inventory                
                spriteBatch.DrawString(font, h.TempInventory[0, 0] + ", " + h.TempInventory[0, 1], new Vector2(screen.X + 100, screen.Y + 60), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[1, 0] + ", " + h.TempInventory[1, 1], new Vector2(screen.X + 100, screen.Y + 80), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[2, 0] + ", " + h.TempInventory[2, 1], new Vector2(screen.X + 100, screen.Y + 100), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[3, 0] + ", " + h.TempInventory[3, 1], new Vector2(screen.X + 100, screen.Y + 120), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[4, 0] + ", " + h.TempInventory[4, 1], new Vector2(screen.X + 100, screen.Y + 140), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[5, 0] + ", " + h.TempInventory[5, 1], new Vector2(screen.X + 100, screen.Y + 160), Color.Black);
                spriteBatch.DrawString(font, h.TempInventory[6, 0] + ", " + h.TempInventory[6, 1], new Vector2(screen.X + 100, screen.Y + 180), Color.Black);
            }

            

            //if(debug != null) spriteBatch.DrawString(font, debug, new Vector2(0, 0), Color.Orange);

            // enemy
            foreach (Enemy e in listOfEnemies)
            {
                // if it doesn't exist or is dead, don't draw
                if (e != null && e.StateProp != Enemy.State.dead)
                {
                    //spriteBatch.Draw(block, e.EnemyPos, Color.White);
                    if (e is Guy)
                    {
                        if (e is WiseGuy)
                        {
                            e.Draw(wiseGuySprite, gameTime, spriteBatch, Color.White);
                        }
                        else if (e is ToughGuy)
                        {
                            e.Draw(toughGuySprite, gameTime, spriteBatch, Color.White);
                        }
                        else if (e is WiseToughGuy)
                        {
                            e.Draw(guySprite, gameTime, spriteBatch, Color.Purple);
                        }
                        else
                        {
                            e.Draw(guySprite, gameTime, spriteBatch, Color.White);
                        }
                    }
                    
                    if (e is IntelligentEnemy)
                    {
                        e.Draw(guySprite, gameTime, spriteBatch, Color.Black);
                    }

                    if (e is SmallGuy)
                    {
                        e.Draw(smallGuySprite, gameTime, spriteBatch, Color.White);
                    }
                }
            }

            // draw live items
            foreach (Item i in listOfItems)
            {
                i.Draw(gameTime, spriteBatch, itemSpriteSheet);
            }

            // projectiles
            if (h1 != null)
            {
                foreach (Projectile p in h1.ListOfProj)
                {
                    if (!p.MarkedForRemoval)
                    {
                        p.Draw(gameTime, spriteBatch);
                    }
                }
            }
            if (h2 != null)
            {
                foreach (Projectile p in h2.ListOfProj)
                {
                    p.Draw(gameTime, spriteBatch);
                }
            }
            if (h3 != null)
            {
                foreach (Projectile p in h3.ListOfProj)
                {
                    p.Draw(gameTime, spriteBatch);
                }
            }
            if (h4 != null)
            {
                foreach (Projectile p in h4.ListOfProj)
                {
                    p.Draw(gameTime, spriteBatch);
                }
            }

            //draw HUD
            spriteBatch.Draw(healthContainer, new Rectangle(screen.X, screen.Y, graphics.PreferredBackBufferWidth, 60), Color.White);
            // draw health bars
            if (h1 != null) spriteBatch.Draw(healthGraphic, h1.HealthBar, Color.Crimson);
            if (h2 != null) spriteBatch.Draw(healthGraphic, h2.HealthBar, Color.MediumBlue);
            if (h3 != null) spriteBatch.Draw(healthGraphic, h3.HealthBar, Color.Yellow);
            if (h4 != null) spriteBatch.Draw(healthGraphic, h4.HealthBar, Color.Green);

            
        }

        public void DrawGameOver(GameTime gameTime)
        {
            //draw background

            //write winner out
            if (h1 != null) spriteBatch.DrawString(font, "Player 1 wins!", new Vector2(screen.Width / 2, screen.Height / 2), Color.Black);
            if (h2 != null) spriteBatch.DrawString(font, "Player 2 wins!", new Vector2(screen.Width / 2, screen.Height / 2), Color.Black);
            if (h3 != null) spriteBatch.DrawString(font, "Player 3 wins!", new Vector2(screen.Width / 2, screen.Height / 2), Color.Black);
            if (h4 != null) spriteBatch.DrawString(font, "Player 4 wins!", new Vector2(screen.Width / 2, screen.Height / 2), Color.Black);
        }

        public void GenerateRoom()
        {
            checkpointBool = false;

            switch (levelNum)
            {
                /*case 0:
                    blocks = fio.ReadLevel(levelNum);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                    startingPos = new Point(-2 * 32, 0 * 32);
                    checkpoint = new Rectangle(160 * 32, -2 * 32, 64, 64);
                    leftWall = -20 * 32;
                    rightWall = 240 * 32;
                    deathPlane = 25 * 32;
                    topPlane = -20 * 32;
                    goal.GoalPos = new Rectangle(217*32,-7*32,106,64);
                    nextLevel = 1;
                    break;*/
                case 0:
                    blocks = fio.ReadLevel(levelNum);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blocks, listOfHeroes);
                    startingPos = new Point(2 * 32, 6 * 32);
                    checkpoint = new Rectangle(210 * 32, -1 * 32, 64, 64);
                    leftWall = -1 * 32;
                    rightWall = 322 * 32;
                    deathPlane = 25 * 32;
                    topPlane = -30 * 32;
                    goal.GoalPos = new Rectangle(314*32,-15*32,106,64);
                    //goal.GoalPos = new Rectangle(10 * 32, 6 * 32, 106, 64); //test
                    hiddenBoundary.BoundaryPos = new Rectangle(320 * 32, -28 * 32, 2, 4);
                    //hiddenBoundary.BoundaryPos = new Rectangle(319 * 32, -15 * 32, 2, 2); //test
                    hiddenBoundary.NewBoundary = new Point(rightWall + (100 * 32), topPlane);
                    nextLevel = 1;
                    break;
            }
            // set at starting point
            foreach (Hero h in listOfHeroes)
            {
                h.Blocks = blocks;
                h.HeroPos = new Rectangle(startingPos.X, startingPos.Y, h.HeroPos.Width, h.HeroPos.Height);
            }
            ResetItemStack();

        }
        public void ResetItemStack()
        {
            // refilling the stack and emptying the list of items
            while (listOfItems.Count != 0)
            {
                Item tempItem = null;
                foreach (Item i in listOfItems)
                {
                    i.OnBlock = false;
                    i.FinalTimer = 0;
                    itemStack.Push(i);
                    tempItem = i;
                    break;
                }
                listOfItems.Remove(tempItem);
            }
            tempList = new List<Item>();
            for (int i = 0; i < itemStackSize; i++)
            {
                tempList.Add(itemStack.Pop(0));
            }
            for (int i = 0; i < itemStackSize; i++)
            {
                tempList[i].Blocks = blocks;
                //tempList[i].OnBlock = false;
                itemStack.Push(tempList[i]);
            }

        }
    }
}
