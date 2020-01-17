using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector3 worldPos;
    public bool walkable; //avoid obstacles
    

    public Node(Vector3 worldPos, bool walkable)
    {
        this.worldPos = worldPos;
        this.walkable = walkable;
    }
}
