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
    class WiseGuy: Guy
    {
        // block smartguy is on
        Block myBlock;

        public WiseGuy(Block[] bls, List<Hero> list)
            :base(bls,list)
        {
            damage = 3;
            numOfDrops = 2;
        }

        public override void Attack()
        {
            base.Attack();
            
            // find block smart guy is on
            if (myBlock == null)
            {
                foreach (Block b in blocks)
                {
                    if (enemyPos.Intersects(b.BlockPos))
                    {
                        myBlock = b;
                        break;
                    }
                }
            }
            // stop if at edge of block
            if (target != null && myBlock != null)
            {
                if ((enemyPos.X <= myBlock.BlockPos.X && target.HeroPos.X < enemyPos.X) || (enemyPos.X + enemyPos.Width >= myBlock.BlockPos.X + myBlock.BlockPos.Width && target.HeroPos.X + target.HeroPos.Width > myBlock.BlockPos.X + myBlock.BlockPos.Width))
                {
                    hSpeed = myBlock.XSpeed;
                    finalHSpeed = myBlock.XSpeed;
                    state = State.idle;
                }
            }

        }
        public override void DropItem(ItemStack itemStack_, List<Item> listOfItems)
        {
            int j = 0;
            // get random number
            Random rng = new Random();
            for (int i = 0; i < numOfDrops; i++)
            {
                int random = rng.Next(15, 16); // seed the rng with different values
                random = rng.Next(14);
                Item item;
                if (random <= 9)
                {
                    item = itemStack_.Pop(0); // gold
                }
                else if (random >= 10 && random <= 12)
                {                    
                    item = itemStack_.Pop(2); // eye
                }
                else 
                {
                    item = itemStack_.Pop(3); // brain matter
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
