using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    Manager manager;

    public bool hasBeenVisited = false;
    public int cellNumber;

    //references to the walls of each cell, assigned in the prefab's inspector:
    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;

    //references to the neighbor cells, assigned after all cells are placed
    public GameObject neighborNorth;
    public GameObject neighborSouth;
    public GameObject neighborEast;
    public GameObject neighborWest;


    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int NumberOfNeighbors()
    {
        int i = 0;
        if (neighborNorth) { i++; }
        if (neighborSouth) { i++; }
        if (neighborEast) { i++; }
        if (neighborWest) { i++; }
        return i;
    }

    bool IsEdge()
    {
        if (NumberOfNeighbors() <= 3)
            return true;
        else return false;
    }

    bool IsCorner()
    {
        if (NumberOfNeighbors() <= 2)
            return true;
        else return false;
    }

    public void RemoveWall(GameObject wall)
    {
        if(!wall.activeSelf)
        {
            Debug.Log("Cell " + cellNumber + " tried to remove a nonexistent wall.");
        }
    }
}
