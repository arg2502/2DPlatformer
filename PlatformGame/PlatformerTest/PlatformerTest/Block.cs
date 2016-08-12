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
    class Block
    {
        // attributes        
        protected int x, y, width, height; // rectangle (stationary just needs these...rest equal zero)
        protected Rectangle blockPos;
        protected int xSpeed, ySpeed; // for calculating player movement
        protected int damage;
        protected bool isAssembly;
        protected bool isJumpThru;

        // for switchblock
        protected bool isOwned;
        public bool IsOwned { get { return isOwned; } set { isOwned = value; } }

        // animation
        protected int frame; // the frame to display
        protected Point frameSize;
        protected int numFrames;// the amount of frames in animation
        protected int timeSinceLastFrame; // in milliseconds
        protected int millisecondsPerFrame;
        protected Point currentFrame; // upper left corner of the frame

        // properties
        public Rectangle BlockPos { get { return blockPos; } set { blockPos = value; } }
        public int XSpeed { get { return xSpeed; } set { xSpeed = value; } }
        public int YSpeed { get { return ySpeed; } set { ySpeed = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public bool IsAssembly { get { return isAssembly; } }
        public bool IsJumpThru { get { return isJumpThru; } set { isJumpThru = value; } }

        // constructor
        public Block(int x_, int y_, int width_, int height_)
        {
            x = x_;
            y = y_;
            width = width_;
            height = height_;            
            blockPos = new Rectangle(x, y, width, height);
            xSpeed = 0;
            ySpeed = 0;
            isAssembly = false;
            currentFrame = new Point(0, 0);
            frameSize = new Point(32, 32);
            isJumpThru = false;
                       
            
        }

        public virtual void Update()
        {
        }
        public virtual void Animation(GameTime gameTime)
        {
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            // different hazards
            if (damage == 5) // spiked balls
            {
                currentFrame.X = 0;
            }
            else if (damage == 6) // ground spikes
            {
                currentFrame.X = 32;
            }
            else
            {
                currentFrame.X = 0;
            }
            if (damage != 0)
            {
                for (int j = 0; j < (blockPos.Height / 32); j++)
                {
                    for (int i = 0; i < (blockPos.Width / 32); i++)
                    {

                        spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (i * 32)), (blockPos.Y + (j * 32))),
                            new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                            Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
            }

            if (damage == 0)
            {
                // draw top row of blocks
                currentFrame = new Point(0, 0);
                for (int i = 0; i < (blockPos.Width / 32); i++)
                {

                    spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (i * 32)), (blockPos.Y)),
                            new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                            Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                    if (currentFrame.X == 0) currentFrame.X = 32;
                    else if (currentFrame.X == 32) currentFrame.X = 64;
                    else if (currentFrame.X == 64) currentFrame.X = 96;
                    else if (currentFrame.X == 96) currentFrame.X = 128;
                    else currentFrame.X = 0;          
                }

                // draw rest of rows
                currentFrame = new Point(0, 32);

                for (int j = 1; j < (blockPos.Height / 32); j++)
                {
                    if (blockPos.Y + (j * 32) == (19 * 32)) // check if underground -- 19 is an arbitrary number
                        currentFrame.Y = 64;
                    if (blockPos.Y + (j * 32) >= (20 * 32)) // check if underground -- 20 is an arbitrary number
                        currentFrame.Y = 96;

                    for (int i = 0; i < (blockPos.Width / 32); i++)
                    {                   
                        
                        spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (i * 32)), (blockPos.Y + (j * 32))),
                                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                        if (currentFrame.X == 0) currentFrame.X = 32;
                        else if (currentFrame.X == 32) currentFrame.X = 64;
                        else if (currentFrame.X == 64) currentFrame.X = 96;
                        else if (currentFrame.X == 96) currentFrame.X = 128;
                        else currentFrame.X = 0;

                    }
                }
            }
        }
    }
}
