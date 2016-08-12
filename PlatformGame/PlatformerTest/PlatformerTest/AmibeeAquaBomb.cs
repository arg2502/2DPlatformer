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
    class AmibeeAquaBomb: Projectile
    {
        // attributes
        int cycles;
        int maxCycles;

        // property        
        public int Direction { get { return direction; } set { direction = value; } }

        // construtor
        public AmibeeAquaBomb(Texture2D spriteSht, Rectangle[] blks, Hero own, int dir)
            : base(spriteSht, blks, own)
        {
            direction = dir;
            hSpeed = 0;
            vSpeed = 0;
            if (direction < 0) ProjectilePos = new Rectangle((own.HeroPos.X + (own.HeroPos.Width - 24)), (own.HeroPos.Y - 48), 24, 24);
            if (direction > 0) ProjectilePos = new Rectangle((own.HeroPos.X - 12), (own.HeroPos.Y - 48), 24, 24);

            damage = 1;

            if (direction > 0) knockBack = 3;
            if (direction < 0) knockBack = -3;

            // animation
            frame = 0;
            frameSize = new Point(32, 32);
            numFrames = 4;
            millisecondsPerFrame = 30;
            currentFrame = new Point(0, 0);
            scale = 0.5f;

            currentFrame.Y = frameSize.Y * 3; // setting where in the spritesheet
            stun = 300;

            cycles = 0;
            maxCycles = 90;
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
                }
            }
            currentFrame.X = (frameSize.X * frame);
        }

        // updates the size of the bomb
        public void ScaleUpdate(int charge)
        {
            scale = 0.5f + ((float)(charge) / 2000);
            damage = (int)((scale * 5.5));
        }

        // update method
        public override void Update(GameTime gameTime)
        {
            ProjectilePos.X += (int)hSpeed; // move the projectile
            ProjectilePos.Y += vSpeed;
            AnimationUpdate(gameTime);
            Collision();       
    
            // remove projectile
            if (hSpeed != 0)
            {
                cycles++;
            }
            if (cycles > maxCycles)
            {
                markedForRemoval = true;
            }
            
        }
    }
}
