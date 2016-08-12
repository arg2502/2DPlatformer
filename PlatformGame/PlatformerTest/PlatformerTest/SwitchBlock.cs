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
    class SwitchBlock: MovingBlock
    {
        // attributes
        int startPos;
        int type;
        bool isOwned;
        int realXSpeed, realYSpeed; // hold values of actual x and y speeds

        public bool IsOwned { get { return isOwned; } set { isOwned = value; } }

        public SwitchBlock(int x_, int y_, int width_, int height_, int xSpeed_, int ySpeed_, int rangeX_, int rangeY_, int startPos_, int type_)
            :base(x_,y_,width_,height_, xSpeed_, ySpeed_, rangeX_, rangeY_)
        {
            startPos = startPos_;
            type = type_;
            isOwned = false;
            xSpeed = 0;
            ySpeed = 0;
            realXSpeed = xSpeed_;
            realYSpeed = ySpeed_;
        }
        public override void Update()
        {            
        }
        // timer activates, move to position and stop
        public void StartUpdate()
        {
            xSpeed = realXSpeed;
            ySpeed = realYSpeed;
            // move in x
            if(type == 0)
            {
                // move right
                if (blockPos.X < (startPos + rangeX))
                {
                    blockPos.X += xSpeed;
                }
                // move left
                else if (blockPos.X > (startPos + rangeX))
                {
                    blockPos.X -= xSpeed;
                }
                // stay still
                else
                {
                    blockPos.X = (startPos + rangeX);
                }
            }
            // move in y
            else
            {
                // move down
                if (blockPos.Y < (startPos + rangeY))
                {
                    blockPos.Y += ySpeed;
                }
                // move up
                else if (blockPos.Y > (startPos + rangeY))
                {
                    blockPos.Y -= ySpeed;
                }
                // stay still
                else
                {
                    blockPos.Y = (startPos + rangeY);
                }
            }
        }

        // timer up, move back to original position
        public int EndUpdate()
        {
            // negate to go other way
            xSpeed = -realXSpeed;
            ySpeed = -realYSpeed;
            // move in x
            if (type == 0)
            {
                // move right
                if (blockPos.X > startPos)
                {
                    blockPos.X += xSpeed;
                    return 0;
                }
                // move left
                else if (blockPos.X < startPos)
                {
                    blockPos.X -= xSpeed;
                    return 0;
                }
                // stay still
                else
                {
                    blockPos.X = startPos;
                    xSpeed = 0;
                    ySpeed = 0;
                    return 1;
                }
            }
            // move in y
            else
            {
                // move down
                if (blockPos.Y > startPos)
                {
                    blockPos.Y += ySpeed;
                    return 0;
                }
                // move up
                else if (blockPos.Y < startPos)
                {
                    blockPos.Y -= ySpeed;
                    return 0;
                }
                // stay still
                else
                {
                    blockPos.Y = startPos;
                    xSpeed = 0;
                    ySpeed = 0;
                    return 1;
                }
            }
        }
        //public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        //{
        //    spriteBatch.Draw(spriteSheet, new Vector2((blockPos.X), (blockPos.Y)),
        //                        new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
        //                        Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        //}
    }
}
