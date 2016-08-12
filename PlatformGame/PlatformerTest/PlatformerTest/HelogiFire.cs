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
    class HelogiFire: Projectile
    {
        public HelogiFire(Texture2D spriteSht, Rectangle[] blks, Hero own)
            : base(spriteSht, blks, own)
        {
            ProjectilePos = new Rectangle((own.HeroPos.X - 10), (own.HeroPos.Y + own.HeroPos.Height), 48, 48);
            damage = 5;

            // animation
            frame = 0;
            frameSize = new Point(32, 32);
            numFrames = 4;
            millisecondsPerFrame = 30;
            currentFrame = new Point(0, 32);
            scale = 2f;
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
                    markedForRemoval = true;
                }
            }
            currentFrame.X = (frameSize.X * frame);
        }

        // update method
        public override void Update(GameTime gameTime)
        {
            AnimationUpdate(gameTime);
            Collision();
        }
        
    }
}
