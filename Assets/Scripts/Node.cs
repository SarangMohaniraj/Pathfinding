using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector3 worldPos;
    public bool walkable; //avoid obstacles
    public double gCost { get; }
    public double hCost { get; }
    public double fCost => gCost + fCost;
    public int gridX;
    public int gridY;

    public Node(Vector3 worldPos, bool walkable, int gridX, int gridY)
    {                                           
        this.worldPos = worldPos;
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public List<Node> getNeighbors() //get adjacent nodes
    {
        List<Node> neighbors = new List<Node>();





        return neighbors;
    }

    public double distanceToNode(Node node) //only called for neighbor nodes, so always 45-45-90 triangle
    {
        double distanceX = Mathf.Abs(node.gridX - node.gridY);
        return Mathf.Sqrt(2) * distanceX;
    }
}
