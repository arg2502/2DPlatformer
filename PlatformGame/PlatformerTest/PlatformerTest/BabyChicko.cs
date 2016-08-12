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
    class BabyChicko: Hero
    {
        // constructor
        public BabyChicko(Block[] blcks, PlayerIndex p)
            : base(blcks, p)
        {
            //animation stuffs

            frame = 0;
            frameSize = new Point(100, 100);
            currentFrame.X = 0; // upper left corner
            currentFrame.Y = 0; // same
            totalIdleFrames = 4;
            totalJumpFrames = 4;
            totalWalkFrames = 4;
            totalAttackFrames = 4;
            totalSpecialAttackFrames = 3;

            maxJumps = 2;
            maxJumpSpeed = -12;
            jumpsRemaining = maxJumps;
            blocks = blcks;
            gravity = .35;
            leftCollision = false;
            rightCollision = false;
            maxVSpeed = 20;
            pi = p;
            prevGState = GamePad.GetState(pi); // controller  
            hSpeed = 5;
            isAttacking = false;
            canMove = true;
            HeroPos = new Rectangle(0, 0, 50, 100);
            drawOffset = -18;

            maxProj = 3;
            numOfProj = 0;
        }

        //Animation update code
        public override void AnimationUpdate(GameTime gameTime)
        {
            // update the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            //determine what animation to display
            if (canMove)
            {
                //idle
                if ((!(Keyboard.GetState().IsKeyDown(Keys.A)) && !(Keyboard.GetState().IsKeyDown(Keys.D)) || (GamePad.GetState(pi).ThumbSticks.Left.X == 0))
                    && canFall == false)
                {
                    numFrames = totalIdleFrames;
                    currentFrame.Y = frameSize.Y * 0;
                    millisecondsPerFrame = 120;

                    //reset the special attack
                    canUse = true;
                }

                //walk
                if (((Keyboard.GetState().IsKeyDown(Keys.A)) && !(Keyboard.GetState().IsKeyDown(Keys.D))
                    || (Keyboard.GetState().IsKeyDown(Keys.D)) && !(Keyboard.GetState().IsKeyDown(Keys.A))
                    || !(GamePad.GetState(pi).ThumbSticks.Left.X == 0))
                    && canFall == false && finalHSpeed != 0)
                {
                    numFrames = totalWalkFrames;
                    currentFrame.Y = frameSize.Y * 1;
                    millisecondsPerFrame = 60;
                    if (finalHSpeed < 0) { flip = true; }
                    else if(finalHSpeed > 0){ flip = false; }

                    //reset the special attack
                    canUse = true;
                }

                //jump or fall
                if (canFall == true)
                {
                    numFrames = totalJumpFrames;
                    currentFrame.Y = frameSize.Y * 2;
                    if (finalVSpeed > 0)
                    {
                        millisecondsPerFrame = 30;
                    }
                    else
                    {
                        millisecondsPerFrame = 60;
                    }

                    //reset the special attack
                    canUse = true;
                }
            }
            if (isAttacking)
            {
                numFrames = totalAttackFrames;
                currentFrame.Y = frameSize.Y * 3;
                millisecondsPerFrame = 70;
            }
            if (isSpecialAttacking)
            {
                numFrames = totalSpecialAttackFrames;
                currentFrame.Y = frameSize.Y * 4;
                millisecondsPerFrame = 80; //original 100
            }
            if (isHit)
            {
                frame = 0;
                currentFrame.X = 0;

                // set everything to false
                canMove = false;
                canUse = false;
                canJump = false;
                isAttacking = false;
                isSpecialAttacking = false;

                numFrames = 1;
                currentFrame.Y = frameSize.Y * 5;
                millisecondsPerFrame = hitStun;

            }

            // move to next frame if the elapsedtime is long enough
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                // set up to do a new image
                timeSinceLastFrame = 0;
                frame++;


                // normal attack
                if (frame >= (numFrames) && isAttacking)
                {
                    isAttacking = false;
                    canMove = true;
                }

                // special attack
                if (frame >= (numFrames) && isSpecialAttacking)
                {
                    isSpecialAttacking = false;
                    canMove = true;
                }

                // jump animation
                if (frame >= numFrames && canFall)
                {
                    frame = numFrames - 1;
                }

                // normal animation
                if (frame >= numFrames && !isAttacking && !isSpecialAttacking && !isHit)  // wrap around to starting image
                {
                    frame = 0;
                }

                
                // hit
                if (frame >= numFrames && isHit)
                {
                    isHit = false;
                    canMove = true;
                    canUse = true;
                    canJump = true;
                }

                // location in the spritesheet of frame to display
                { currentFrame.X = frameSize.X * frame; }

                //normal attack
                //if (isAttacking) { currentFrame.X = (frameSize.X * 4) + (frameSize.X * frame); }

                
            }
        }

        //attack
        /*public override void Attack()
        {
            if (!isAttacking && !isSpecialAttacking && !isHit)
            {
                if (GamePad.GetState(pi).IsButtonDown(Buttons.A) && !prevGState.IsButtonDown(Buttons.A) && canFall == false)
                {                    
                    frame = 0;
                    currentFrame.X = 0;
                    timeSinceLastFrame = 0;
                    canMove = false;
                    isAttacking = true;
                }
                //move and punch
                if (GamePad.GetState(pi).IsButtonDown(Buttons.A) && !prevGState.IsButtonDown(Buttons.A) && canFall == false 
                    && Math.Abs(GamePad.GetState(pi).ThumbSticks.Left.X) > Math.Abs(prevGState.ThumbSticks.Left.X))
                {
                    frame = 0;
                    currentFrame.X = 0;
                    timeSinceLastFrame = 0;
                    //canMove = false;
                    canMove = true;
                    isAttacking = true;
                }
                if ((GamePad.GetState(pi).IsButtonDown(Buttons.B) && !prevGState.IsButtonDown(Buttons.B) && canUse)
                    || (Keyboard.GetState().IsKeyDown(Keys.Space) && !prevKState.IsKeyDown(Keys.Space) && canUse))
                {
                    frame = 0;
                    currentFrame.X = 0;
                    //canMove = false; //Dom & John suggestion to move and shoot
                    isSpecialAttacking = true;
                    //canUse = false;
                }
            }

            else if (isAttacking)
            {
                if (frame == totalAttackFrames - 1 && canUse)
                {
                    //make a projectile
                    BabyChickaPunch bcp;
                    BabyChickaPunchEffect bcpe;
                    if (!flip)
                    {
                        //bcp = new BabyChickaPunch(projectileSheet, blocks, this, 25);
                        //bcpe = new BabyChickaPunchEffect(projectileSheet, blocks, this, 25);
                    }
                    else
                    {
                        //bcp = new BabyChickaPunch(projectileSheet, blocks, this, -15);
                        //bcpe = new BabyChickaPunchEffect(projectileSheet, blocks, this, -15);
                    }

                    // call list of heroes
                    listOfProj.Add(bcp); // add to list of projectiles
                    listOfProj.Add(bcpe);
                    if (canMove == true)// multiple projectiles are created when moving for some reason
                    {
                        frame++;
                        canMove = false;
                    }
                    canUse = false;
                }
            }

            else if (isSpecialAttacking)
            {
                if (frame == (totalSpecialAttackFrames - 1) && canUse && numOfProj < maxProj)
                {
                    //make a projectile
                    BabyChickaSuperPeck sp;
                    if (!flip) sp = new BabyChickaSuperPeck(projectileSheet, blocks, this, 1);
                    else sp = new BabyChickaSuperPeck(projectileSheet, blocks, this, -1);
                    numOfProj++;

                    // call list of heroes

                    listOfProj.Add(sp); // add to list of projectiles
                    canUse = false;
                    frame++; // required if we want bc to move and shoot
                }
            }
        }*/

        // jump speed
        public override int CalculateJump()
        {
            return maxJumpSpeed * jumpsRemaining;
        }
    }
}
