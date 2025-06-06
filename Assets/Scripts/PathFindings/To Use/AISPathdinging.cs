using System.Collections.Generic;
using UnityEngine;

public class AISPathfindind<T> : AISBase<T>
{
    protected List<Vector3> _waypoints;
    int _index;
    protected Transform _entity;
    float _distanceToPoint = 0.2f;
    bool _isFinishPath;
    public Transform target;
    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public override void Exit()
    {
        base.Exit();
    }


    public void SetWaypoints(List<Vector3> newPoints)
    {
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        _index = 0;
        _isFinishPath = false;
        //OnStartPath();
    }
    void Run()
    {
        if (_isFinishPath) return;
        Vector3 point = _waypoints[_index];
        point.y = _entity.position.y;
        Vector3 dir = point - _entity.position;
        if (dir.magnitude < _distanceToPoint)
        {
            if (_index + 1 < _waypoints.Count)
                _index++;
            else
            {
                _isFinishPath = true;
                //OnFinishPath();
                return;
            }
        }
        move.Move(dir.normalized);
    }

    public void SetPathAStarPlusVector() // grilla (creo)
    {
        //goal = Vector3Int.RoundToInt(target.transform.position);
        Vector3 init = Vector3Int.RoundToInt(_entity.transform.position);
        List<Vector3> path = ASTAR.Run<Vector3>(init, IsSatisfied, GetConnections, GetCost, Heuristic);
        path = ASTAR.CleanPath(path, InView);
        Debug.Log("Path " + path.Count);
        //_move.SetPosition(start.transform.position);
        SetWaypoints(path);
    }

    bool InView(Vector3 grandparent, Vector3 child) // grilla
    {
        Debug.Log("INVIEW");
        var diff = child - grandparent;
        return !Physics.Raycast(grandparent, diff.normalized, diff.magnitude, PathfindingConstants.obsMask);
    }

    float Heuristic(Vector3 current) // grilla
    {
        float distanceMultiplier = 1;

        float h = 0;
        h += Vector3.Distance(current, target.transform.position) * distanceMultiplier;
        return h;
    }
    float GetCost(Vector3 parent, Vector3 child) // grilla
    {
        float distanceMultiplier = 1;

        float cost = 0;
        cost += Vector3.Distance(parent, child) * distanceMultiplier;
        return cost;
    }

    bool IsSatisfied(Vector3 curr) // grilla
    {
        if (Vector3.Distance(curr, target.transform.position) > 1.25f) return false;
        return InView(curr, target.transform.position);
    }

    List<Vector3> GetConnections(Vector3 curr) // grilla 
    {
        var neightbourds = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                //if (x == z || x == -z) continue;
                var child = new Vector3(x, 0, z) + curr;
                if (ObstacleManager.Instance.IsRightPos(child))
                {
                    neightbourds.Add(child);
                }
            }
        }
        return neightbourds;
    }
}
