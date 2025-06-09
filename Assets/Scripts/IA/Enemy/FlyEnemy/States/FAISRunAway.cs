using UnityEngine;

public class FAISRunAway<T> : AISBase<T>
{
    private Vector3 _target;
    private ISteering _steering;
    private PathfindingBehaviour _pathfindingBehaviour;
    public FAISRunAway(ISteering steering, Vector3 target, LeaderBehaviour leader, PathfindingBehaviour pathfindingBehaviour)
    {
        _steering = steering;
        _target = target;
        _pathfindingBehaviour = pathfindingBehaviour;
        leader.IsActive = false;
    }

    public override void Enter()
    {
        base.Enter();

        _pathfindingBehaviour.IsActive = true;

        _pathfindingBehaviour.GeneratePathTo(_target);

        if (path != null)
        {
            path.isFinishPath = false;
            path.isOnPathfinding = true;
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (path.isFinishPath) return;
        if (_pathfindingBehaviour.IsPathCompleted)
        {
            if (path != null)
            {
                path.isFinishPath = true;
                path.isOnPathfinding = false;
            }
            Debug.Log("Pathfinding con flocking completado");
            return;
        }

        

        var steeringDir = _steering.GetDir();
        steeringDir.y = 0;

        var finalDir = obs.GetDir(steeringDir);

        if (finalDir.magnitude > 0.1f)
        {
            move.Move(finalDir.normalized);
            look.LookDir(finalDir.normalized);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _pathfindingBehaviour.IsActive = false;
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        _target = newTarget;
        if (_pathfindingBehaviour.IsActive)
        {
            _pathfindingBehaviour.GeneratePathTo(_target);
        }
    }
}

