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
    class Switch
    {
        Rectangle switchPos;
        //List<SwitchBlock> myList;
        Block[] myList;
        int width, height;
        bool isActivated;
        int switchTimer;
        int maxTime = 10000;
        int type;
        int isUpdateDone;

        Point currentFrame;
        Point frameSize;

        public Rectangle SwitchPos { get { return switchPos; } set { switchPos = value; } }
        public Block[] MyList { get { return myList; } set { myList = value; } }
        public bool IsActivated { get { return isActivated; } set { isActivated = value; } }

        public Switch(int x_, int y_, int type_, int size_)
        {
            width = 64;
            height = 64;
            switchPos = new Rectangle(x_, y_, width, height);
            myList = new SwitchBlock[size_];
            isActivated = false;
            type = type_; // 0 = timed
            switchTimer = 0;
            isUpdateDone = 0;

            currentFrame = new Point(0, 0);
            frameSize = new Point(64, 64);
        }

        //// add switches blocks to game1 list of Blocks
        //public Block[] AddList(Block[] fullList)
        //{
        //    //foreach(SwitchBlock b in myList)
        //    //{
        //    //    fullList.Add(b);
        //    //}
        //    Block[] finalArray = new Block[fullList.Length + myList.Length];

        //    // loop through first list
        //    for(int i = 0; i < fullList.Length; i++)
        //    {
        //        finalArray[i] = fullList[i];
        //    }
        //    // loop through switch list
        //    for(int i = fullList.Length; i < myList.Length; i++)
        //    {
        //        int j = 0;
        //        finalArray[i] = myList[j];
        //        j++;
        //    }

        //    return fullList;
        //}

        public void Update(GameTime gameTime)
        {
            // timed switch
            if(type == 0)
            {
                TimerSwitchUpdate(gameTime);
            }
           
        }
        public void TimerSwitchUpdate(GameTime gameTime)
        {
            // start timer
            switchTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (isActivated)
            {
                if (switchTimer < maxTime)
                {
                    foreach (SwitchBlock b in myList)
                    {
                        b.StartUpdate();
                    }
                }
                else if (switchTimer >= maxTime)
                {
                    isUpdateDone = 0; // count to check if all the block are back to original
                    foreach(SwitchBlock b in myList)
                    {
                        isUpdateDone += b.EndUpdate();
                    }

                    // stop update if all blocks are back to start
                    if(isUpdateDone >= myList.Count())
                    {
                        isActivated = false;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            if (isActivated)
            {
                spriteBatch.Draw(spriteSheet, new Vector2((switchPos.X), (switchPos.Y)),
                                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                                Color.Blue, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (!isActivated)
            {
                spriteBatch.Draw(spriteSheet, new Vector2((switchPos.X), (switchPos.Y)),
                                new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y),
                                Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

    }
}
