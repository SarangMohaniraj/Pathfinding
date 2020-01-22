using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid();
    }

    // Update is called once per frame
    void Update()
    {
        //seeker or target may be moving so will need to constantly find a new path
        Node startNode;
        Node targetNode;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>(); //better performance for adding, especially as size increases

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node lowestCost = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < lowestCost.fCost)
                    lowestCost = openSet[i];
            }

            openSet.Remove(lowestCost);
            closedSet.Add(lowestCost);

            foreach(Node neighbor in lowestCost.getNeighbors())
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;
            }
        }
    }

    void GetPath(List<Node> openSet)
    {
        openSet.Reverse();
    }

}
