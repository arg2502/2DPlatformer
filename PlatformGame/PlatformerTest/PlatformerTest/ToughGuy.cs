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
    class ToughGuy: Guy
    {
        public ToughGuy(Block[] bls, List<Hero> list)
            :base(bls,list)
        {
            frameSize = new Point(62, 56);
            isJumpable = false;
            damage = 5;
        }
        public override void DropItem(ItemStack itemStack_, List<Item> listOfItems)
        {
            int j = 0;
            // get random number
            Random rng = new Random();
            for (int i = 0; i < numOfDrops; i++)
            {
                int random = rng.Next(14, 15); // seed the rng with different values
                random = rng.Next(13);
                Item item;
                if (random <= 6)
                {
                    item = itemStack_.Pop(4); // blue jelly
                }
                else if (random >= 7 && random <= 11)
                {
                    item = itemStack_.Pop(0); // gold
                }
                else
                {
                    item = itemStack_.Pop(2); // eye
                }



                // set rectangle
                item.ItemPos = new Rectangle(enemyPos.X, enemyPos.Y, 32, 32);
                item.YSpeed = -7.0f;
                item.FinalTimer = 0;

                switch (j)
                {
                    case 0:
                        item.XSpeed = 3.0f;
                        break;
                    case 1:
                        item.XSpeed = -3.0f;
                        break;
                    case 2:
                        item.XSpeed = 0.0f;
                        break;

                }

                listOfItems.Add(item);
                j++;
            }
        }
    }
}
