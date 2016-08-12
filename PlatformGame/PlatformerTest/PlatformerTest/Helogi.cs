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
    class Helogi : Hero
    {        
        //additional attributes
        List<HelogiFire> reserveFire;
        public Helogi(Rectangle[] blcks, PlayerIndex p)
            : base(blcks, p)
        {
            //animation stuffs
            
            frame = 0;
            frameSize = new Point(87, 100);
            currentFrame.X = 0; // upper left corner
            currentFrame.Y = 0; // same
            totalIdleFrames = 4;
            totalJumpFrames = 8;
            totalWalkFrames = 5;
            totalAttackFrames = 4;
            totalSpecialAttackFrames = 8;

            maxJumps = 5;
            maxJumpSpeed = -5;
            jumpsRemaining = maxJumps;
            blocks = blcks;
            gravity = .39;
            //sideCollision = false;
            maxVSpeed = 20;
            pi = p;
            prevGState = GamePad.GetState(pi); // controller  
            hSpeed = 3;
            isAttacking = false;
            canMove = true;
            HeroPos = new Rectangle(0, 0, 50, 100);
            drawOffset = -20;

            maxProj = 3;
            numOfProj = 0;

            //to avoid lag
            reserveFire = new List<HelogiFire>(40);
            for (int i = 0; i < reserveFire.Count; i++)
            {
                reserveFire[i] = new HelogiFire(projectileSheet, blocks, this);
            }
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
                    if (finalHSpeed < 0) { flip = true; }
                    else { flip = false; }

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
                }
            }
            if (isAttacking)
            {
                numFrames = totalAttackFrames;
                currentFrame.Y = frameSize.Y * 0;
                millisecondsPerFrame = 50;
            }
            if (isSpecialAttacking)
            {
                numFrames = totalSpecialAttackFrames;
                currentFrame.Y = frameSize.Y * 2;
                millisecondsPerFrame = 20;
            }
            if (isHit)
            {
                frame = 0;

                // set everything to false
                canMove = false;
                canUse = false;
                canJump = false;
                isAttacking = false;
                isSpecialAttacking = false;

                numFrames = 1;                
                currentFrame.Y = frameSize.Y * 3;
                millisecondsPerFrame = hitStun;
            }

            // move to next frame if the elapsedtime is long enough
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                // set up to do a new image
                timeSinceLastFrame = 0;
                frame++;
                if (frame >= numFrames && !isAttacking && !isHit)  // wrap around to starting image
                {
                    frame = 0;
                }
                if (frame >= numFrames && isAttacking)
                {
                    isAttacking = false;
                    canMove = true;
                }
                if (frame >= numFrames && isHit)
                {
                    isHit = false;
                    canMove = true;
                    canUse = true;
                    canJump = true;
                    canHit = true;
                }

                // location in the spritesheet of frame to display
                if (!isAttacking /* && !isSpecialAttacking*/) { currentFrame.X = frameSize.X * frame; }

                //normal attack
                if (isAttacking) { currentFrame.X = (frameSize.X * 4) + (frameSize.X * frame); }

                if (isSpecialAttacking)
                {
                    if (jumpSpeed >= 0)
                    {
                        canMove = true;
                        isSpecialAttacking = false;
                    }
                }
            }
        }

        //attack
        public override void Attack()
        {
            if (!isAttacking && !isSpecialAttacking && !isHit)
            {
                if (GamePad.GetState(pi).IsButtonDown(Buttons.A) && !prevGState.IsButtonDown(Buttons.A) && canUse)
                {
                    frame = 0;
                    isJumping = true;
                    canFall = true;
                    vSpeed = 0;
                    jumpSpeed = -32;
                    canMove = false;
                    isSpecialAttacking = true;
                    canUse = false;
                }
                if (GamePad.GetState(pi).IsButtonDown(Buttons.B) && !prevGState.IsButtonDown(Buttons.B)) //&& canFall == false originally here
                {
                    frame = 0;
                    canMove = false;
                    isAttacking = true;
                }
                
            }
            
            else if (isAttacking)
            {

                if (frame == (totalAttackFrames - 1) && canUse && numOfProj < maxProj)
                {
                    //make a projectile
                    HelogiBall hb; // electric ball
                    if (!flip) hb = new HelogiBall(projectileSheet, blocks, this, 1);
                    else hb = new HelogiBall(projectileSheet, blocks, this, -1);

                    numOfProj++;
                    // call list of heroes
                    
                    listOfProj.Add(hb); // add to list of projectiles
                    canUse = false;
                }
            }

            else if (isSpecialAttacking)
            {

                if (frame % 2 == 0)
                {
                    HelogiFire hf = new HelogiFire(projectileSheet, blocks, this);
                    listOfProj.Add(hf);
                }
            }
        }

        // jumping
        public override int CalculateJump()
        {
            return (maxJumpSpeed * jumpsRemaining);
        }
    }
}
