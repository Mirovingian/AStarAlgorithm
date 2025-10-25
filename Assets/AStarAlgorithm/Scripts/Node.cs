using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool IsWalkable;
    public Vector3 WorldPosition;

    public Node(bool isWalkable, Vector3 worldPosition)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPosition;
    }
}
