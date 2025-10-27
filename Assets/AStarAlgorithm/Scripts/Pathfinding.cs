using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>(); 
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
               

            foreach (Node neigbour in _grid.GetNeigbours(currentNode))
            {
                if (!neigbour.IsWalkable || closeSet.Contains(neigbour))
                    continue;

                int newMovementCostToNeigbour = currentNode.gCost + GetDistance(currentNode, neigbour);
                if (newMovementCostToNeigbour < neigbour.gCost || !openSet.Contains(neigbour))
                {
                    neigbour.gCost = newMovementCostToNeigbour;
                    neigbour.hCost = GetDistance(neigbour, targetNode);
                    neigbour.Parent = currentNode;

                    if (!openSet.Contains(neigbour))
                    {
                        openSet.Add(neigbour);
                    }
                }

            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    public int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
