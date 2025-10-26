using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform StartPoint;
    public Transform TargetPoint;

    private Grid m_Grid;

    private void Awake()
    {
        m_Grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            FindPath(StartPoint.position, TargetPoint.position);
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = m_Grid.NodeFromWorldPoint(startPos);
        Node targetNode = m_Grid.NodeFromWorldPoint(targetPos);
        
        Heap<Node> openSet = new Heap<Node>(m_Grid.MaxSize);
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
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
    }

    public void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        m_Grid.Path = path;
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
