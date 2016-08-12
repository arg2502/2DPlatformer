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
    class Room
    {
        // attributes
        Rectangle checkpoint;
        bool checkpointBool;
        int levelNum;
        FileIO fio;
        Point startingPos;
        Texture2D background; // work on later

        // room boundaries
        int leftWall;
        int rightWall;
        int deathPlane;
        int topPlane;

        // temp item list
        List<Item> tempList;

        public FileIO Fio { get { return fio; } }

        public Room()
        {
            fio = new FileIO();
        }

        public void GenerateRoom(int levelNum_, List<Hero> listOfHeroes, List<Enemy> listOfEnemies, Block[] blockArray, ItemStack itS)
        {
            levelNum = levelNum_;
            checkpointBool = false;

            switch(levelNum)
            {
                case 0:
                    blockArray = fio.ReadLevel(levelNum);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blockArray, listOfHeroes);
                    startingPos = new Point(3*32, 0*32);
                    leftWall = 0;
                    rightWall = 100 * 32;
                    deathPlane = 20 * 32;
                    topPlane = -5 * 32;
                    break;
                case 1:
                    blockArray = fio.ReadLevel(levelNum);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blockArray, listOfHeroes);
                    startingPos = new Point(-2*32, 0*32);
                    leftWall = -5 * 32;
                    rightWall = 100 * 32;
                    deathPlane = 20 * 32;
                    topPlane = -5 * 32;
                    break;
                case 2:
                    blockArray = fio.ReadLevel(levelNum);
                    listOfEnemies = fio.ReadEnemyList(levelNum, blockArray, listOfHeroes);
                    startingPos = new Point(-2*32, 0*32);
                    leftWall = -20 * 32;
                    rightWall = 240 * 32;
                    deathPlane = 25 * 32;
                    topPlane = -20 * 32;
                    break;
            }
            // set at starting point
            foreach(Hero h in listOfHeroes)
            {
                h.HeroPos = new Rectangle(startingPos.X,startingPos.Y, h.HeroPos.Width, h.HeroPos.Height);
            }
            tempList = new List<Item>();
            for (int i = 0; i < 25; i++)
            {
                tempList.Add(itS.Pop(0));
            }
            for (int i = 0; i < 25; i++)
            {
                tempList[i].Blocks = blockArray;
                itS.Push(tempList[i]);
            }
        }
    }
}
