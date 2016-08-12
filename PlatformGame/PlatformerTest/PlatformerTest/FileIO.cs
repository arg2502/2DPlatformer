using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PlatformerTest
{
    class FileIO
    {
      // attributes
      StreamReader readIn;
      List<string[]> listOfLines;
      
      // level specific  
      string levelName; // filename
      Block[] listOfBlocks; // level that is being imported
      int x, y, width, height, xSpeed, ySpeed, rangeX, rangeY, damage, originX, originY, jumpThru; // for parsing
      int speed, range, startPos, blockType; // for switch block
      int switchType, numOfBlocks;

      // enemy specific
      string enemyList; // filename
      List<Enemy> listOfEnemies;
      int enemyType;

      // switch specific
      string switchList;
      List<Switch> listOfSwitches;

      public Block[] ReadLevel(int desiredLevel){
          Directory.SetCurrentDirectory("./Content/levels/");

          // place all text files inside levels folder into string array
          string[] dir = Directory.GetFiles("./");

          // set level equal to desired level
          levelName = dir[desiredLevel];

          try
          {
              readIn = new StreamReader(levelName);

              listOfLines = new List<string[]>();

              // strictly to read each line in the file
              string line = readIn.ReadLine();
              while(line != null)
              {
                  listOfLines.Add(line.Split(',')); // makes each line a string array and adds to list
                  line = readIn.ReadLine();
              }

              // create list of blocks
              //listOfBlocks = new Rectangle[listOfLines.Count];
              listOfBlocks = new Block[listOfLines.Count];

              
              for (int i = 0; i < listOfBlocks.Length; i++)
              {
                  // stationary block
                  if (listOfLines[i].Length <= 5)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out jumpThru);

                      //listOfBlocks[i] = new Rectangle(x, y, width, height);
                      listOfBlocks[i] = new Block(x * 32, y * 32, width * 32, height * 32);
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }
                  }
                  // hazard
                  else if(listOfLines[i].Length == 6)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out damage);
                      int.TryParse(listOfLines[i][5], out jumpThru);

                      listOfBlocks[i] = new Block(x * 32, y * 32, width * 32, height * 32);
                      listOfBlocks[i].Damage = damage;
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }

                  }
                  // speed block (conveyor belts and springs)
                  else if(listOfLines[i].Length == 7)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out xSpeed);
                      int.TryParse(listOfLines[i][5], out ySpeed);
                      int.TryParse(listOfLines[i][6], out jumpThru);

                      listOfBlocks[i] = new SpeedBlock(x * 32, y * 32, width * 32, height * 32, xSpeed, ySpeed);
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }
                  }
                  // switch block
                  else if (listOfLines[i].Length == 8)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out startPos);
                      int.TryParse(listOfLines[i][5], out range);
                      int.TryParse(listOfLines[i][6], out switchType);
                      int.TryParse(listOfLines[i][7], out jumpThru);

                      // manually set x and y speeds and range depending on type
                      if (switchType == 0) { listOfBlocks[i] = new SwitchBlock(x * 32, y * 32, width * 32, height * 32, 16, 0, range * 32, 0, startPos * 32, switchType); }
                      else if (switchType == 1) { listOfBlocks[i] = new SwitchBlock(x * 32, y * 32, width * 32, height * 32, 0, 16, 0, range * 32, startPos * 32, switchType); }
                      
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }
                      

                  }
                  // moving block
                  else if(listOfLines[i].Length == 9)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out xSpeed);
                      int.TryParse(listOfLines[i][5], out ySpeed);
                      int.TryParse(listOfLines[i][6], out rangeX);
                      int.TryParse(listOfLines[i][7], out rangeY);
                      int.TryParse(listOfLines[i][8], out jumpThru);

                      listOfBlocks[i] = new MovingBlock(x * 32, y * 32, width * 32, height * 32, xSpeed, ySpeed, rangeX * 32, rangeY * 32);
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }
                  }
                  else if(listOfLines[i].Length == 11)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out xSpeed);
                      int.TryParse(listOfLines[i][5], out ySpeed);
                      int.TryParse(listOfLines[i][6], out rangeX);
                      int.TryParse(listOfLines[i][7], out rangeY);
                      int.TryParse(listOfLines[i][8], out originX);
                      int.TryParse(listOfLines[i][9], out originY);
                      int.TryParse(listOfLines[i][10], out jumpThru);

                      listOfBlocks[i] = new AssemblyBlock(x * 32, y * 32, width * 32, height * 32, xSpeed, ySpeed, rangeX * 32, rangeY * 32, originX * 32, originY * 32);
                      if (jumpThru == 1) { listOfBlocks[i].IsJumpThru = true; }
                  }

              }

              // reset current direction
              Directory.SetCurrentDirectory("../../");
                  readIn.Close();
          }
          catch(IOException ioe)
          {
              Console.WriteLine("IOException has been thrown: " + ioe.Message);
          }

          return listOfBlocks;
      }

      public List<Enemy> ReadEnemyList(int desiredLevel, Block[] blks, List<Hero> h)
      {
          Directory.SetCurrentDirectory("./Content/enemies/");

          // place all text files inside levels folder into string array
          string[] dir = Directory.GetFiles("./");

          // set level equal to desired level
          enemyList = dir[desiredLevel];

          try
          {
              readIn = new StreamReader(enemyList);

              listOfLines = new List<string[]>();

              // strictly to read each line in the file
              string line = readIn.ReadLine();
              while (line != null)
              {
                  listOfLines.Add(line.Split(',')); // makes each line a string array and adds to list
                  line = readIn.ReadLine();
              }

              // create list of blocks
              listOfEnemies = new List<Enemy>();//Enemy[listOfLines.Count];

              for (int i = 0; i < listOfLines.Count; i++)
              {
                  if (listOfLines[i].Length == 5)
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out enemyType);
                  }
                  else
                  {
                      int.TryParse(listOfLines[i][0], out x);
                      int.TryParse(listOfLines[i][0], out originX);
                      int.TryParse(listOfLines[i][1], out y);
                      int.TryParse(listOfLines[i][2], out width);
                      int.TryParse(listOfLines[i][3], out height);
                      int.TryParse(listOfLines[i][4], out rangeX);
                      int.TryParse(listOfLines[i][5], out enemyType);
                  }

                  // check and see which enemy you're making
                  switch(enemyType)
                  {
                      case 0:
                          listOfEnemies.Add(new Guy(blks, h));
                          break;
                      case 1:
                          listOfEnemies.Add(new WiseGuy(blks, h));
                          break;
                      case 2:
                          listOfEnemies.Add(new ToughGuy(blks, h));
                          break;
                      case 3:
                          listOfEnemies.Add(new WiseToughGuy(blks, h));
                          break;
                      case 4:
                          listOfEnemies.Add(new IntelligentEnemy(blks, h));
                          break;
                      case 5:
                          listOfEnemies.Add(new SmallGuy(blks, h, (rangeX * 32), (originX * 32)));
                          break;
                      default:
                          listOfEnemies.Add(new Guy(blks, h));
                          break;
                  }
                  
                  listOfEnemies[i].EnemyPos = new Rectangle(x*32, y*32, width, height);
              }

              // reset current direction
              Directory.SetCurrentDirectory("../../");
              readIn.Close();
          }
          catch (IOException ioe)
          {
              Console.WriteLine("IOException has been thrown: " + ioe.Message);
          }

          return listOfEnemies;
      }

      public List<Switch> ReadSwitch(int desiredLevel)
      {
          Directory.SetCurrentDirectory("./Content/switches/");

          // place all text files inside levels folder into string array
          string[] dir = Directory.GetFiles("./");

          // set level equal to desired level
          switchList = dir[desiredLevel];

          try
          {
              readIn = new StreamReader(switchList);

              listOfLines = new List<string[]>();

              // strictly to read each line in the file
              string line = readIn.ReadLine();
              while (line != null)
              {
                  listOfLines.Add(line.Split(',')); // makes each line a string array and adds to list
                  line = readIn.ReadLine();
              }

              // create list of blocks
              listOfSwitches = new List<Switch>();//Enemy[listOfLines.Count];

              for (int i = 0; i < listOfLines.Count; i++)
              {
                                   
                  int.TryParse(listOfLines[i][0], out x);
                  int.TryParse(listOfLines[i][1], out y);
                  int.TryParse(listOfLines[i][2], out switchType);
                  int.TryParse(listOfLines[i][3], out numOfBlocks);

                  listOfSwitches.Add(new Switch(x*32, y*32, switchType, numOfBlocks));

                  // set size of block array
                  listOfSwitches[i].MyList = new SwitchBlock[numOfBlocks];
                  for(int j = 0; j < numOfBlocks; j++)
                  {
                      foreach(Block b in listOfBlocks)
                      {
                          // if block is switch block and not already owned, add to array
                          if (b is SwitchBlock && !b.IsOwned)
                          {
                              listOfSwitches[i].MyList[j] = b;
                              b.IsOwned = true;
                          }
                      }
                  }
              }

              // reset current direction
              Directory.SetCurrentDirectory("../../");
              readIn.Close();
          }
          catch (IOException ioe)
          {
              Console.WriteLine("IOException has been thrown: " + ioe.Message);
          }

          return listOfSwitches;
      }

    }
}
