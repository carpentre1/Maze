# Maze
 Generating mazes that are then visualized in Unity.
 
This is my plan for creating a maze generator in C#, using Unity to visualize the maze. 

Unfortunately, I had already looked around at maze generation concepts before you recommended that we go in blind, so my algorithm will be tainted by prior knowledge - but I didn’t look into it much, just got the general idea of how they go about it. 

Each cell of the maze will be a prefab in Unity, with each cell containing a ‘floor’ plane (the empty, traversable space) and 4 walls. As a result, there will be overlapping walls on connecting cells (for example Cell A’s north wall and Cell B’s south wall), so both sets of walls will need to be removed when creating a path. I could, alternatively, not include walls in the prefab, and only instantiate them after laying out the cell prefabs. I’ll figure out which method works better when actually building the algorithm and visualization. 

Users will be able to enter a number N that will determine the maze size, and Unity will create the Cell prefabs in a square shape that is N cells long and N cells wide. Each cell will store references to its neighbor cells in up to 4 directions (as few as 2 if it’s a corner cell). 

Pseudocode for how it will carve paths through the cells: 

Mark an edge cell as ‘start’. From there, pick a neighbor cell and carve a path to it. Add this cell to the “visited” list. 

Pick a new neighbor from the current position that is unvisited. Carve a path. If the new neighbor is an edge cell and the algorithm has traveled more than N/2 times, mark this new neighbor as ‘end’. If not, keep repeating the process until this condition is met. 

Travel backwards from the end cell to the next previously visited cell. If that cell has any unvisited neighbors: 

Pick an unvisited neighbor, carve a path to it, add it to the visited list. Repeat until there are no unvisited cells. 

Repeat step 3 until all cells along the path from start to end are exhausted. 

 

I’m sure there are various flaws in this pseudocode that I’ll iron out while implementing it. I chose N/2 for the distance that needs to be travelled before marking an edge cell as the end because otherwise the mazes generated could have an extremely short correct path. That number probably has lots of room to be tweaked. 
