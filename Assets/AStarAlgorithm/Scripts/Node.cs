using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool IsWalkable;
    public Vector3 WorldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node Parent;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}
