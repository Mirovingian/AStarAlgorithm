using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    [SerializeField] private LayerMask UnwalkableMask;
    [SerializeField] private Vector2 GridWorldSize;
    [SerializeField] private float NodeRadius;
    private Node[,] _grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = NodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        _grid = new Node[gridSizeX, gridSizeY];

        Vector3 leftBottomAngel = transform.position - Vector3.right * (GridWorldSize.x / 2) - Vector3.up * (GridWorldSize.y / 2);
        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPos = leftBottomAngel + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.up * (y * nodeDiameter + NodeRadius);
                _grid[x, y] = new Node(true, worldPos, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(percentX * (gridSizeX - 1));
        int y = Mathf.RoundToInt(percentY * (gridSizeY - 1));

        return _grid[x, y];
    }

    public List<Node> GetNeigbours(Node node)
    {
        List<Node> neigbours = new List<Node>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neigbours.Add(_grid[checkX, checkY]);
            }
        }
        return neigbours;
    }
}
