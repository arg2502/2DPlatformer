using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerTest
{
    class AssemblyBlock:MovingBlock
    {
        public AssemblyBlock(int x_,int y_,int width_,int height_,int xSpeed_, int ySpeed_, int rangeX_, int rangeY_, int originX_, int originY_)
            :base (x_,y_,width_,height_,xSpeed_, ySpeed_, rangeX_, rangeY_)
        {
            originX = originX_;
            originY = originY_;
        }
        public override void Update()
        {
            // move the block
            blockPos.X += xSpeed;
            blockPos.Y += ySpeed;

            if (Math.Abs(blockPos.X - originX) >= Math.Abs(rangeX))
            {
                blockPos.X = originX;
            }

            if (Math.Abs(blockPos.Y - originY) >= Math.Abs(rangeY))
            {
                blockPos.Y = originY;
            }
        }
    }
}
