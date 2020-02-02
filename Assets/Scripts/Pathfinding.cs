using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    public bool stopwatchIsActive;
    bool stopwatchInitial = true;
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
        if (grid.path.Capacity > 0) //move and rotate seeker toward next node in path
            Move(seeker, grid.path[0].worldPos);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch stopwatch = new Stopwatch();
        if (stopwatchIsActive && stopwatchInitial)
            stopwatch.Start();

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
                if (openSet[i].FCost < lowestCost.FCost || (openSet[i].FCost == lowestCost.FCost && openSet[i].HCost < lowestCost.HCost) ) //if fCost is the same, compare hCost
                    lowestCost = openSet[i];
            }

            openSet.Remove(lowestCost);
            closedSet.Add(lowestCost);

            if (lowestCost == targetNode) //We found our path!
            {
                RetracePath(startNode, targetNode);
                if (stopwatchIsActive && stopwatchInitial) { 
                    stopwatch.Stop();
                    stopwatchInitial = false;
                    UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds + " ms");
                }
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(lowestCost))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;
                
                double potentialCost = lowestCost.GCost + lowestCost.DistanceToNode(neighbor) + neighbor.movementPenalty; //potential cost of moving to current neighbor node
                if(potentialCost < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = potentialCost;
                    neighbor.HCost = neighbor.DistanceToNode(targetNode);
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

    void Move(Transform seeker, Vector3 nextPos)
    {
        
        float speed = 5 / (Mathf.Pow(grid.GetCurrentNode(seeker.position).movementPenalty, 2) + 30f);

        nextPos.y = seeker.position.y;
        seeker.position = Vector3.MoveTowards(seeker.position, nextPos, speed);

        //find the vector pointing from our position to the target
        Vector3 direction = (nextPos - seeker.position).normalized;
        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //rotate us over time according to speed until we are in the required rotation
        seeker.rotation = Quaternion.Slerp(seeker.rotation, lookRotation, Time.deltaTime);

    }

    
}
