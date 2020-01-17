using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter { get { return nodeRadius * 2; } }
    int gridSizeX { get { return Mathf.RoundToInt(gameObject.GetComponent<Renderer>().bounds.size.x / nodeDiameter); } }
    int gridSizeY { get { return Mathf.RoundToInt(gameObject.GetComponent<Renderer>().bounds.size.z / nodeDiameter); } }

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
                grid[x, y] = new Node(worldPos, !Physics.CheckSphere(worldPos,nodeRadius,unwalkableMask));
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 1, gridSizeY));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * nodeDiameter);
            }
        }
    }
}
