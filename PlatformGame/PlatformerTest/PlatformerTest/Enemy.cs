using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PlatformerTest
{
    class Enemy
    {
        //attributes
        protected Rectangle enemyPos;
        protected double gravity;
        protected bool canFall;
        protected bool sideCollision;
        protected bool leftCollision;
        protected bool rightCollision;
        protected bool lECollision;
        protected bool rECollision;
        protected double vSpeed;
        protected int finalVSpeed;
        protected int maxVSpeed;
        protected Block[] blocks;
        protected int damage;
        protected int deathPlane;
        protected bool isJumpable;
        protected int numOfDrops;

        // additional attributes
        protected List<Hero> characters;
        protected Hero target;
        public enum State { idle, attack, dying, dead };
        public State state; // public so other objects can get to it
        protected int rangeX;
        protected int rangeY;
        protected int tolerance;
        protected double acceleration;
        protected double hSpeed;
        protected int maxHSpeed;
        protected int finalHSpeed;
        protected Block collidingBlock;

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
        protected int totalDyingFrames;
        protected bool flip;

        // Properties
        public Rectangle EnemyPos { get { return enemyPos; } set { enemyPos = value; } }
        public double Gravity { get { return gravity; } }
        public bool CanFall { get { return canFall; } }        
        public bool SideCollision { get { return sideCollision; } }
        public bool LeftCollision { get { return leftCollision; } }
        public bool RightCollision { get { return rightCollision; } }
        public bool LEftCollision { get { return lECollision; } }
        public bool RECollision { get { return rECollision; } }
        public double VSpeed { get { return vSpeed; } }
        public int FinalVSpeed { get { return finalVSpeed; } }
        public int MaxVSpeed { get { return maxVSpeed; } }        
        public int NumOfBlocks { get { return blocks.Length; } }
        public int Damage { get { return damage; } }
        public int DeathPlane { get { return deathPlane; } }
        public bool IsJumpable { get { return isJumpable;} }

        // additional properties
        public List<Hero> Characters { get { return characters; } set { characters = value; } }
        public Hero Target { get { return target; } }
        public State StateProp { get { return state; } set { state = value; } }
        public int RangeX { get { return rangeX; } }
        public int RangeY { get { return rangeY; } }
        public double Acceleration { get { return acceleration; } }
        public double HSpeed { get { return hSpeed; } }
        public int MaxHSpeed { get { return maxHSpeed; } }
        public int FinalHSpeed { get { return finalHSpeed; } }

        // Constructor
        public Enemy(Block[] bls, List<Hero> list)
        {
            //sideCollision = false;
            deathPlane = 5000;
        }

        // collision with other enemies
        public void CollisionWithEnemies(List<Enemy> le)
        {
            // check for side collisions
            foreach (Enemy e in le)
            {
                if(e != this && e.StateProp != Enemy.State.dead)
                {
                    canFall = true;

                if (finalHSpeed < 0)
                {
                    rECollision = false;

                    if (enemyPos.Y > e.enemyPos.Y || enemyPos.Y + (enemyPos.Height - 1) > e.enemyPos.Y) // checks if the block is worth colliding into the side
                    {
                        if ((enemyPos.X + enemyPos.Width - maxHSpeed < e.enemyPos.X) || (enemyPos.X - maxHSpeed > e.enemyPos.X + e.enemyPos.Width))
                        {
                            lECollision = false;
                        }
                        else if ((enemyPos.Y + (enemyPos.Height - 1) < e.enemyPos.Y) || (enemyPos.Y > e.enemyPos.Y + e.enemyPos.Height))
                        {
                            lECollision = false;
                        }
                        else
                        {
                            lECollision = true;
                            break;
                        }
                    }
                }
                if (finalHSpeed > 0)
                {
                    lECollision = false;

                    if (enemyPos.Y > e.enemyPos.Y || enemyPos.Y + (enemyPos.Height - 1) > e.enemyPos.Y) // checks if the block is worth colliding into the side
                    {
                        if ((enemyPos.X + enemyPos.Width + maxHSpeed < e.enemyPos.X) || (enemyPos.X + maxHSpeed > e.enemyPos.X + e.enemyPos.Width))
                        {
                            rECollision = false;
                        }
                        else if ((enemyPos.Y + (enemyPos.Height - 1) < e.enemyPos.Y) || (enemyPos.Y > e.enemyPos.Y + e.enemyPos.Height))
                        {
                            rECollision = false;
                        }
                        else
                        {
                            rECollision = true;
                            break;
                        }
                    }
                }
                }
                else // prevent stuckness
                {
                    lECollision = false;
                    rECollision = false;
                }
                
            }


            // stop if collision
            if (((lECollision && hSpeed < 0) || (rECollision && hSpeed > 0)) && finalVSpeed == 0)
            {
                //hSpeed = 0;
            }
            else // run towards target
            {
                finalHSpeed = (int)hSpeed;
            }
        }

        // Attack phase
        public virtual void Attack()
        {
            
        }

        // Idle
        public virtual void MoveUpdate()
        {
            
        }

        // Item Drop
        public virtual void DropItem(ItemStack itemStack_, List<Item> listOfItems)
        {

        }

        // Dead
        public void DyingUpdate(ItemStack itemStack_, List<Item> listOfItems)
        {
            hSpeed = 0;
            if (frame >= totalDyingFrames ) 
            {
                DropItem(itemStack_, listOfItems);
                state = State.dead; 
            }
        }

        //passive gravity
        public void GravityUpdate()
        {
            foreach (Block b in blocks)
            {
                if (!(b.BlockPos.X <= enemyPos.X && b.BlockPos.X + b.BlockPos.Width >= enemyPos.X)) //(!(heroPos.Intersects(r)))
                {
                    canFall = true;
                }
            }
            
                foreach (Block b in blocks)
                {
                    //if a block is underfoot
                    if (b.BlockPos.Intersects(enemyPos))
                    {
                        // clip to the top of the block                       
                        if ((b.BlockPos.Top - 5) < enemyPos.Bottom && (enemyPos.Top + (enemyPos.Height/2)) < b.BlockPos.Top)
                        {
                            enemyPos.Y = b.BlockPos.Y - (enemyPos.Height - 1);
                            vSpeed = 0;
                            canFall = false;
                            collidingBlock = b;
                            break;
                        }
                        // moving block
                        if ((enemyPos.Bottom - 10) < b.BlockPos.Top)
                        {
                            hSpeed += b.XSpeed;
                        }
                        // hit block from below
                        if (b.BlockPos.Intersects(enemyPos) && vSpeed < 0 && b.BlockPos.Bottom < enemyPos.Bottom)
                        {
                            vSpeed = 0;
                            break;
                        }
                    }
                    
                }

                //no block close
                if (canFall == true)
                    vSpeed += gravity;
      
        }

        //Animation update code
        public virtual void AnimationUpdate(GameTime gameTime)
        {
            
        }

        // Final Update
        public void Update(GameTime gameTime, List<Enemy> enemyList, ItemStack itemStack_, List<Item> listOfItems)
        {
            GravityUpdate();
            AnimationUpdate(gameTime);
            if(state == State.idle)
                MoveUpdate();
            if (state == State.attack) 
                Attack();
            if (state == State.dying)
                DyingUpdate(itemStack_, listOfItems);
            
            CollisionWithEnemies(enemyList);

            // limits vSpeed
            if (vSpeed < maxVSpeed)
            {
                finalVSpeed = (int)vSpeed;
            }
            else
            {
                vSpeed = maxVSpeed;
                finalVSpeed = (int)vSpeed;
            }


            // limits hSpeed
            if (collidingBlock == null)
            {
                if (hSpeed < maxHSpeed && hSpeed > -maxHSpeed)
                {
                    finalHSpeed = (int)hSpeed;
                }
                else if (hSpeed > maxHSpeed)
                {
                    hSpeed = maxHSpeed;
                    finalHSpeed = (int)hSpeed;
                }

                else
                {
                    hSpeed = -maxHSpeed;
                    finalHSpeed = (int)hSpeed;
                }
            }
            // limits hSpeed for moving block
            else
            {
                if (hSpeed < (maxHSpeed + collidingBlock.XSpeed) && hSpeed > (-maxHSpeed + collidingBlock.XSpeed))
                {
                    finalHSpeed = (int)hSpeed;
                }
                else if (hSpeed > (maxHSpeed + collidingBlock.XSpeed))
                {
                    hSpeed = (maxHSpeed + collidingBlock.XSpeed);
                    finalHSpeed = (int)hSpeed;
                }

                else
                {
                    hSpeed = (-maxHSpeed + collidingBlock.XSpeed);
                    finalHSpeed = (int)hSpeed;
                }
            }
           
            



            enemyPos.X += finalHSpeed;
            enemyPos.Y += finalVSpeed;

            // check deathplane
            if(enemyPos.Y >= deathPlane)
            {                
                state = State.dead;
            }

            

        }

        // draw the image
        // called between spriteBatch.Begin and End in Game1.cs
        public void Draw(Texture2D sheet, GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            // draw the selected frame
            if(flip == false)
            spriteBatch.Draw(sheet, new Vector2(enemyPos.X, enemyPos.Y),
                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            else
                spriteBatch.Draw(sheet, new Vector2(enemyPos.X, enemyPos.Y),
                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                color, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
