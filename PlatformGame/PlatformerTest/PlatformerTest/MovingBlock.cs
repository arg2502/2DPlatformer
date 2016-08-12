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
    class MovingBlock: Block
    {
        // attributes
        protected int rangeX, rangeY; // how far they can go
        protected int originX, originY; // where they start

        // properties
        public int RangeX { get { return rangeX; } }
        public int RangeY { get { return rangeY; } }

        public MovingBlock(int x_, int y_, int width_, int height_, int xSpeed_, int ySpeed_, int rangeX_, int rangeY_)
            :base(x_,y_,width_,height_)
        {
            
            xSpeed = xSpeed_;
            ySpeed = ySpeed_;
            rangeX = rangeX_;
            rangeY = rangeY_;
            originX = x_;
            originY = y_;
           
        }

        public override void Update()
        {
            // move the block
            blockPos.X += xSpeed;
            blockPos.Y += ySpeed;

            if(Math.Abs(blockPos.X - originX) >= rangeX)
            {
                xSpeed *= -1;
            }

            if(Math.Abs(blockPos.Y - originY) >= rangeY)
            {
                ySpeed *= -1;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            // draw Left side
            currentFrame.Y = 0;
            spriteBatch.Draw(spriteSheet, new Vector2(blockPos.X, blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            // drawing middle 
            currentFrame.Y = 32; // second row
            for (int i = 1; i < (blockPos.Width / 32) - 1; i++)
            {
                spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (i * 32)), blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            // drawing Right Side
            currentFrame.Y = 64;
            spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X + (blockPos.Width - 32)), blockPos.Y),
                    new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        
    }
}
