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
    class SpeedBlock: Block
    {
        
       

        public SpeedBlock(int x_, int y_, int width_, int height_, int xSpeed_, int ySpeed_)
            :base(x_,y_,width_,height_)
        {
            xSpeed = xSpeed_;
            ySpeed = ySpeed_;

            // animation
            if (xSpeed > 0) frame = 0;
            else if (xSpeed < 0) frame = numFrames - 1;
            frameSize = new Point(32, 32);
            numFrames = 4;
            millisecondsPerFrame = 300/Math.Abs(xSpeed);
            currentFrame = new Point(0, 0);

        }
        public override void Animation(GameTime gameTime)
        {
            // update elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                if (xSpeed > 0)
                {
                    // increase frame
                    timeSinceLastFrame = 0;
                    frame++;
                    if (frame >= numFrames)
                    {
                        frame = 0;
                    }
                    currentFrame.X = frame * 32;
                }
                else if (xSpeed < 0)
                {
                    // decrease frame
                    timeSinceLastFrame = 0;
                    frame--;
                    if (frame < 0)
                    {
                        frame = numFrames - 1;
                    }
                    currentFrame.X = frame * 32;
                }
            }

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            Animation(gameTime);
            
            // draw start conveyor
            currentFrame.Y = 0;
            spriteBatch.Draw(spriteSheet, new Vector2(blockPos.X, blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            // drawing middle conveyor
            currentFrame.Y = 32; // second row
            for(int i = 1; i < (blockPos.Width/32) - 1; i++)
            {                
                spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (i*32)), blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            // drawing end conveyor
            currentFrame.Y = 64;
            spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (blockPos.Width - 32)), blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
        }
    }
}
