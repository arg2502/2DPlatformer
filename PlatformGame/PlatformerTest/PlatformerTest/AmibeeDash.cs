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
    class AmibeeDash:Projectile
    {
        // constructor
        public AmibeeDash(Texture2D spriteSht, Rectangle[] blks, Hero own, int dir)
            : base(spriteSht, blks, own)
        {
            //ProjectilePos = new Rectangle(own.HeroPos.X, (own.HeroPos.Y + 15), own.HeroPos.Width, own.HeroPos.Height -15);
            damage = 4;
            direction = dir;

            if (dir > 0)
            {
                knockBack = 7;
                ProjectilePos = new Rectangle(own.HeroPos.X + 123, (own.HeroPos.Y + 15), 20, own.HeroPos.Height - 15);
            }
            if (dir < 0)
            {
                knockBack = -7;
                ProjectilePos = new Rectangle(own.HeroPos.X - 35, (own.HeroPos.Y + 15), 20, own.HeroPos.Height - 15);
            }

            // animation
            frame = 0;
            frameSize = new Point(20, 85); //50, 32
            numFrames = 1;
            millisecondsPerFrame = 500;
            currentFrame = new Point(320, 0);//320, 0
            scale = 1f;
            timeSinceLastFrame = 0;
                        
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
                markedForRemoval = true;
            }
        }

        // update method
        public override void Update(GameTime gameTime)
        {
            if (direction > 0)
            {
                ProjectilePos.X = owner.HeroPos.X + 123; // move the projectile
            }
            if (direction < 0)
            {
                ProjectilePos.X = owner.HeroPos.X - 35; // move the projectile
            }
            ProjectilePos.Y = owner.HeroPos.Y + 15;
            AnimationUpdate(gameTime);
            Collision();
        }
    }
}
