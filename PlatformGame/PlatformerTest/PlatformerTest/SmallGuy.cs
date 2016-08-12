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
    class SmallGuy : Enemy
    {
        //attributes
        int originX;

        public SmallGuy(Block[] bls, List<Hero> list, int rangeX_, int originX_)
            : base(bls, list)
        {
            state = State.attack;
            rangeX = rangeX_;
            originX = originX_;
            numOfDrops = 1;
            hSpeed = 4;
            maxHSpeed = 5;
            blocks = bls;
            characters = list;
            damage = 1;
            gravity = 0.39;
            maxVSpeed = 20;
            isJumpable = true;
            acceleration = 0.05;

            //animation stuffs
            frame = 0;
            frameSize = new Point(48, 32);
            currentFrame.X = 0; // upper left corner
            currentFrame.Y = 0; // same
            totalIdleFrames = 0;
            totalWalkFrames = 4;
            totalDyingFrames = 4;
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


            if (Math.Abs(enemyPos.X - originX) >= rangeX)
            {
                hSpeed *= -1;
                enemyPos.X += (int)hSpeed;
            }

            // reverse if collision
            if ((leftCollision && hSpeed < 0) || (rightCollision && hSpeed > 0))
            {
                hSpeed *= -1;
                enemyPos.X += (int)hSpeed;
            }
            else
            {
                finalHSpeed = (int)hSpeed;
            }

            //added for testing
            if (((lECollision && hSpeed < 0) || (rECollision && hSpeed > 0)) && finalVSpeed == 0)
            {
                enemyPos.X -= (int)hSpeed;
                hSpeed *= -1;
            }

            if (originX + (rangeX - 96) < enemyPos.X)
            {
                hSpeed -= acceleration;
            }
            if (originX - (rangeX - 96) > enemyPos.X)
            {
                hSpeed += acceleration;
            }
            
        }

        public override void AnimationUpdate(GameTime gameTime)
        {
            // update the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (state == State.attack)
            {
                currentFrame.Y = frameSize.Y * 0;
                numFrames = totalWalkFrames;
                millisecondsPerFrame = 60;
                if (finalHSpeed < 0) { flip = true; }
                else if (finalHSpeed > 0) { flip = false; }
            }

            //this one will probably be changed
            if (state == State.dying)
            {
                currentFrame.Y = (frameSize.Y * 1);
                numFrames = totalDyingFrames;
                millisecondsPerFrame = 45;
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
            for (int i = 0; i < numOfDrops; i++)
            {
                int random = rng.Next(14, 15); // seed the rng with different values
                random = rng.Next(8);
                Item item;
                if (random <= 5)
                {
                    item = itemStack_.Pop(1); // jelly
                }
                else
                {
                    item = itemStack_.Pop(0); // gold
                }
                
                // set rectangle
                item.ItemPos = new Rectangle(enemyPos.X, enemyPos.Y, 32, 32);
                item.YSpeed = -7.0f;
                item.FinalTimer = 0;

                switch (j)
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
