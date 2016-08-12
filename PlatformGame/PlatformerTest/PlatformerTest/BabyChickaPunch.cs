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
    class BabyChickaPunch:Projectile
    {
        //additional attributes
        int cycles; //set lifespan
        int maxCycles;

        // constructor
        public BabyChickaPunch(Texture2D spriteSht, Rectangle[] blks, Hero own, int dir)
            : base(spriteSht, blks, own)
        {            
            
            damage = 8;
            
            if (dir > 0)
            {
                knockBack = 6;
                ProjectilePos = new Rectangle((own.HeroPos.X + 50), (own.HeroPos.Y + 20), 16, 16);
            }

            if (dir < 0)
            {
                knockBack = -6;
                ProjectilePos = new Rectangle((own.HeroPos.X + dir), (own.HeroPos.Y + 20), 16, 16);
            }

            // animation
            frame = 0;
            frameSize = new Point(50, 32);
            numFrames = 1;
            millisecondsPerFrame = 50;
            currentFrame = new Point(-48, 128);            
            scale = 1f;

            cycles = 0;
            maxCycles = 2;
            stun = 250;
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
                    if (cycles >= maxCycles) markedForRemoval = true;
                }
            }
        }

        // update method
        public override void Update(GameTime gameTime)
        {
            //ProjectilePos.X += hSpeed; // move the projectile
            AnimationUpdate(gameTime);
            Collision();
        }
    }
}
