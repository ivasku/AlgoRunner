# AlgoRunner
Unity project demonstrating pathfinding algorithms (PC &amp; Mobile)

**Windows build located in the PCBuild directory.**
**Android build (APK) located in the AndroidBuild directory.**

Developed with Unity3d 2019.3.15f1 version.


Game has the following features:
------------
- **a**: It supports 4 Pathfinding algorithm implementation.
- **b**: It supports map generation by random generator or reading maps from TXT files, from the Assets/Maps DIR.
- **c**: It supports configuring the start and end position of the node.
- **d**: Everything is configurable via options in the game.
- **e**: Displays algorithm path statistics after the run is over. During the run button is disabled.

Game contains following implementations:
------------
- **a**: Breadth first Search algorithm implementation.
- **b**: Dijkstra algorithm implementation.
- **c**: Greedy Best-First Search algorithm implementation.
- **d**: AStar algorithm implementation.
- **e**: Home Scene with configurable options.
- **f**: Game Scene with configurable options, statistics and maze legend.
- **g**: Maps can be added via TXT file.

Changing maps via TXT file:
------------
- **a**: GoTo DIR Assets/Maps
- **b**: There you will find some maps present , edit the existing file
- **c**: A map is represented in the TXT format as '0' and '1' , where '0' represent walkable tiles and '1' obstacles
- **d**: Just create a maze you want :).

Assets:
------------
All assets are free for comercial use, no attribution required
