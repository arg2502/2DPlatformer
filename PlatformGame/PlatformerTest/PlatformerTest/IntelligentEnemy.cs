using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerTest
{
    class IntelligentEnemy:Guy
    {
        Block myBlock;
        public IntelligentEnemy(Block[] bls, List<Hero> list)
            :base(bls,list)
        {
            maxHSpeed = 5;
            isJumpable = false;
            rangeX += 100;
        }

        public override void Attack()
        {
            base.Attack();
            // find block smart guy is on
            if (myBlock == null)
            {
                foreach (Block b in blocks)
                {
                    if (enemyPos.Intersects(b.BlockPos) && (b.BlockPos.Bottom > enemyPos.Bottom))
                    {
                        myBlock = b;
                        break;
                    }
                }
            }
            // stop if at edge of block
            if (target != null && myBlock != null)
            {
                if ((enemyPos.X <= myBlock.BlockPos.X && target.HeroPos.X < enemyPos.X) 
                    || (enemyPos.X + enemyPos.Width >= myBlock.BlockPos.X + myBlock.BlockPos.Width && target.HeroPos.X + target.HeroPos.Width > myBlock.BlockPos.X + myBlock.BlockPos.Width))
                {
                    if (myBlock.BlockPos.Bottom > enemyPos.Bottom)
                    {
                        
                        vSpeed = -12;
                        collidingBlock = null;
                        myBlock = null;
                    }
                }
            }

        }
    }
}
