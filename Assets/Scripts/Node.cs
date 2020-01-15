using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private Vector3 worldPos;
    private bool walkable; //avoid obstacles
    

    public Node(Vector3 worldPos, bool walkable)
    {
        this.worldPos = worldPos;
        this.walkable = walkable;
    }
}
