using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public bool showOnlyPath;
    public Vector2 gridWorldSize;
    Node[,] grid;
    public List<Node> path;
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsMovementPenalties; //dictionary is faster than looping through array to search

    float NodeDiameter { get { return nodeRadius * 2; } }
    int GridSizeX => Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
    int GridSizeY => Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);

    private void Start()
    {
        CreateTerrainTypes();
        CreateGrid();
    }

    void CreateGrid()
    {
        //scalar*vector results vector
        grid = new Node[GridSizeX, GridSizeY];
        Vector3 origin = transform.position - (GridSizeX / 2 * Vector3.right) - (GridSizeY / 2 * Vector3.forward);//bottom left to create coordinate system

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPos = origin + Vector3.right * (x * NodeDiameter + nodeRadius) + Vector3.forward * (y * NodeDiameter + nodeRadius); //center of each node, not bottom left
                bool walkable = !Physics.CheckSphere(worldPos, nodeRadius, unwalkableMask);
                
                int movementPenalty = 0;
                if (walkable)
                {
                    Ray ray = new Ray(worldPos + Vector3.up * 100f, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableMask))
                        walkableRegionsMovementPenalties.TryGetValue(hit.collider.gameObject.layer, out movementPenalty); //change movementPenalty based on walkable region layer
                }

                grid[x, y] = new Node(worldPos, walkable, x, y, movementPenalty);
            }
        }
    }

    public Node GetCurrentNode(Vector3 worldPos) //get the node the player is standing on
    {
        //position given by percentages from the origin to the upper bounds
        float percentX = Mathf.Clamp01( (worldPos.x + GridSizeX / 2) / GridSizeX );
        float percentY = Mathf.Clamp01( (worldPos.z + GridSizeY / 2) / GridSizeY );

        //node position in grid array
        int x = Mathf.RoundToInt(percentX * (GridSizeX - 1));
        int y = Mathf.RoundToInt(percentY * (GridSizeY - 1));

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

                if (nx >= 0 && nx < GridSizeX && ny >= 0 && ny < GridSizeY) //checks if it is in bounds
                    neighbors.Add(grid[nx, ny]);

            }
        }
        return neighbors;
    }

    //must keep Gizmos on through the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(!showOnlyPath)
            Gizmos.DrawWireCube(transform.position, new Vector3(GridSizeX, 1, GridSizeY));

        Vector3 origin = transform.position - (GridSizeX / 2 * Vector3.right) - (GridSizeY / 2 * Vector3.forward);//bottom left to create coordinate system
        
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPos = origin + Vector3.right * (x * NodeDiameter + nodeRadius) + Vector3.forward * (y * NodeDiameter + nodeRadius); //center of each node, not bottom left

                if (!showOnlyPath)
                {
                    Gizmos.color = !Physics.CheckSphere(worldPos, nodeRadius, unwalkableMask) ? Color.green : Color.red;

                    if (path != null && grid != null && path.Contains(GetCurrentNode(worldPos)))
                        Gizmos.color = Color.black;
                    Gizmos.DrawWireCube(worldPos, Vector3.one * NodeDiameter);
                }
                else
                {
                    if (path != null && grid != null && path.Contains(GetCurrentNode(worldPos)))
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawWireCube(worldPos, Vector3.one * NodeDiameter);
                    }
                }
            }
        }
    }
    void CreateTerrainTypes()
    {
        walkableRegionsMovementPenalties = new Dictionary<int, int>();
        //LayerMask is a 32 bit number consisting of true or false for each of the 32
        foreach (TerrainType terrainType in walkableRegions)
        {
            walkableMask |= terrainType.terrainMask.value; //Bitwise Logical OR operator to simplify ALL layers in TerrainType[] to one LayerMask
            walkableRegionsMovementPenalties.Add((int)Mathf.Log(terrainType.terrainMask.value, 2), terrainType.movementPenalty); // Convert 32 bit to layer number
        }
    }

    [System.Serializable] //so the class can show up in the Editor
    public class TerrainType
    {
        public LayerMask terrainMask; //make sure to only select one layer per terrain type
        public int movementPenalty;
    }
}
