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
    class BabyChickaPunchEffect:Projectile
    {
        //additional attributes
        int cycles; //set lifespan
        int maxCycles;

        // constructor
        public BabyChickaPunchEffect(Texture2D spriteSht, Rectangle[] blks, Hero own, int dir)
            : base(spriteSht, blks, own)
        {            
            
            damage = 5;
            
            if (dir > 0)
            {
                knockBack = 6;
                ProjectilePos = new Rectangle((own.HeroPos.X + 38), (own.HeroPos.Y + 28), 48, 32);
            }

            if (dir < 0)
            {
                flip = true;
                knockBack = -6;
                ProjectilePos = new Rectangle((own.HeroPos.X - 25), (own.HeroPos.Y + 28), 48, 32);
            }

            // animation
            frame = -1;
            frameSize = new Point(48, 32);
            numFrames = 3;
            millisecondsPerFrame = 30;
            currentFrame = new Point(-48, 128);            
            scale = 1f;

            cycles = 1;
            maxCycles = 1;
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
                if(markedForRemoval == false) currentFrame.X = (frameSize.X * frame);
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
