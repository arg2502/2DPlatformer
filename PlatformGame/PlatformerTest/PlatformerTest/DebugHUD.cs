using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PlatformerTest
{
    class DebugHUD
    {
        //attributes
        //int fps;
        string HUD;

        // frame counter
        FrameCounter fc = new FrameCounter();

        //Calculate the frames per second
        public string CalcFps(GameTime gameTime, Hero h1, Hero h2, Hero h3, Hero h4)
        {
            // Frame Counter stuff
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fc.Update(deltaTime);
            var fps = fc.AverageFramesPerSecond;

            /*HUD = "Frames per second: " + fps + 
                "\nHero Right, Left, Top: " + h.HeroPos.Right + " ," + h.HeroPos.Left + " ," + h.HeroPos.Top +
                "\nGravity: " + h.Gravity +
                "\nCanFall: " + h.CanFall + " Can Jump: " + h.CanJump +
                "\nIsJumping: " + h.IsJumping +
                "\nSide Collision: " + h.SideCollision +
                "\nFalling Speed: " + h.VSpeed +
                "\nFinal Vertical Speed: " + h.FinalVSpeed +
                "\nMax Vertical Speed: " + h.MaxVSpeed +
                "\nJump Speed: " + h.JumpSpeed +
                "\nMax Jumps: " + h.MaxJumps +
                "\nMax Jump Speed: " + h.MaxJumpSpeed +
                "\nJumps Remaining: " + h.JumpsRemaining +
                "\nNum of Blocks: " + h.NumOfBlocks;*/

            /*HUD = "Enemy Position: " + e.EnemyPos.X + ", " + e.EnemyPos.Y +
                "\nGravity: " + e.Gravity +
                "\nCanFall: " + e.CanFall +
                "\nSide Collision: " + e.SideCollision +
                "\nVertical Speed: " + e.VSpeed +
                "\nFinal V Speed: " + e.FinalVSpeed +
                "\nMax V Speed: " + e.MaxVSpeed +
                "\nCharacters Array: " + e.Characters[0].HeroPos + ", " + //e.Characters[1].HeroPos +
                //"\nTarget: " + e.Target.HeroPos + ", " + e2.Target.HeroPos +
                "\nRange: " + e.Range + 
                "\nAcceleration: " + e.Acceleration +
                "\nState: " + e.StateProp + ", 2: " + e2.StateProp + ", 3: " + e3.StateProp + ", 4: " + e4.StateProp +
                "\nHorizontal Speed: " + e.HSpeed +
                "\nMax H Speed: " + e.MaxHSpeed +
                "\nFinal H Speed: " + e.FinalHSpeed;*/
            HUD = null;
            if (h1 != null) HUD += "Player 1: " + h1.Health;
            if (h2 != null) HUD += "\nPlayer 2: " + h2.Health;
            if (h3 != null) HUD += "\nPlayer 3: " + h3.Health;
            if (h4 != null) HUD += "\nPlayer 4: " + h4.Health;
            
            return HUD;
        }
    }
}
