using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PlatformerTest
{
    class Hero
    {
        // GamePad states
        protected PlayerIndex pi;
        protected GamePadState prevGState;
        protected KeyboardState prevKState;
        

        //attributes
        protected Rectangle heroPos;
        protected double gravity;
        protected bool canFall;
        protected bool isJumping;
        protected bool leftCollision;
        protected bool rightCollision;
        protected double vSpeed;
        protected int finalVSpeed;
        protected int maxVSpeed;
        protected int jumpSpeed;
        protected int maxJumps;
        protected int maxJumpSpeed;
        protected bool canJump;
        protected int jumpsRemaining;
        protected Block[] blocks;
        protected int hSpeed;
        protected int finalHSpeed;
        protected bool isAttacking;
        protected bool isSpecialAttacking;
        protected bool canMove;
        protected bool canUse;
        protected List<Projectile> listOfProj;
        protected int health;
        protected Rectangle healthBar;
        protected int maxHealth;
        protected bool isHit;
        protected int hitStun;
        protected bool canHit;
        protected int heroKnockBack;
        protected int startInvincibilityTime;
        protected int invinciblityTime;

        //inventory 2d array
        int[,] inventory;
        int[,] tempInventory;

        //nerf attributes
        public int maxProj;
        public int numOfProj;

        //animation attributes
        protected Texture2D spriteSheet;
        protected int frame; // the frame to display
        protected Point frameSize;
        protected int numFrames;// the amount of frames in animation
        protected int timeSinceLastFrame; // in milliseconds
        protected int millisecondsPerFrame;
        protected Point currentFrame; // upper left corner of the frame
        protected int totalWalkFrames;
        protected int totalIdleFrames;
        protected int totalAttackFrames;
        protected int totalSpecialAttackFrames;
        protected int totalJumpFrames;
        protected bool flip;
        protected Texture2D projectileSheet;
        protected int drawOffset;
        protected int drawOffsetY;
        protected float scale;

        // Properties
        public Rectangle HeroPos { get { return heroPos; } set { heroPos = value; } }
        public double Gravity { get { return gravity; } }
        public bool CanFall { get { return canFall; } set { canFall = value; } }
        public bool IsJumping { get { return isJumping; } }
        public bool LeftCollision { get { return leftCollision; } }
        public bool RightCollision { get { return rightCollision; } }
        public double VSpeed { get { return vSpeed; } }
        public int FinalVSpeed { get { return finalVSpeed; } }
        public int MaxVSpeed { get { return maxVSpeed; } }
        public int FinalHSpeed { get { return finalHSpeed; } set { finalHSpeed = value; } }
        public int JumpSpeed { get { return jumpSpeed; } }
        public int MaxJumps { get { return maxJumps; } }
        public int MaxJumpSpeed { get { return maxJumpSpeed; } }
        public bool CanJump { get { return canJump; } }
        public int JumpsRemaining { get { return jumpsRemaining; } }
        public int NumOfBlocks { get { return blocks.Length; } }
        public List<Projectile> ListOfProj { get { return listOfProj; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public Rectangle HealthBar { get { return healthBar; } set { healthBar = value; } }
        public bool IsHit { get { return isHit; } set { isHit = value; } }
        public int HitStun { get { return hitStun; } set { hitStun = value; } }
        public bool CanHit { get { return canHit; } set { canHit = value; } }
        public bool IsSpecialAttacking { get { return isSpecialAttacking; } set { isSpecialAttacking = value; } }
        public int HeroKnockBack { set { heroKnockBack = value; } }
        public int[,] Inventory { get { return inventory; } }
        public int[,] TempInventory { get { return tempInventory; } set { tempInventory = value; } }
        public Block[] Blocks { set { blocks = value; } }



        public Hero(Block[] blcks, PlayerIndex p)
        {
            if (p == PlayerIndex.One) heroPos = new Rectangle(0, 0, 65, 95);
            if (p == PlayerIndex.Two) heroPos = new Rectangle(320, 0, 75, 100);
            if (p == PlayerIndex.Three) heroPos = new Rectangle(640, 0, 75, 100);
            if (p == PlayerIndex.Four) heroPos = new Rectangle(960, 0, 75, 100);

            maxJumps = 5;
            maxJumpSpeed = -5;
            jumpsRemaining = maxJumps;
            blocks = blcks;
            gravity = .39;
            leftCollision = false;
            rightCollision = false;
            maxVSpeed = 20;
            pi = p;
            prevGState = GamePad.GetState(pi); // controller     
            prevKState = Keyboard.GetState(); 
            listOfProj = new List<Projectile>();
            maxHealth = 10;
            health = maxHealth; // set initial health
            canHit = true;
            invinciblityTime = 1500;
            
            //animation stuffs
            frame = 0;
            frameSize = new Point(86, 100);
            currentFrame.X = 0; // upper left corner
            currentFrame.Y = 0; // same
            totalIdleFrames = 4;
            totalWalkFrames = 6;
            totalAttackFrames = 7;
            scale = 1f;

            inventory = new int[40, 2]; // up to 40 different types of items possible, with an additional value to store quantity
            for (int i = 0; i < (inventory.Length/2); i++)
            {
                inventory[i, 0] = -1;
            }
            tempInventory = new int[40, 2];
            for (int i = 0; i < (tempInventory.Length / 2); i++)
            {
                tempInventory[i, 0] = -1;
            }
        }

        //load graphics
        public void LoadGraphics(Texture2D sheet, Texture2D projSht)
        {
            spriteSheet = sheet;
            projectileSheet = projSht;
        }


        //passive gravity
        public void GravityUpdate()
        {
            //if(canFall == true)            
            //{
                foreach (Block b in blocks)
                {
                    
                    if (b.BlockPos.Intersects(heroPos))
                    {
                        // check for hazards
                        if (b.Damage > 0 && canHit)
                        {
                            Damage(b.Damage);
                        }

                        //if a block is underfoot
                        if (isJumping == false)
                        {
                            // moving block
                            if ((heroPos.Bottom - 10) < b.BlockPos.Top)
                            {
                                if ((b is MovingBlock || b is SpeedBlock) && !leftCollision && !rightCollision)
                                {
                                    finalHSpeed += b.XSpeed;
                                    finalVSpeed += b.YSpeed;
                                    heroPos.Y += finalVSpeed;
                                    
                                    // this is for springs
                                    if(b is SpeedBlock && b.YSpeed > 0)
                                    {
                                        jumpsRemaining = maxJumps;
                                        canJump = true;
                                        jumpSpeed = b.YSpeed;
                                        vSpeed = 0;
                                        isJumping = true;
                                        canFall = true;

                                    }
                                    // check again while on moving platforms
                                    if (finalHSpeed > 0)
                                    {
                                        RightCollisionCheck();
                                    }
                                    if (finalHSpeed < 0)
                                    {
                                        LeftCollisionCheck();
                                    }
                                    if (rightCollision || leftCollision)
                                    {
                                        finalHSpeed = 0;
                                        canFall = true;
                                    }
                                    else
                                    {
                                        heroPos.X += finalHSpeed;
                                    }
                                }
                            }
                            if ((b.BlockPos.Top + 5) < heroPos.Bottom && (heroPos.Top + 50) < b.BlockPos.Top)
                            {
                                heroPos.Y = b.BlockPos.Y - (heroPos.Height - 1);
                                vSpeed = 0;
                                canFall = false;
                                jumpsRemaining = maxJumps;
                                canJump = true;
                                break;
                            }
                        }
                        
                    }                   
                    //removed to allow jumpthru platforms -- we can put this back at any time
                    if (b.BlockPos.Intersects(heroPos) && isJumping == true && b.BlockPos.Bottom < heroPos.Bottom && b.IsJumpThru == false)
                    {
                        jumpSpeed = 0;

                        // prevents stopping when collide from bottom
                        leftCollision = false;
                        rightCollision = false;
                        break;
                    }


                   
                }
                //no block close
                if(canFall == true)
                vSpeed += gravity;
                
        }

        //player input method
        public void MoveUpdate()
        {
            if (canMove)
            {
                //jumping
                if ((GamePad.GetState(pi).IsButtonDown(Buttons.Y) && canJump == true && !prevGState.IsButtonDown(Buttons.Y))
                    || (Keyboard.GetState().IsKeyDown(Keys.W) && canJump == true && !prevKState.IsKeyDown(Keys.W)))
                {
                    jumpSpeed = CalculateJump();
                    vSpeed = 0;
                    isJumping = true;
                    canFall = true;
                    jumpsRemaining--;
                    if (jumpsRemaining == 0)
                        canJump = false;
                }
                // move right
                if ((GamePad.GetState(pi).ThumbSticks.Left.X > 0) || (Keyboard.GetState().IsKeyDown(Keys.D)))
                {
                    canFall = true;

                    RightCollisionCheck();
                    if (rightCollision) 
                    {
                        finalHSpeed = 0;
                                               
                    }
                    else 
                    { 
                        finalHSpeed = hSpeed; 
                    }
                    heroPos.X += finalHSpeed;


                }

                // move left
                if ((GamePad.GetState(pi).ThumbSticks.Left.X < 0) || (Keyboard.GetState().IsKeyDown(Keys.A)))
                {
                    canFall = true;

                    LeftCollisionCheck();
                    if (leftCollision) 
                    {
                        finalHSpeed = 0;
                    }
                    else 
                    { 
                        finalHSpeed = -hSpeed; 
                    }
                    heroPos.X += finalHSpeed;
                }

                // reset animation when stop walking
                if (GamePad.GetState(pi).ThumbSticks.Left.X == 0 && prevGState.ThumbSticks.Left.X != 0)
                {
                    frame = 0;
                    currentFrame.X = 0;
                    timeSinceLastFrame = 0;
                }

                // set canfall back to false to prevent platform clipping
                foreach (Block b in blocks)
                {
                    //if a block is underfoot
                    if (b.BlockPos.Intersects(heroPos))
                    {
                        if (isJumping == false && b.BlockPos.Bottom > heroPos.Bottom)
                        {
                            canFall = false;
                            break;
                        }
                    }
                    else
                    {
                        canFall = true;
                    }
                }
            }

            else if (isHit)
            {
                finalHSpeed = 0;
            }

        }

        // collision check methods
        public void LeftCollisionCheck()
        {
            foreach (Block b in blocks)
            {
                // ignore method if jumpthru
                if(b.IsJumpThru == true && heroPos.Intersects(b.BlockPos))
                {
                    break;
                }
                // prevents stopping when collide from bottom
                if (b.BlockPos.Intersects(heroPos) && isJumping == true && b.BlockPos.Bottom < heroPos.Bottom && b.IsJumpThru == false)
                {
                    jumpSpeed = 0;                   
                    leftCollision = false;
                    rightCollision = false;
                    break;
                }

                // AABB
               
                if (heroPos.Y > b.BlockPos.Y || heroPos.Y + (heroPos.Height - 1) > b.BlockPos.Y) // checks if the block is worth colliding into the side
                {
                    if ((heroPos.X + heroPos.Width - hSpeed < b.BlockPos.X) || (heroPos.X - hSpeed > b.BlockPos.X + b.BlockPos.Width))
                    {
                        leftCollision = false;
                    }
                    else if ((heroPos.Y + (heroPos.Height - 1) < b.BlockPos.Y) || (heroPos.Y > b.BlockPos.Y + b.BlockPos.Height))
                    {
                        leftCollision = false;
                    }
                    else
                    {
                        leftCollision = true;
                        // check for hazards
                        if (b.Damage > 0 && canHit)
                        {
                            Damage(b.Damage);
                        }
                        break;
                    }
                }

              
            }
        }
        public void RightCollisionCheck()
        {
            foreach (Block b in blocks)
            {                
                // ignore method if jumpthru
                if (b.IsJumpThru == true && heroPos.Intersects(b.BlockPos))
                {
                    break;
                }
                // prevents stopping when collide from bottom
                if (b.BlockPos.Intersects(heroPos) && isJumping == true && b.BlockPos.Bottom < heroPos.Bottom && b.IsJumpThru == false)
                {
                    jumpSpeed = 0;
                    leftCollision = false;
                    rightCollision = false;
                    break;
                }
           
                if (heroPos.Y > b.BlockPos.Y || heroPos.Y + (heroPos.Height - 1) > b.BlockPos.Y) // checks if the block is worth colliding into the side
                {
                    if ((heroPos.X + heroPos.Width + hSpeed < b.BlockPos.X) || (heroPos.X + hSpeed > b.BlockPos.X + b.BlockPos.Width))
                    {
                        rightCollision = false;
                    }
                    else if ((heroPos.Y + (heroPos.Height - 1) < b.BlockPos.Y) || (heroPos.Y > b.BlockPos.Y + b.BlockPos.Height))
                    {
                        rightCollision = false;
                    }
                    else
                    {
                        rightCollision = true;
                        // check for hazards
                        if (b.Damage > 0 && canHit)
                        {
                            Damage(b.Damage);
                        }
                        break;
                    }
                }
                
            }
        }


        // jump on enemy
        public void KillEnemy(List<Enemy> en, GameTime gameTime)
        {
            foreach (Enemy e in en)
            {   
                // if the enemies are not dead
                if (e.StateProp == Enemy.State.idle || e.StateProp == Enemy.State.attack)
                {
                    // check if inside them in X
                    if(e.EnemyPos.Intersects(heroPos))
                    {
                        // jump on them
                        if ((heroPos.Bottom + vSpeed) >= e.EnemyPos.Top && heroPos.Top < e.EnemyPos.Top && finalVSpeed > 0)
                        {
                            // kill them
                            if (e.IsJumpable)
                            {
                                e.StateProp = Enemy.State.dying;
                            }

                            // bounce off most enemies
                            if((e is Guy) && (canMove))
                            {
                                //jumpsRemaining = maxJumps;
                                canJump = true;
                                jumpSpeed = -15;
                                vSpeed = 0;
                                isJumping = true;
                                canFall = true;
                                
                                if(!(e.IsJumpable))
                                {
                                    Damage(e.Damage);
                                }
                            }
                        }
                        // else you get hurt if colliding in Y
                        else
                        {
                            if (canHit)
                            {
                                Damage(e.Damage);
                            }
                        }
                    }
                }

            }
        }

        // damage method
        public void Damage(int dam)
        {
            health -= dam;
            isHit = true;
            canHit = false;
            hitStun = 300;

            // start timer
            startInvincibilityTime = 0;
        }

        //Animation update code
        public virtual void AnimationUpdate(GameTime gameTime)
        {
           
        }

        //attack
        public virtual void Attack()
        {
            
        }

        //special attack
        public virtual void SpecialAttack()
        {
        }

        // calculate individual jump speed
        public virtual int CalculateJump()
        {
            return 0;
        }
        
        // pick up item
        public void GetItem(List<Item> listOfItems, ItemStack itemStack_, GameTime gameTime)
        {
            Item tempItem = null;
            foreach(Item i in listOfItems)
            {
                // update item timer and check if expired
                i.FinalTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (i.FinalTimer >= 4000)
                {
                    itemStack_.Push(i);
                    tempItem = i;
                    i.OnBlock = false;
                    i.FinalTimer = 0;
                    break;
                }

                //if not expired, add to your  temporary inventory
                if(heroPos.Intersects(i.ItemPos) && i.OnBlock)
                {
                    // add to temp inventory here
                    for (int j = 0; j < (tempInventory.Length/2); j++)
                    {
                        // there is already an item of this type
                        if(i.Type == tempInventory[j,0])
                        {
                            tempInventory[j, 1] += 1; // increase the quantity of this item
                            break;
                        }
                        //there is not already an item of this type
                        else if(tempInventory[j,0] == -1)
                        {
                            tempInventory[j, 0] = i.Type; // assign this item to the first open space
                            tempInventory[j, 1] = 1; //there is now one item of this type at position j 
                            break;
                        }
                    }
                    itemStack_.Push(i);
                    tempItem = i;
                    i.OnBlock = false;
                    break;
                }
            }
            if (tempItem != null)
            {
                tempItem.OnBlock = false;
                listOfItems.Remove(tempItem);
            }
        }
        // convert temp inventory to inventory
        public void SetInventory()
        {
            for(int i = 0; i < tempInventory.Length/2; i++)
            {
                // only check filled slots
                if(tempInventory[i,0] != -1)
                {
                    // loop through inventory to find appropriate slot
                    for(int j = 0; j < inventory.Length/2; j++)
                    {
                        // you have the item, add to the slot
                        if(inventory[j,0] == tempInventory[i,0])
                        {
                            inventory[j, 1] += tempInventory[i, 1];
                            break;
                        }
                        // you don't have the item, assign type to slot and add to it
                        else if(inventory[j,0] == -1)
                        {
                            inventory[j, 0] = tempInventory[i, 0];
                            inventory[j, 1] += tempInventory[i, 1];
                            break;
                        }
                    }
                }
                // break when hit -1. rest of list is empty
                else if(tempInventory[i,0] == -1)
                {
                    break;
                }
            }

            // reset temp inventory
            tempInventory = new int[40, 2];
            for(int i = 0; i < tempInventory.Length/2; i++)
            {
                tempInventory[i, 0] = -1;
            }
        }

        //update player pos
        public void Update(Block[] blocks, KeyboardState current, KeyboardState previous, List<Enemy> le, GameTime gameTime, List<Item> listOfItems, ItemStack itemStack_)
        {
            //canFall = true;
            leftCollision = false;
            rightCollision = false;
            Attack();
            GravityUpdate();
            MoveUpdate();
            AnimationUpdate(gameTime);
            KillEnemy(le, gameTime);
            GetItem(listOfItems, itemStack_, gameTime);

            // check if canhit
            startInvincibilityTime += gameTime.ElapsedGameTime.Milliseconds;
                                
            if(startInvincibilityTime >= invinciblityTime)
            {
                canHit = true;
            }

            if (isJumping == true)
            {
                if (jumpSpeed > -1) { isJumping = false; }
                else
                {
                    finalVSpeed = (int)vSpeed + jumpSpeed;
                    jumpSpeed += 1;
                }
            }
            else
            {
                if (vSpeed < maxVSpeed)
                {
                    finalVSpeed = (int)vSpeed;
                }
                else
                {
                    vSpeed = maxVSpeed;
                    finalVSpeed = (int)vSpeed;
                }
            }
            heroPos.Y += finalVSpeed;
            
            prevGState = GamePad.GetState(pi);
            prevKState = Keyboard.GetState();
            finalHSpeed = 0;

        }

        
        // draw the image
        // called between spriteBatch.Begin and End in Game1.cs
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw the selected frame
            if (flip == false)
                spriteBatch.Draw(spriteSheet, new Vector2(heroPos.X + drawOffset, heroPos.Y + drawOffsetY),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            else
                spriteBatch.Draw(spriteSheet, new Vector2(heroPos.X + drawOffset, heroPos.Y + drawOffsetY),
                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
        }


    }
}
