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
    class Item
    {
        int type;
        Rectangle itemPos;
        //Texture2D spriteSheet;
        float xSpeed;
        float ySpeed;
        float maxYSpeed;
        Block[] blocks;
        bool onBlock;

        // timers
        //int initialTimer;
        int finalTimer;

        // properties
        public int Type { get { return type; } set { type = value; } }
        public float XSpeed { get { return xSpeed; } set { xSpeed = value; } }
        public float YSpeed { get { return ySpeed; } set { ySpeed = value; } }
        public Rectangle ItemPos { get { return itemPos; } set { itemPos = value; } }
        public bool OnBlock { get { return onBlock; } set { onBlock = value; } }
        public int FinalTimer { get { return finalTimer; } set { finalTimer = value; } }
        public Block[] Blocks { set { blocks = value; } }

        public Item(int type_, Block[] blocks_)
        {
            type = type_;            
            blocks = blocks_;
            onBlock = false;
            maxYSpeed = 25.0f;
        }

        public void GravityUpdate()
        {
            foreach(Block b in blocks)
            {
                if (itemPos.Intersects(b.BlockPos))
                {
                    
                    // item is on top of block
                    if ((itemPos.Top) < b.BlockPos.Top && !onBlock)
                    {
                        ySpeed = 0;
                        itemPos.Y = b.BlockPos.Y - (itemPos.Height - 1);
                        onBlock = true;
                    }
                    // item hits block from below
                    else if(b.BlockPos.Bottom < itemPos.Bottom && !onBlock)
                    {
                        ySpeed *= -1;
                    }
                    // side collision
                    else
                    {
                        xSpeed *= -1;
                    }
                }                
            }
            if (!onBlock)
            {
                ySpeed++;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(itemPos.X, itemPos.Y),
                    new Rectangle(32 * type, 0, 32, 32),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public void Update()
        {
            onBlock = false;

            if(xSpeed > 0)
            {
                xSpeed -= 0.1f;
            }
            else if (xSpeed < 0)
            {
                xSpeed += 0.1f;
            }

            // limit ySpeed
            if(Math.Abs(ySpeed) > Math.Abs(maxYSpeed))
            {
                ySpeed = maxYSpeed;
            }

            itemPos.X += (int)xSpeed;
            itemPos.Y += (int)ySpeed;

            GravityUpdate();         
        }
    }
}
