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
    class Projectile
    {
        //this class will be the parent of all player projectiles

        //attributes
        protected Rectangle ProjectilePos;
        protected float hSpeed;
        protected int vSpeed;
        protected int damage; // the amount of damage the projectile inflicts on target
        protected int direction; // Left: -1 Right: +1
        protected bool markedForRemoval;
        protected int knockBack;

        // properties
        public float HSpeed { get { return hSpeed; } set { hSpeed = value; } }
        public int VSpeed { get { return vSpeed; } set { vSpeed = value; } }
        public bool MarkedForRemoval { get { return markedForRemoval; } }

        //collision attributes
        protected Rectangle[] blocks;
        protected List<Hero> opponents;//can this be the hero rectangles instead?
        protected Hero owner;
        protected int stun;

        //animation attributes
        protected Texture2D spriteSheet;
        protected int frame; // the frame to display
        protected Point frameSize;
        protected int numFrames;// the amount of frames in animation
        protected int timeSinceLastFrame; // in milliseconds
        protected int millisecondsPerFrame;
        protected Point currentFrame; // upper left corner of the frame
        protected int totalFrames;
        protected float scale;
        protected bool flip;

        //default constructor
        public Projectile(Texture2D spriteSht, Rectangle[] blks, Hero own)
        {
            //hope to make all projectiles exist on one sheet
            spriteSheet = spriteSht;
            blocks = blks;
            owner = own;

            markedForRemoval = false;
        }

        // get list of heroes you are up against
        public void GetList(List<Hero> opp)
        {
            opponents = opp;
        }

        // generic update
        public virtual void Update(GameTime gameTime)
        {
        }
        
        // generic draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!flip)
            spriteBatch.Draw(spriteSheet, new Vector2(ProjectilePos.X, ProjectilePos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteSheet, new Vector2(ProjectilePos.X, ProjectilePos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);

        }

        // collision detection
        public void Collision()
        {
            foreach (Hero h in opponents)
            {
                if (h != owner && ProjectilePos.Intersects(h.HeroPos) && h.CanHit)
                {                    
                    // damage the opponent
                    h.Health -= damage;
                    h.IsHit = true;
                    h.CanHit = false;
                    h.HeroKnockBack = knockBack;
                    h.CanFall = true; // to make them fall still
                    if (stun != 0)
                    {
                        h.HitStun = stun;                        
                    }

                    // amibee dash
                    /*if (this is AmibeeDash)
                    {
                        owner.IsHit = true;
                        owner.HeroKnockBack = -(knockBack + 2);
                        owner.HitStun = stun *2;
                        owner.CanFall = true;
                    }*/

                    markedForRemoval = true;

                    //projectile nerf
                    if (owner.numOfProj > 0)
                    {
                        owner.numOfProj--;
                    }
                }

                if (h != owner && ProjectilePos.Intersects(h.HeroPos) && !h.CanHit)
                {
                    markedForRemoval = true;
                    //projectile nerf
                    if (owner.numOfProj > 0)
                    {
                        owner.numOfProj--;
                    }
                }

                // if Amibee gets hit
                /*if (h is Amibee && h == owner && !h.IsSpecialAttacking && this is AmibeeAquaBomb && hSpeed == 0)
                {
                    markedForRemoval = true;
                }*/

                

            }
        }

    }
}
