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
    class Player
    {
        // attributes
        Rectangle choice;
        Texture2D playerSheet;

        // properties
        public Rectangle Choice { get { return choice; } set { choice = value; } }
        public Texture2D PlayerSheet { get { return playerSheet; } set { playerSheet = value; } }

    }
}
