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
    class Goal
    {
        Rectangle goalPos;
        Point currentFrame;
        Point frameSize;
        int numOfDrops;
        int timesCalled;
        bool goalCollide;

        public Rectangle GoalPos { get { return goalPos; } set { goalPos = value; } }
        public bool GoalCollide { get { return goalCollide; } set { goalCollide = value; } }
        public int TimesCalled { get { return timesCalled; } set { timesCalled = value; } }

        public Goal()
        {
            currentFrame = new Point(0, 0);
            frameSize = new Point(106, 64);
            numOfDrops = 2;
            timesCalled = 0;
            goalCollide = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(spriteSheet, new Vector2((goalPos.X), (goalPos.Y)),
                            new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                            Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public void DropItem(ItemStack itemStack_, List<Item> listOfItems, int type1_, int type2_, int type3_, int type4_, int type5_)
        {
            //int xSpeed = 0;
            // get random number
            Random rng = new Random();
            for (int i = 0; i < numOfDrops; i++)
            {
                int random = rng.Next(22, 23); // seed the rng with different values
                random = rng.Next(21);
                Item item;
                if (random <= 6)
                {
                    item = itemStack_.Pop(type1_);
                }
                else if (random >= 7 && random <= 12)
                {
                    item = itemStack_.Pop(type2_);
                }
                else if(random >= 13 && random <= 17)
                {
                    item = itemStack_.Pop(type3_);
                }
                else if(random >= 18 && random <= 19)
                {
                    item = itemStack_.Pop(type4_); 
                }
                else
                {
                    item = itemStack_.Pop(type5_);
                }



                // set rectangle
                item.ItemPos = new Rectangle(goalPos.X + goalPos.Width/2, goalPos.Y, 32, 32);

                // random y speed
                random = rng.Next(8, 14);                
                item.YSpeed = -1.0f * random;
                item.FinalTimer = 0;

                random = rng.Next(-5, 5);
                item.XSpeed = random;

                //switch(xSpeed)
                //{
                //    case 0:
                //        item.XSpeed = 3.0f;
                //        break;
                //    case 1:
                //        item.XSpeed = -3.0f;
                //        break;
                //    case 2:
                //        item.XSpeed = 0.0f;
                //        break;

                //}

                listOfItems.Add(item);
                //j++;
            }
            // increased num of times this method has been called
            timesCalled++;
        }
    }
}
