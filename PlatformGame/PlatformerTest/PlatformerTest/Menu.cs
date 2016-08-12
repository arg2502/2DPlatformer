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
    class Menu
    {
        /*// attributes
        public enum MenuReader { CharSelect, Gameplay };
        public MenuReader menu;

        // draw methods
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawCharSelect(spriteBatch, gameTime);
            DrawGamePlay(spriteBatch, gameTime);
        }

        public void DrawCharSelect(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void DrawGamePlay(SpriteBatch spriteBatch, GameTime gameTime)
        {
            h.LoadGraphics(babyChicka_sheet, projectileSheet);
            h.Draw(gameTime, spriteBatch);

            h2.LoadGraphics(helogi_sheet, projectileSheet);
            h2.Draw(gameTime, spriteBatch);
            foreach (Rectangle r in blocks)
            {
                spriteBatch.Draw(block, r, Color.White);
            }
            spriteBatch.DrawString(font, debug, new Vector2(0, 0), Color.Black);

            // enemy
            /*foreach (Enemy e in listOfEnemies)
            {
                if (e != null)
                {
                    e.Draw(guySprite, gameTime, spriteBatch);
                }
            }

            // projectiles
            foreach (Projectile p in h.ListOfProj)
            {
                p.Draw(gameTime, spriteBatch);
            }
            foreach (Projectile p in h2.ListOfProj)
            {
                p.Draw(gameTime, spriteBatch);
            }
        }*/


        // move cursor
        public Rectangle CursorUpdate(Rectangle cursor, PlayerIndex pi)
        {
            // value to hold gamepad states
            Vector2 direction;
            GamePadState gState = GamePad.GetState(pi);
            direction = new Vector2(gState.ThumbSticks.Left.X, -1 * gState.ThumbSticks.Left.Y);

            // ints to hold values of the x and y
            int x = 0;
            int y = 0;

            if (direction.X > 0) x = 3;
            if (direction.X < 0) x = -3;

            if (direction.Y > 0) y = 3;
            if (direction.Y < 0) y = -3;

            // change the position
            cursor.X += x;
            cursor.Y += y;

            //keep the cursor on screen
            if (cursor.X < 0) cursor.X = 0;
            if (cursor.X > 1280 - cursor.Width) cursor.X = 1280 - cursor.Width;
            if (cursor.Y < 0) cursor.Y = 0;
            if (cursor.Y > 720 - cursor.Height) cursor.Y = 720 - cursor.Height;

            Rectangle newDirection = new Rectangle(cursor.X, cursor.Y, cursor.Width, cursor.Height);
            return newDirection;
        }

    }
}
