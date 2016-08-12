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
    class Amibee: Hero
    {
        // attribute
        private int charge;
        public int Charge { get { return charge; } }

        int dash;

        // aqua bomb
        AmibeeAquaBomb ab;

        // special attacking - special to Amibee
        int timer;
        

        // constructor
        public Amibee(Rectangle[] blcks, PlayerIndex p)
            : base(blcks, p)
        {
            // animations
            frame = 0;
            frameSize = new Point(200, 200);
            currentFrame.X = 0;
            currentFrame.Y = 0;
            totalIdleFrames = 4;
            totalJumpFrames = 3;
            totalWalkFrames = 7;
            totalAttackFrames = 3;
            totalSpecialAttackFrames = 3;
            scale = 0.82f;

            // individual
            maxJumps = 2;
            maxJumpSpeed = -12;
            jumpsRemaining = maxJumps;
            blocks = blcks;
            gravity = .39;
            //sideCollision = false;
            maxVSpeed = 20;
            pi = p;
            prevGState = GamePad.GetState(pi); // controller  
            hSpeed = 5;
            isAttacking = false;
            canMove = true;
            HeroPos = new Rectangle(0, 0, 100, 100);
            drawOffset = -18;
            drawOffsetY = -64;
            charge = 0;
            timer = 500;
        }

        //Animation update code
        public override void AnimationUpdate(GameTime gameTime)
        {
            // update the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            //deteermine what animation to display
            if (canMove)
            {
                //idle
                if (finalHSpeed == 0 && canFall == false)
                {
                    numFrames = totalIdleFrames;
                    currentFrame.Y = frameSize.Y * 0;
                    millisecondsPerFrame = 120;

                    //reset the special attack
                    canUse = true;
                }

                //walk
                if (finalHSpeed != 0 && canFall == false)
                {
                    numFrames = totalWalkFrames;
                    currentFrame.Y = frameSize.Y * 1;
                    millisecondsPerFrame = 60;
                    if (finalHSpeed < 0) { flip = true; drawOffset = -44; }
                    else { flip = false; drawOffset = -18; }

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
                currentFrame.Y = frameSize.Y * 4;
                millisecondsPerFrame = 70;
                charge += gameTime.ElapsedGameTime.Milliseconds; // increments charge time, amount of time Amibee lunges
            }
            if (isSpecialAttacking)
            {
                numFrames = totalSpecialAttackFrames;
                currentFrame.Y = frameSize.Y * 3;
                millisecondsPerFrame = 100;
                charge += gameTime.ElapsedGameTime.Milliseconds;
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
                    frame = 0;
                }

                // special attack
                if (frame >= (numFrames) && isSpecialAttacking)
                {
                    frame = 0;                 
                }

                // jump animation
                if (frame >= numFrames && canFall)
                {
                    frame = 0;
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
                    canHit = true;
                }

                // location in the spritesheet of frame to display
                //if (!isAttacking /* && !isSpecialAttacking*/) 
                { currentFrame.X = frameSize.X * frame; }

                //normal attack
                //if (isAttacking) { currentFrame.X = (frameSize.X * 4) + (frameSize.X * frame); }
                


            }
            if (timer < 500)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

            //attack
        public override void Attack()
        {
            if (!isAttacking && !isSpecialAttacking && !isHit)
            {
                if (GamePad.GetState(pi).IsButtonDown(Buttons.A) && !prevGState.IsButtonDown(Buttons.A))
                {                    
                    frame = 0;
                    currentFrame.X = 0;
                    timeSinceLastFrame = 0;
                    canMove = false;                    
                    isAttacking = true;
                    charge = 0; // reset charge time

                    // create projectile
                    AmibeeDash ad;
                    if (!flip) { ad = new AmibeeDash(projectileSheet, blocks, this, 1); }
                    else { ad = new AmibeeDash(projectileSheet, blocks, this, -1); }
                    listOfProj.Add(ad);
                }
                if (GamePad.GetState(pi).IsButtonDown(Buttons.B) && !prevGState.IsButtonDown(Buttons.B) && !canFall && canUse && timer >= 500)
                {
                    frame = 0;
                    currentFrame.X = 0;
                    canMove = false;
                    isSpecialAttacking = true;
                    canUse = false;
                    charge = 0; // reset charge time
                    timer = 0;

                    // create projectile here

                    if (!flip) { ab = new AmibeeAquaBomb(projectileSheet, blocks, this, 1); }
                    else { ab = new AmibeeAquaBomb(projectileSheet, blocks, this, -1); }
                    listOfProj.Add(ab);
                    
                }
            }

            else if (isAttacking)
            {
                if (charge < 50)
                {
                    if (GamePad.GetState(pi).ThumbSticks.Left.X > 0) dash = 12;
                    if (GamePad.GetState(pi).ThumbSticks.Left.X < 0) dash = -12;                                      
                }
                else if (charge < 500)
                {
                   finalHSpeed = dash;
                }
                else if (charge >= 500)
                {

                    canMove = true;
                    isAttacking = false;
                }
            }

            else if (isSpecialAttacking)
            {
                if (GamePad.GetState(pi).IsButtonDown(Buttons.B) && charge < 2001) // if B is being held down, increase size and damage
                {                    
                    ab.ScaleUpdate(charge);
                }
                else
                {
                    if (!flip) { ab.HSpeed = (ab.Direction * 8) - ((charge / 2000) * 3); }
                    else { ab.HSpeed = (ab.Direction * 8) + ((charge / 2000) * 3); }
                    ab.VSpeed = 1;
                    canMove = true;
                    isSpecialAttacking = false;
                }
            }
        }

        // jump speed
        public override int CalculateJump()
        {
            return maxJumpSpeed * jumpsRemaining;
        }        
    }
}
