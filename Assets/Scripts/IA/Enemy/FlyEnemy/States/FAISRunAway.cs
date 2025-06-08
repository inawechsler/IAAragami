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

        // Resetear el estado del path
        if (path != null)
        {
            path.isFinishPath = false;
            path.isOnPathfinding = true;
        }

        Debug.Log("Iniciando pathfinding con flocking hacia: " + _target);
    }

    public override void Execute()
    {
        base.Execute();

        if (path.isFinishPath) return;
        // Verificar si el path está completo
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
        steeringDir.y = 0; // Mantener en el plano horizontal

        // Aplicar obstacle avoidance si está disponible
        var finalDir = obs.GetDir(steeringDir);

        // Mover y mirar en la dirección calculada
        if (finalDir.magnitude > 0.1f) // Solo mover si hay dirección válida
        {
            move.Move(finalDir.normalized);
            look.LookDir(finalDir.normalized);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Desactivar el comportamiento de pathfinding
        _pathfindingBehaviour.IsActive = false;

        Debug.Log("Saliendo del pathfinding con flocking");
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        _target = newTarget;
        if (_pathfindingBehaviour.IsActive)
        {
            _pathfindingBehaviour.GeneratePathTo(_target);
        }
    }

    public Vector3 CurrentTarget => _target;
    public bool IsPathCompleted => _pathfindingBehaviour.IsPathCompleted;
}

