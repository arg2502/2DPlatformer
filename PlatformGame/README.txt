2D Platformer Test
Alec Gerchman & Eric Stivala

This was a personal project that I worked on with my friend, Eric. We mainly wanted to see if we could create the basic mechanics of a classic platformer and have it feel satisfying.

The level and enemies are passed in through text files, read in, and created.

The player character is created through a class hierarchy so it would be easy to create different characters to make multiplayer possible.

The enemies are also in their own class hierarchy so that there can be a diverse selection of enemies with minimal coding effort.

There is an item inventory system that just prints out the locations in the 2D array when the items are collected. The items are placed in a temporary inventory until a checkpoint is reached or the level is completed so that way the items are lost if the player dies. (Items serve no purpose as of current version)

All artwork was done by Eric Stivala.

Controls:
A - Left
D - Right
W - Jump

Types of Enemies:
Small Guy - Red. Goes back and forth in a path. Jump on him to kill him. Gives one item.
Guy - Red. Chases you when you get close. Jump on him to kill him. Gives two items.
Wise Guy - Yellow. Chases you when you get close, but won't fall off edge. Jump on him to kill him. 		Gives two items.
Tough Guy - Blue with spikes. Chases you when you get close. You get hurt if you jump on him. Trick him 	into falling off.
Wise Tough Guy - Looks and acts like Wise Guy but also acts like Tough Guy. Cannot kill him.
Intelligent Guy - Black. Chases you when you get close. Will jump when it reaches end of platform. Not 		always a perfect jump. 

Types of Items:
0 - Gold Nugget
1 - Red Jelly
2 - Eyes
3 - Brain Matter
4 - Blue Jelly
5 - Spikes
6 - Gold Bars


Executable File: PlatformerTest/PlatformerTest/bin/WindowsGL/Debug/PlatformerTest.exe

File IO Source:
	Levels: PlatformerTest/PlatformerTest/bin/WindowsGL/Debug/Content/levels
	Enemies: PlatformerTest/PlatformerTest/bin/WindowsGL/Debug/Content/enemies