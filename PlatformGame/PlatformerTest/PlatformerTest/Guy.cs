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
    class Guy : Enemy
    {
        public Guy(Block[] bls, List<Hero> list)
            : base(bls, list)
        {
            //animation stuffs
            frame = 0;
            frameSize = new Point(63, 46);
            currentFrame.X = 0; // upper left corner
            currentFrame.Y = 0; // same
            totalIdleFrames = 4;
            totalWalkFrames = 6;
            totalDyingFrames = 7;

            //enemyPos = new Rectangle(800, 0, 62, 46);
            characters = list;
            gravity = .39;
            sideCollision = false;
            acceleration = 0.3;
            maxVSpeed = 20;
            maxHSpeed = 4;
            state = State.idle;
            rangeX = 300;
            rangeY = 400;
            tolerance = 200;
            blocks = bls;
            vSpeed = 0;
            damage = 1;
            isJumpable = true;
            numOfDrops = 2;
        }
        public override void Attack()
        {
            // check for side collisions
            foreach (Block b in blocks)
            {
                canFall = true;
                    if (finalHSpeed < 0)
                    {
                        rightCollision = false;

                        if (enemyPos.Y > b.BlockPos.Y || enemyPos.Y + (enemyPos.Height - 1) > b.BlockPos.Y) // checks if the block is worth colliding into the side
                        {
                            if ((enemyPos.X + enemyPos.Width - maxHSpeed < b.BlockPos.X) || (enemyPos.X - maxHSpeed > b.BlockPos.X + b.BlockPos.Width))
                            {
                                leftCollision = false;
                            }
                            else if ((enemyPos.Y + (enemyPos.Height - 1) < b.BlockPos.Y) || (enemyPos.Y > b.BlockPos.Y + b.BlockPos.Height))
                            {
                                leftCollision = false;
                            }
                            else
                            {
                                leftCollision = true;
                                break;
                            }
                        }
                    }
                    if (finalHSpeed > 0)
                    {
                        leftCollision = false;

                        if (enemyPos.Y > b.BlockPos.Y || enemyPos.Y + (enemyPos.Height - 1) > b.BlockPos.Y) // checks if the block is worth colliding into the side
                        {
                            if ((enemyPos.X + enemyPos.Width + maxHSpeed < b.BlockPos.X) || (enemyPos.X + maxHSpeed > b.BlockPos.X + b.BlockPos.Width))
                            {
                                rightCollision = false;
                            }
                            else if ((enemyPos.Y + (enemyPos.Height - 1) < b.BlockPos.Y) || (enemyPos.Y > b.BlockPos.Y + b.BlockPos.Height))
                            {
                                rightCollision = false;
                            }
                            else
                            {
                                rightCollision = true;
                                break;
                            }
                        }
                    }
            }


            if (target.HeroPos.X < enemyPos.X)
            {
                hSpeed -= acceleration;
            }
            if (target.HeroPos.X > enemyPos.X)
            {
                hSpeed += acceleration;
            }
            
            // stop if collision
            if((leftCollision && hSpeed < 0) || (rightCollision && hSpeed > 0))
            {
                hSpeed = 0;
            }
            else // run towards target
            {
                finalHSpeed = (int)hSpeed;
            }

            for (int i = 0; i < characters.Count; i++)
            {
                if (!(Math.Abs(characters[i].HeroPos.X - enemyPos.X) <= rangeX && Math.Abs(characters[i].HeroPos.Y - enemyPos.Y) <= rangeY))
                {
                    target = null; // set target
                    state = State.idle; // change state
                }
            }

            //added for testing
            if (((lECollision && hSpeed < 0) || (rECollision && hSpeed > 0)) && finalVSpeed == 0)
            {
                enemyPos.X -= (int)hSpeed; // prevents clipping
                hSpeed = 0;
            }

        }
        public override void MoveUpdate()
        {
            //hSpeed = 0;
            if(collidingBlock != null)
            {
                hSpeed = collidingBlock.XSpeed;
            }

            // check for characters
            for (int i = 0; i < characters.Count; i++)
            {
                //if ((characters[i].HeroPos.X + range) == enemyPos.X || (characters[i].HeroPos.X - range) == enemyPos.X) // within range from left or right
                if (Math.Abs(characters[i].HeroPos.X - enemyPos.X) <= rangeX && Math.Abs(characters[i].HeroPos.Y - enemyPos.Y) <= rangeY)
                {
                    target = characters[i]; // set target
                    rangeY = 400; // normal
                    rangeX = 600; // double
                    state = State.attack; // change state
                    break;
                }
                else
                {
                    target = null;
                    rangeY = 200; // half of normal
                    rangeX = 300; // normal
                    state = State.idle;
                }
            }
        }
        public override void AnimationUpdate(GameTime gameTime)
        {
            // update the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            //deteermine what animation to display
            if (state == State.idle)
            {
                currentFrame.Y = frameSize.Y * 1;
                numFrames = totalIdleFrames;
                millisecondsPerFrame = 70;
            }

            if (state == State.attack)
            {
                currentFrame.Y = frameSize.Y * 0;
                numFrames = totalWalkFrames;
                if (Math.Abs(finalHSpeed + hSpeed) == 0)
                {
                    millisecondsPerFrame = 60;
                }
                else
                {
                    millisecondsPerFrame = (int)(180 / Math.Abs(finalHSpeed + hSpeed)); // to account for acceleration           
                }
                if (finalHSpeed < 0) { flip = true; }
                else if (finalHSpeed > 0) { flip = false; }
            }

            //this one will probably be changed
            if (state == State.dying)
            {
                currentFrame.Y = (frameSize.Y * 2) + 1; // +1 to stop line of pixels
                numFrames = totalDyingFrames;
                millisecondsPerFrame = 30;
            }

            // move to next frame if the elapsedtime is long enough
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                // set up to do a new image
                timeSinceLastFrame = 0;
                frame++;
                if (frame >= numFrames && state != State.dying)  // wrap around to starting image unless dying
                {
                    frame = 0;
                }

                // location in the spritesheet of frame to display
                currentFrame.X = frameSize.X * frame;
            }
        }

        public override void DropItem(ItemStack itemStack_, List<Item> listOfItems)
        {
            int j = 0;
            // get random number
            Random rng = new Random();
            for(int i = 0; i < numOfDrops; i++)
            {
                int random = rng.Next(14, 15); // seed the rng with different values
                random = rng.Next(13);
                Item item;
                if(random <= 7)
                {                  
                        item = itemStack_.Pop(1); // jelly
                }
                else if(random >= 8 && random <= 11)
                {
                        item = itemStack_.Pop(0); // gold
                }
                else
                {
                        item = itemStack_.Pop(2); // eye
                }
                 
                
                
                // set rectangle
                item.ItemPos = new Rectangle(enemyPos.X, enemyPos.Y, 32, 32);
                item.YSpeed = -7.0f;
                item.FinalTimer = 0;

                switch(j)
                {
                    case 0:
                        item.XSpeed = 3.0f;                       
                        break;
                    case 1:
                        item.XSpeed = -3.0f;
                        break;
                    case 2:
                        item.XSpeed = 0.0f;
                        break;

                }

                listOfItems.Add(item);
                j++;
            }
        }
    }
}
