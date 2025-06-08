using UnityEngine;

public class FAISEvade<T> : AISBase<T>
{
    Rigidbody target;
    ISteering steering;
    LeaderBehaviour leaderBehaviour;
    PredatorBehaviour predatorBehaviour;
    public FAISEvade(ISteering steering, Rigidbody target, LeaderBehaviour leaderBehaviour, PredatorBehaviour predatorBehaviour)
    {
        this.predatorBehaviour = predatorBehaviour;
        this.leaderBehaviour = leaderBehaviour;
        this.steering = steering;
        this.target = target;
    }
    public override void Enter()
    {
        base.Enter();
        leaderBehaviour.IsActive = true;
        leaderBehaviour.LeaderRb = target;
        predatorBehaviour.multiplier = 10;
    }
    public override void Execute()
    {
        var steeringDir = steering.GetDir();   
        steeringDir.y = 0; 
        var dir = obs.GetDir(steeringDir);
        
        move.Move(dir.normalized);
        look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
        leaderBehaviour.IsActive = false;
        predatorBehaviour.multiplier = 0; // Reset multiplier to default value
    }
}
