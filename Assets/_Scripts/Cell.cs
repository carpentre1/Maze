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

    //start and end marker prefabs
    public GameObject startPrefab;
    public GameObject endPrefab;

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


    public void EstablishNumber(int cellN)
    {
        cellNumber = cellN;
        name = "Cell " + cellN;
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
        if (NumberOfNeighbors() == 3)
            return true;
        else return false;
    }

    bool IsCorner()
    {
        if (NumberOfNeighbors() == 2)
            return true;
        else return false;
    }

    public void CreateStart()
    {
        startPrefab.SetActive(true);
    }

    public void CreateEnd()
    {
        endPrefab.SetActive(true);
    }

    public void EstablishNeighbors()
    {
        neighborNorth = neighborInDirection(wallNorth);
        neighborSouth = neighborInDirection(wallSouth);
        neighborEast = neighborInDirection(wallEast);
        neighborWest = neighborInDirection(wallWest);
    }

    /// <summary>
    /// Cast an overlap sphere on the cell's wall in the direction where you're looking for a neighbor.
    /// If it finds a different wall, mark that wall's owner as the neighboring cell in that direction.
    /// </summary>
    /// <param name="wall"></param>
    /// <returns></returns>
    public GameObject neighborInDirection(GameObject wall)
    {
        float radius = 1f;
        Collider[] hitColliders = Physics.OverlapSphere(wall.transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.tag == "Wall")
            {
                if(hitColliders[i].gameObject.transform.parent.gameObject == this.gameObject) { continue; }//don't establish the current cell as its neighbor
                else return hitColliders[i].gameObject.transform.parent.gameObject;
            }
        }
        return null;
    }

    public void RemoveWall(GameObject wall)
    {
        if(!wall.activeSelf)
        {
            Debug.Log("Cell " + cellNumber + " tried to remove a nonexistent wall.");
        }
    }
}
