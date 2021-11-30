using System;

/**
 * This class is used to generate a random "cave" map.
 * The map is generated as a two-dimensional array of ints, where "0" denotes floor and "1" denotes wall.
 * Initially, the boundaries of the cave are set to "wall", and the inner cells are set at random.
 * Then, a cellular automaton is run in order to smooth out the cave.
 * 
 * Based on Unity tutorial https://www.youtube.com/watch?v=v7yyZZjF1z4 
 * Code by Habrador: https://github.com/Habrador/Unity-Programming-Patterns/blob/master/Assets/Patterns/7.%20Double%20Buffer/Cave/GameController.cs
 * Using a double-buffer technique explained here: https://github.com/Habrador/Unity-Programming-Patterns#7-double-buffer
 * 
 * Adapted by: Erel Segal-Halevi
 * Since: 2020-12
 */
public class CaveGenerator
{
    //Used to init the cellular automata by spreading random dots on a grid,
    //and from these dots we will generate caves.
    //The higher the fill percentage, the smaller the caves.
    protected float randomFillPercent;

    //The height and length of the grid
    protected int gridSize;

    //The double buffer
    private int[,] bufferOld;
    private int[,] bufferNew;

    //========= My Change ===========//
    private int numOfTiles; // number of tiles 

    private Random random;

    //=========== My Change =============//
    public CaveGenerator(int gridSize = 100, int numOfTiles = 1)
    {
        random = new Random();

        this.bufferOld = new int[gridSize, gridSize];
        this.bufferNew = new int[gridSize, gridSize];
        this.gridSize = gridSize;
        this.numOfTiles = numOfTiles;


    }

    public int[,] GetMap()
    {
        return bufferOld;
    }



    /**
     * Generate a random map.
     * The map is not smoothed; call Smooth several times in order to smooth it.
     */
    public void RandomizeMap()
    {
        //Init the old values so we can calculate the new values
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                {
                    //We dont want holes in our walls, so the border is always a wall
                    bufferOld[x, y] = 0;
                }
                else
                {
                    //Random walls and caves
                    bufferOld[x, y] = random.Next(1, numOfTiles);
                }
            }
        }
    }


    /**
     * Generate caves by smoothing the data
     * Remember to always put the new results in bufferNew and use bufferOld to do the calculations
     */
    public void SmoothMap()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //Border is always wall
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                {
                    bufferNew[x, y] = 0;
                    continue;
                }

                //Uses bufferOld to get the wall count
                //=========== My Change =============//
                Tuple<int, int> surroundingtiles = GetSurroundingCount(x, y);

                //Use some smoothing rules to generate caves
                if (surroundingtiles.Item2 > 4)
                {
                    bufferNew[x, y] = surroundingtiles.Item1;
                }
                else
                {
                    bufferNew[x, y] = bufferOld[x, y];
                }
            }
        }

        //Swap the pointers to the buffers
        (bufferOld, bufferNew) = (bufferNew, bufferOld);
    }



    //Given a cell, how many of the 8 surrounding cells are walls?
    private Tuple<int, int> GetSurroundingCount(int cellX, int cellY)
    {
        // =============== My change ==================//
        int[] tilesCounter = new int[numOfTiles];

        for (int neighborX = cellX - 1; neighborX <= cellX + 1; neighborX++)
        {
            for (int neighborY = cellY - 1; neighborY <= cellY + 1; neighborY++)
            {
                //We dont need to care about being outside of the grid because we are never looking at the border
                //This is the cell itself and no neighbor!
                if (neighborX == cellX && neighborY == cellY)
                {
                    continue;
                }

                //This neighbor is a wall
                int tile = bufferOld[neighborX, neighborY];
                tilesCounter[tile]++;
            }
        }

        // =============== My change ==================//
        //find max 
        return findMax(tilesCounter);
    }

    // =============== My change ==================//
    private Tuple<int, int> findMax(int[] arr)
    {
        int index = 0;
        int maxValue = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > maxValue)
            {
                index = i;
                maxValue = arr[i];
            }
        }
        return Tuple.Create(index, maxValue);
    }
}
