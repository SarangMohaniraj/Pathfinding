using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public float nodeRadius;
    private Node[,] grid;
    private float nodeDiameter { get { return nodeRadius * 2; } }
    private int gridSizeX { get { return Mathf.RoundToInt(gameObject.GetComponent<Renderer>().bounds.size.x / nodeDiameter); } }
    private int gridSizeY { get { return Mathf.RoundToInt(gameObject.GetComponent<Renderer>().bounds.size.z / nodeDiameter); } }

    private void Start()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 origin = transform.position;//bottom left to create coordinate system

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {

            }
        }
    }
    private void OnDrawGizmos()
    {
       Vector3 size = new Vector3(gridSizeX, 1, gridSizeY);
       Gizmos.DrawWireCube(transform.position, size); 
    }
}
