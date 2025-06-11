using UnityEngine;
using System.Collections.Generic;

public class PathfindingBehaviour : FlockingBaseBehaviour
{
    private List<Vector3> _waypoints;
    private int _currentIndex;
    private float _distanceToPoint = 0.8f;
    private Vector3 _target;
    private bool _pathCompleted;

    private Transform _entity;

    private void Awake()
    {
        _entity = transform;
        _currentIndex = 0;
        _pathCompleted = true;
    }

    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_waypoints == null || _waypoints.Count == 0 || _pathCompleted)
            return Vector3.zero;

        Vector3 currentTarget = _waypoints[_currentIndex];
        currentTarget.y = _entity.position.y;

        Vector3 dir = currentTarget - _entity.position;

        if (dir.magnitude < _distanceToPoint)
        {
            if (_currentIndex + 1 < _waypoints.Count)
            {
                _currentIndex++;
                currentTarget = _waypoints[_currentIndex];
                currentTarget.y = _entity.position.y;
                dir = currentTarget - _entity.position;
            }
            else
            {
                _pathCompleted = true;
                Debug.Log("Pathfinding completado con flocking");
                return Vector3.zero;
            }
        }

        return dir.normalized * multiplier;
    }

    public void GeneratePathTo(Vector3 destination)
    {
        _target = destination;
        _pathCompleted = false;

        Vector3 start = Vector3Int.RoundToInt(_entity.position);
        Vector3 goal = Vector3Int.RoundToInt(destination);
        goal.y = start.y;

        List<Vector3> pathResult = ASTAR.Run<Vector3>(start, IsSatisfied, GetConnections, GetCost, Heuristic);
        pathResult = ASTAR.CleanPath(pathResult, HasLineOfSight);

        SetWaypoints(pathResult);
    }

    public void SetWaypoints(List<Vector3> points)
    {
        if (points == null || points.Count == 0)
        {
            _waypoints = null;
            _pathCompleted = true;
            return;
        }

        _waypoints = points;
        _currentIndex = 0;
        _pathCompleted = false;
    }

    private float Heuristic(Vector3 current)
    {
        return Vector3.Distance(current, _target);
    }

    private bool IsSatisfied(Vector3 curr)
    {
        if (Vector3.Distance(curr, _target) > 1.25f) return false;
        return HasLineOfSight(curr, _target);
    }

    private bool HasLineOfSight(Vector3 from, Vector3 to)
    {
        var dir = to - from;
        return !Physics.Raycast(from, dir.normalized, dir.magnitude, PathfindingConstants.obsMask);
    }

    private float GetCost(Vector3 a, Vector3 b) => Vector3.Distance(a, b);

    private List<Vector3> GetConnections(Vector3 pos)
    {
        var connections = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var candidate = pos + new Vector3(x, 0, z);
                if (CollidersManager.instance.IsRightPos(candidate))
                    connections.Add(candidate);
            }
        }
        return connections;
    }

    public bool IsPathCompleted => _pathCompleted;
}