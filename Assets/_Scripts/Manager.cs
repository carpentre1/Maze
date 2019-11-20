using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject cellPrefab;
    List<GameObject> createdCells = new List<GameObject>();

    //cells that the maze generating algorithm has visited
    List<GameObject> visitedCells = new List<GameObject>();

    int mazeMinSize = 3;
    int mazeMaxSize = 100;

    int desiredMazeSize;


    //the total length of the cell prefab squares, for the sake of figuring out how far apart to space them
    float cellLength = 9;

    //the number of cells we have created
    int totalCells = 0;

    // Start is called before the first frame update
    void Start()
    {
        desiredMazeSize = 9;//temporary method of designating maze size before user input functionality is added
        CreateMaze(desiredMazeSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //lay out all the cells in an nXn square
    void CreateMaze(int n)//O(N^2) operation
    {
        if(n < mazeMinSize)
        {
            Debug.Log("Please make a larger maze.");
            return;
        }
        else if(n > mazeMaxSize)
        {
            Debug.Log("Please make a smaller maze.");
            return;
        }
        else
        {
            //make all cells and position them
            for(int x = 0; x < n; x++)
            {
                for(int y = 0; y < n; y++)
                {
                    Vector3 cellPosition = new Vector3(x * cellLength, 0, y * cellLength);
                    CreateCell(cellPosition);
                }
            }
        }

        EstablishAllNeighbors();

    }

    void CreateCell(Vector3 position)
    {
        GameObject newCell = Instantiate(cellPrefab, position, Quaternion.identity);
        createdCells.Add(newCell);
        totalCells++;
        Cell cellScript = newCell.GetComponent<Cell>();
        cellScript.EstablishNumber(totalCells);
        if(cellScript.cellNumber == 1)
        {
            cellScript.CreateStart();
        }
        else if(cellScript.cellNumber == desiredMazeSize * desiredMazeSize)
        {
            //TODO: assign the ending cell dynamically in the maze algorithm instead of here
            cellScript.CreateEnd();
        }
        
    }

    void EstablishAllNeighbors()
    {
        foreach(GameObject cell in createdCells)
        {
            cell.GetComponent<Cell>().EstablishNeighbors();
        }
    }

    void DestroyAllCells()
    {
        foreach(GameObject cell in createdCells)
        {
            
        }
    }
}
