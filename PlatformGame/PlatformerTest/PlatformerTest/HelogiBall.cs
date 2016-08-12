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
    // Helogi's normal attack (A)
    class HelogiBall: Projectile
    {

        //additional attributes
        int cycles; //set lifespan
        int maxCycles;

        // constructor
        public HelogiBall(Texture2D spriteSht, Rectangle[] blks, Hero own, int dir)
            : base(spriteSht, blks, own)
        {
            direction = dir;
            hSpeed = 14 * direction;
            ProjectilePos = new Rectangle(own.HeroPos.X, (own.HeroPos.Y + 20), 16, 16);
            damage = 1;

            // animation
            frame = 0;
            frameSize = new Point(32, 32);
            numFrames = 3;
            millisecondsPerFrame = 30;
            currentFrame = new Point(0, 0);
            scale = 1f;

            cycles = 0;
            maxCycles = 15;
            stun = 0;
        }

        // animation
        public void AnimationUpdate(GameTime gameTime)
        {
            // getting game time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;            

            // changing frame
            if (timeSinceLastFrame > millisecondsPerFrame)
            {                
                timeSinceLastFrame = 0;
                frame++;
                if (frame >= numFrames)
                {
                    frame = 0;
                    cycles++;
                    if (cycles >= maxCycles)
                    {
                        markedForRemoval = true;

                        //projectile nerf
                        if (owner.numOfProj > 0)
                        {
                            owner.numOfProj--;
                        }
                    }
                }
            }
            currentFrame.X = (frameSize.X * frame);
        }

        // update method
        public override void Update(GameTime gameTime)
        {
            if(hSpeed < 0 && hSpeed < -4) hSpeed += 0.2f;
            if (hSpeed > 0 && hSpeed > 4) hSpeed -= 0.2f;
            ProjectilePos.X += (int)hSpeed;// move the projectile
            AnimationUpdate(gameTime);
            Collision();
        }
    }
}
