using UnityEngine;

public class FAISChase<T> : AISBase<T>
{
    Rigidbody target;
    ISteering steering;
    LeaderBehaviour leaderBehaviour;
    public FAISChase(ISteering steering, Rigidbody target, LeaderBehaviour leaderBehaviour)
    {
        this.leaderBehaviour = leaderBehaviour;
        this.steering = steering;
        this.target = target;
    }
    public override void Enter()
    {
        base.Enter();
        leaderBehaviour.IsActive = true;
        leaderBehaviour.LeaderRb = target;
        
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
    }
}
