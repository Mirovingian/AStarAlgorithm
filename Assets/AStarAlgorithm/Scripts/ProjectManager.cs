using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum State
{
    None,
    CreatingObstacles,
    CreatingStartNode,
    CreatingTargetNode
}

public class ProjectManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _obstacleMaterial;
    [SerializeField] private Material _pathMaterial;
    [SerializeField] private Material _startNodeMaterial;
    [SerializeField] private Material _targetNodeMaterial;

    [SerializeField] private GameObject _cube;
    private Renderer _renderer;

    private State _state = State.None;
    private Pathfinding _pathfinding;
    private Grid _grid;
    private Dictionary<Vector3, GameObject> _displayedNodes = new Dictionary<Vector3, GameObject>();

    private void Awake()
    {
        _pathfinding = GetComponent<Pathfinding>();
        _grid = GetComponent<Grid>();
        _renderer = _cube.GetComponent<Renderer>();
    }

    private void Update()
    {
        ReadingKeyboardInput();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = _camera.nearClipPlane;
            Vector3 worldMousePos = _camera.ScreenToWorldPoint(mouseScreenPos);
            worldMousePos.z = 0f;

            Node currentNode = _grid.NodeFromWorldPoint(worldMousePos);
            switch (_state)
            {
                case State.CreatingObstacles:
                    SetObstacle(currentNode);
                    break;
                case State.CreatingStartNode:
                    SetStartNode(currentNode);
                    break;
                case State.CreatingTargetNode:
                    SetTargetNode(currentNode);
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            DisplayPath(_pathfinding.FindPath());
        }
    }

    private void SetObstacle(Node node)
    {
        if (node.IsWalkable)
        {
            HideNodeOnScene(node.WorldPosition);
            node.IsWalkable = false;
            DisplayNodeOnScene(node.WorldPosition, _obstacleMaterial);
        }
        else
        {
            HideNodeOnScene(node.WorldPosition);
            node.IsWalkable = true;
            HideNodeOnScene(node.WorldPosition);  
        }
    }

    private void SetStartNode(Node node)
    {
        if (node.IsWalkable)
        {
            HideNodeOnScene(node.WorldPosition);
            if (_pathfinding.startNode != null) HideNodeOnScene(_pathfinding.startNode.WorldPosition);
            _pathfinding.startNode = node;
            DisplayNodeOnScene(node.WorldPosition, _startNodeMaterial);   
        }
        else
        {
            node.IsWalkable = true;
            HideNodeOnScene(node.WorldPosition);
            if (_pathfinding.startNode != null) HideNodeOnScene(_pathfinding.startNode.WorldPosition);
            _pathfinding.startNode = node;
            DisplayNodeOnScene(node.WorldPosition, _startNodeMaterial);   
        }
    }

    private void SetTargetNode(Node node)
    {
        if (node.IsWalkable)
        {
            HideNodeOnScene(node.WorldPosition);
            if (_pathfinding.targetNode != null) HideNodeOnScene(_pathfinding.targetNode.WorldPosition);
            _pathfinding.targetNode = node;
            DisplayNodeOnScene(node.WorldPosition, _targetNodeMaterial);
        }
        else
        {
            node.IsWalkable = true;
            HideNodeOnScene(node.WorldPosition);
            if (_pathfinding.targetNode != null) HideNodeOnScene(_pathfinding.targetNode.WorldPosition);
            _pathfinding.targetNode = node;
            DisplayNodeOnScene(node.WorldPosition, _targetNodeMaterial);
        }
    }

    private List<Node> _currentPath;
    private void DisplayPath(List<Node> path)
    {
        HidePath(_currentPath);

        _currentPath = path;
        if (_currentPath == null) return;

        foreach (var node  in _currentPath)
        {
            DisplayNodeOnScene(node.WorldPosition, _pathMaterial);
        }
    }

    private void HidePath(List<Node> path)
    {
        if (path == null) return;

        foreach (var node in path)
        {
            if (node.IsWalkable && node != _pathfinding.startNode && node != _pathfinding.targetNode) 
                HideNodeOnScene(node.WorldPosition);
        }
    }

    private void DisplayNodeOnScene(Vector3 pos, Material material)
    {
        if (!_displayedNodes.ContainsKey(pos))
        {
            _renderer.material = material;
            _displayedNodes.Add(pos, Instantiate(_cube, pos, Quaternion.identity));
        }
    }

    private void HideNodeOnScene(Vector3 pos)
    {
        if (_displayedNodes.ContainsKey(pos))
        {
            Destroy(_displayedNodes[pos]);
            _displayedNodes.Remove(pos);
        }
    }

    private void ReadingKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _state = State.CreatingObstacles;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _state = State.CreatingStartNode;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _state = State.CreatingTargetNode;
        }
    }
}
