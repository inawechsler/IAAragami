using UnityEngine;

public class FAISRunAway<T> : AISBase<T>
{
    private Vector3 _target;
    private ISteering _steering;
    private PathfindingBehaviour _pathfindingBehaviour;
    private LeaderBehaviour _leaderBehaviour;

    public FAISRunAway(ISteering steering, Vector3 target, LeaderBehaviour leader, PathfindingBehaviour pathfindingBehaviour)
    {
        _steering = steering;
        _target = target;
        _pathfindingBehaviour = pathfindingBehaviour;
        _leaderBehaviour = leader;
    }

    public override void Enter()
    {
        base.Enter();

        _pathfindingBehaviour.IsActive = true;
        _pathfindingBehaviour.GeneratePathTo(_target);

        _leaderBehaviour.IsActive = false;

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
            return;
        }

        var flockingDir = _steering.GetDir();
        flockingDir.y = 0;

        var finalDir = obs.GetDir(flockingDir);

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

        _leaderBehaviour.IsActive = false;
    }
}