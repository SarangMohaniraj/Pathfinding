using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(seeker.position, target.position); //seeker or target may be moving so will need to constantly find a new path
        if(grid.path.Capacity > 0)
            seeker.position = Vector3.MoveTowards(seeker.position,grid.path[0].worldPos,.1f);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.GetCurrentNode(startPos);
        Node targetNode = grid.GetCurrentNode(targetPos);

        List<Node> openSet = new List<Node>(); //nodes to be evaluated
        HashSet<Node> closedSet = new HashSet<Node>(); //nodes already evaluated, HashSet has better performance for adding, especially as size increases

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node lowestCost = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < lowestCost.fCost || (openSet[i].fCost == lowestCost.fCost && openSet[i].hCost < lowestCost.hCost) ) //if fCost is the same, compare hCost
                    lowestCost = openSet[i];
            }

            openSet.Remove(lowestCost);
            closedSet.Add(lowestCost);

            if (lowestCost == targetNode) //We found our path!
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(lowestCost))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;
                
                double potentialCost = lowestCost.gCost + lowestCost.DistanceToNode(neighbor); //potential cost of moving to current neighbor node
                if(potentialCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = potentialCost;
                    neighbor.hCost = neighbor.DistanceToNode(targetNode);
                    neighbor.parent = lowestCost;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node targetNode) // work backward from the target node by retracing the path by following each node's parent node until the starting node
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    
}
