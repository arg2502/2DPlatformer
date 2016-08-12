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
    class HiddenBoundary
    {
        Rectangle boundaryPos;
        Point newBoundary;
        bool boundaryCollide;

        public Rectangle BoundaryPos { get { return boundaryPos; } set { boundaryPos = value; } }
        public bool BoundaryCollide { get { return boundaryCollide; } set { boundaryCollide = value; } }
        public Point NewBoundary { get { return newBoundary; } set { newBoundary = value; } }

        public HiddenBoundary(Rectangle boundaryPos_)
        {
            boundaryPos = boundaryPos_;
            newBoundary = new Point(0,0);
            boundaryCollide = false;
        }
    }
}
