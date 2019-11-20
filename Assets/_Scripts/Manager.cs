using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject cellPrefab;
    List<GameObject> createdCells = new List<GameObject>();

    //cells that the maze generating algorithm has visited
    List<GameObject> visitedCells = new List<GameObject>();

    //the total length of the cell prefab squares, for the sake of figuring out how far apart to space them
    float cellLength = 9;

    //the number of cells we have created
    int totalCells = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMaze(int n)
    {

    }

    void CreateCell()
    {
        GameObject newCell = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        createdCells.Add(newCell);
        totalCells++;
        newCell.GetComponent<Cell>().cellNumber = totalCells;
    }

    void DestroyAllCells()
    {
        foreach(GameObject cell in createdCells)
        {
            
        }
    }
}
