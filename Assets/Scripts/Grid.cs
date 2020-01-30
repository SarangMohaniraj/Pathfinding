using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform seeker;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public Vector2 gridWorldSize;
    Node[,] grid;
    public List<Node> path;

    float nodeDiameter { get { return nodeRadius * 2; } }
    int gridSizeX => Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
    int gridSizeY => Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    private void Awake()
    {
        //scalar*vector results vector
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 origin = transform.position - (gridSizeX/2 * Vector3.right) - (gridSizeY/2 * Vector3.forward);//bottom left to create coordinate system

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPos = origin + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * ( y * nodeDiameter + nodeRadius); //center of each node, not bottom left
                grid[x, y] = new Node(worldPos, !Physics.CheckSphere(worldPos,nodeRadius,unwalkableMask),x,y);
            }
        }
    }

    public Node GetCurrentNode(Vector3 worldPos) //get the node the player is standing on
    {
        //position given by percentages from the origin to the upper bounds
        float percentX = Mathf.Clamp01( (worldPos.x + gridSizeX/2) / gridSizeX );
        float percentY = Mathf.Clamp01((worldPos.y + gridSizeY / 2) / gridSizeY);

        //node position in grid array
        int x = Mathf.RoundToInt(percentX * (gridSizeX - 1));
        int y = Mathf.RoundToInt(percentY * (gridSizeY - 1));

        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node) //get adjacent nodes (up to 8)
    {
        List<Node> neighbors = new List<Node>();

        //check 8 adjacent nodes relative to current node 
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) //skips center since that is the relative origin
                    continue;

                int nx = node.gridX + x;
                int ny = node.gridY + y;

                if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
                    neighbors.Add(grid[nx, ny]);

            }
        }
        return neighbors;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 1, gridSizeY));

        Vector3 origin = transform.position - (gridSizeX / 2 * Vector3.right) - (gridSizeY / 2 * Vector3.forward);//bottom left to create coordinate system
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPos = origin + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); //center of each node, not bottom left
                Gizmos.color = !Physics.CheckSphere(worldPos, nodeRadius, unwalkableMask) ? Color.green : Color.red;

                //foreach (Node node in path)
                //{
                //    Debug.Log(node);
                //}
                Debug.Log(path == null);
                if (path != null && grid != null && path.Contains(GetCurrentNode(worldPos)))
                    Gizmos.color = Color.black;
                Gizmos.DrawWireCube(worldPos, Vector3.one * nodeDiameter);
            }
        }
    }
}
