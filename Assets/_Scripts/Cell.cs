using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    Manager manager;

    GameObject floor;
    Color defaultFloorColor;

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

    public GameObject pathMarker;

    //references to the neighbor cells, assigned after all cells are placed
    public GameObject neighborNorth;
    public GameObject neighborSouth;
    public GameObject neighborEast;
    public GameObject neighborWest;

    public List<GameObject> neighbors = new List<GameObject>();
    public List<GameObject> unvisitedNeighbors = new List<GameObject>();


    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        floor = transform.Find("Floor").gameObject;
        defaultFloorColor = floor.GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPathMarker(int n)
    {
        pathMarker.GetComponent<TextMeshPro>().text = n.ToString();
        pathMarker.SetActive(true);
    }


    public void EstablishNumber(int cellN)
    {
        cellNumber = cellN;
        name = "Cell " + cellN;
    }

    int NumberOfNeighbors()
    {
        int i = 0;
        if (neighborNorth) { i++;  }
        if (neighborSouth) { i++; }
        if (neighborEast) { i++; }
        if (neighborWest) { i++; }
        return i;
    }

    public bool IsEdge()
    {
        if (NumberOfNeighbors() == 3)
            return true;
        else return false;
    }

    public bool IsCorner()
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

        if (neighborNorth) { neighbors.Add(neighborNorth); }
        if (neighborSouth) { neighbors.Add(neighborSouth); }
        if (neighborEast) { neighbors.Add(neighborEast); }
        if (neighborWest) { neighbors.Add(neighborWest); }

        unvisitedNeighbors = neighbors;
    }

    public void UpdateNeighbors()
    {
        List<GameObject> cellsToRemove = new List<GameObject>();
        foreach (GameObject c in unvisitedNeighbors)
        {
            if(c.GetComponent<Cell>().hasBeenVisited)
            {
                cellsToRemove.Add(c);
            }
        }
        foreach(GameObject c in cellsToRemove)
        {
            unvisitedNeighbors.Remove(c);
        }
    }

    public bool HasUnvisitedNeighbor()
    {
        if(neighborNorth)
        {
            if (!neighborNorth.GetComponent<Cell>().hasBeenVisited) { return true; }
        }
        if (neighborSouth)
        {
            if (!neighborSouth.GetComponent<Cell>().hasBeenVisited) { return true; }
        }
        if (neighborEast)
        {
            if (!neighborEast.GetComponent<Cell>().hasBeenVisited) { return true; }
        }
        if (neighborWest)
        {
            if (!neighborWest.GetComponent<Cell>().hasBeenVisited) { return true; }
        }


        return false;
    }

    public void HighlightCell(bool highlight)
    {
        if(highlight)
        {
            floor.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            floor.GetComponent<MeshRenderer>().material.color = defaultFloorColor;
        }
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

    public void CarvePath(GameObject neighbor)
    {
        Cell neighborCellScript = neighbor.GetComponent<Cell>();
        hasBeenVisited = true;
        neighborCellScript.hasBeenVisited = true;
        manager.timesCarved++;
        AddPathMarker(manager.timesCarved);
        unvisitedNeighbors.Remove(neighbor);
        neighborCellScript.unvisitedNeighbors.Remove(gameObject);
        if (neighbor == neighborNorth)
        {
            neighborCellScript.RemoveWall(neighborCellScript.wallSouth);
            RemoveWall(wallNorth);
        }
        if (neighbor == neighborSouth)
        {
            neighborCellScript.RemoveWall(neighborCellScript.wallNorth);
            RemoveWall(wallSouth);
        }
        if (neighbor == neighborEast)
        {
            neighborCellScript.RemoveWall(neighborCellScript.wallWest);
            RemoveWall(wallEast);
        }
        if (neighbor == neighborWest)
        {
            neighborCellScript.RemoveWall(neighborCellScript.wallEast);
            RemoveWall(wallWest);
        }
    }

    public void RemoveWall(GameObject wall)
    {
        if(!wall.activeSelf)
        {
            Debug.Log("Cell " + cellNumber + " tried to remove a nonexistent wall.");
        }

        wall.SetActive(false);
    }
}
