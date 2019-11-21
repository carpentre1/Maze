using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject camera;

    public GameObject cellPrefab;
    List<GameObject> createdCells = new List<GameObject>();
    List<GameObject> unvisitedCells = new List<GameObject>();

    //cells that the maze generating algorithm has visited
    List<GameObject> cellsToVisit = new List<GameObject>();

    int mazeMinSize = 3;
    int mazeMaxSize = 100;

    public float mazeGenerationSpeed = .02f;
    public float mazePathCarvingSpeed = .2f;

    public float pathCarvingSpeed = 1;
    public float generationSpeed = 1;

    public int desiredMazeSize = 9;

    public int timesCarved = 0;

    GameObject startCell;


    //the total length of the cell prefab squares, for the sake of figuring out how far apart to space them
    float cellLength = 9;

    //the number of cells we have created so far
    int totalCells = 0;

    //the number of cells we will have once they're all created
    int maxCells = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxCells = desiredMazeSize * desiredMazeSize;
        mazePathCarvingSpeed = (5f / maxCells) * (1 / pathCarvingSpeed);
        mazeGenerationSpeed = (.001f / maxCells) * (1 / generationSpeed);
        Debug.Log("path carve: " + mazePathCarvingSpeed);
        Debug.Log("gen: " + mazeGenerationSpeed);
        SetCameraPosition();
        StartCoroutine(CreateMazeCoroutine(desiredMazeSize));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCameraPosition()
    {
        camera.transform.position = new Vector3(maxCells * .3f, maxCells * .6f, maxCells * .3f);
    }

    //lay out all the cells in an nXn square
    IEnumerator CreateMazeCoroutine(int n)//O(N^2) operation
    {
        if(n < mazeMinSize)
        {
            Debug.Log("Please make a larger maze.");
            yield break;
        }
        else if(n > mazeMaxSize)
        {
            Debug.Log("Please make a smaller maze.");
            yield break;
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
                    yield return new WaitForSeconds(mazeGenerationSpeed);
                }
            }
        }

        EstablishAllNeighbors();
        StartCoroutine(CarveUntilCornerCoroutine());

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
            startCell = newCell;
        }
        else if(cellScript.cellNumber == desiredMazeSize * desiredMazeSize)
        {
            //TODO: assign the ending cell dynamically in the maze algorithm instead of here
            //cellScript.CreateEnd();
        }
        
    }
    
    //this function isn't being used; it doesn't work, but I'm not ready to throw out what I've written
    void CreateMazePath()
    {
        if(!startCell)
        {
            Debug.Log("No starting cell designated.");
        }
        Cell cellScript = startCell.GetComponent<Cell>();
        cellsToVisit.Add(startCell);
        while(cellsToVisit.Count > 0)
        {


            GameObject nextCell = cellScript.neighbors[Random.Range(0, cellScript.neighbors.Count)];
            cellScript.CarvePath(nextCell);
            cellsToVisit.Add(nextCell);
            if(!cellScript.HasUnvisitedNeighbor())
            {
                cellsToVisit.Remove(cellScript.gameObject);
            }
        }

    }

    /// <summary>
    /// This function keeps carving a path through randomly chosen unvisited neighbor cells until it reaches a corner cell.
    /// If it isn't at a corner but has no unvisited neighbor cells, it backtracks until it finds a previously visited cell with new unvisited neighbors.
    /// </summary>
    /// <returns></returns>
    IEnumerator CarveUntilCornerCoroutine()
    {
        Stack visitedCells = new Stack();
        Cell cellScript = startCell.GetComponent<Cell>();
        GameObject nextCell = cellScript.neighbors[Random.Range(0, cellScript.neighbors.Count)];
        cellScript.CarvePath(nextCell);
        visitedCells.Push(startCell);

        while(!nextCell.GetComponent<Cell>().IsCorner())
        {
            yield return new WaitForSeconds(mazePathCarvingSpeed);
            visitedCells.Push(nextCell);
            cellScript = nextCell.GetComponent<Cell>();
            if(cellScript.unvisitedNeighbors.Count == 0)
            {
                Debug.Log("Backtracking from " + cellScript.gameObject.name);
                visitedCells.Pop();
                nextCell = (GameObject)visitedCells.Peek();
                while (nextCell.GetComponent<Cell>().unvisitedNeighbors.Count == 0)
                {
                    visitedCells.Pop();
                    nextCell = (GameObject)visitedCells.Peek();
                    nextCell.GetComponent<Cell>().UpdateNeighbors();
                }
                continue;
            }
            nextCell = cellScript.unvisitedNeighbors[Random.Range(0, cellScript.neighbors.Count)];
            nextCell.GetComponent<Cell>().UpdateNeighbors();
            cellScript.CarvePath(nextCell);
        }
        nextCell.GetComponent<Cell>().CreateEnd();

        //keep backtracking and filling in unvisited cells
        while(cellScript.unvisitedNeighbors.Count != 0)
        {
            yield return new WaitForSeconds(mazePathCarvingSpeed);
            visitedCells.Push(nextCell);
            cellScript = nextCell.GetComponent<Cell>();
            if (cellScript.unvisitedNeighbors.Count == 0)
            {
                Debug.Log("Backtracking from " + cellScript.gameObject.name);
                visitedCells.Pop();
                nextCell = (GameObject)visitedCells.Peek();
                while (nextCell.GetComponent<Cell>().unvisitedNeighbors.Count == 0)
                {
                    visitedCells.Pop();
                    nextCell = (GameObject)visitedCells.Peek();
                    nextCell.GetComponent<Cell>().UpdateNeighbors();
                }
                continue;
            }
            nextCell = cellScript.unvisitedNeighbors[Random.Range(0, cellScript.neighbors.Count)];
            nextCell.GetComponent<Cell>().UpdateNeighbors();
            cellScript.CarvePath(nextCell);
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
