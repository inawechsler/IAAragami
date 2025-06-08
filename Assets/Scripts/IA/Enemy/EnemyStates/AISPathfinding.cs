using System.Collections.Generic;
using UnityEngine;

public class AISPathfinding<T> : AISBase<T>
{
    protected List<Vector3> _waypoints;
    int _index;
    protected Transform _entity;
    public float _distanceToPoint = 0.8f;
    public Vector3 target;

    public AISPathfinding(Vector3 target)
    {
        this.target = target;
        _index = 0;
    }

    public override void Enter()
    {
        base.Enter();
        _entity = controller.transform;
        GeneratePath();
        path.isFinishPath = false;
    }


    public override void Execute()
    {
        base.Execute();

        if (path.isFinishPath|| _waypoints == null || _waypoints.Count == 0)
        {
            return;
        }

        Vector3 currentTarget = _waypoints[_index];
        currentTarget.y = _entity.position.y;
        Vector3 dir = currentTarget - _entity.position;

        // Si llegó al waypoint actual, avanzar al siguiente
        if (dir.magnitude < _distanceToPoint)
        {
            if (_index + 1 < _waypoints.Count)
            {
                _index++;
            }
            else
            {
                path.isFinishPath = true;
                path.isOnPathfinding = false;

                Debug.Log("Pathfinding completado, llegó al destino");
                return;
            }
        }
        Debug.Log("Path");
        move.Move(dir.normalized);
        look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
        _index = 0;
    }

    void GeneratePath()
    {
        if (_entity == null)
        {
            Debug.LogWarning("Entity es null, no se puede generar path");
            return;
        }

        GeneratePath(target);
    }
    void GeneratePath(Vector3 destination)
    {
        Vector3 start = Vector3Int.RoundToInt(_entity.position);
        Vector3 goal = Vector3Int.RoundToInt(destination);
        goal.y = start.y;
        List<Vector3> pathResult = ASTAR.Run<Vector3>(start, IsSatisfied, GetConnections, GetCost, Heuristic);
        pathResult = ASTAR.CleanPath(pathResult, HasLineOfSight);
        SetWaypoints(pathResult);
    }

    float Heuristic(Vector3 current)
    {
        return Vector3.Distance(current, target);
    }

    bool IsSatisfied(Vector3 curr)
    {
        if (Vector3.Distance(curr, target) > 1.25f) return false;
        return HasLineOfSight(curr, target);
    }

    public void SetWaypoints(List<Vector3> points)
    {
        if (points == null || points.Count == 0)
        {
            _waypoints = null;
            if (path != null)
                path.isFinishPath = true;
            return;
        }

        _waypoints = points;
        _index = 0;
        if (path != null)
            path.isFinishPath = false;
    }

    bool HasLineOfSight(Vector3 from, Vector3 to)
    {
        var dir = to - from;
        return !Physics.Raycast(from, dir.normalized, dir.magnitude, PathfindingConstants.obsMask);
    }

    float GetCost(Vector3 a, Vector3 b) => Vector3.Distance(a, b);

    List<Vector3> GetConnections(Vector3 pos)
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

}