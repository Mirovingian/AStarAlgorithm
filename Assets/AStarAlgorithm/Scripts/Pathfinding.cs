using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Node startNode;
    public Node targetNode;

    private Grid m_Grid;

    private void Awake()
    {
        m_Grid = GetComponent<Grid>(); 
    }

    public List<Node> FindPath()
    {
        //Node startNode = m_Grid.NodeFromWorldPoint(StartPoint);
        //Node targetNode = m_Grid.NodeFromWorldPoint(TargetPoint);
        
        Heap<Node> openSet = new Heap<Node>(m_Grid.MaxSize);
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
               

            foreach (Node neigbour in m_Grid.GetNeigbours(currentNode))
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
        if (path.Count > 0) path.RemoveAt(0);
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
