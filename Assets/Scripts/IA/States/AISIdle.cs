using UnityEngine;

public class AISIdle<T> : AISBase<T>
{
    private float _restTime;
    private float timer = 0f;
    public bool isResting { get; private set; }

    public AISIdle(float restTime)
    {
        _restTime = restTime;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        // Detener movimiento
        isResting = true;
        move.Move(Vector3.zero);
    }


    public override void Execute()
    {
      
        base.Execute();
        timer += Time.deltaTime;

        // Cuando termina el tiempo de descanso, vuelve a Idle
        if (timer >= _restTime)
        {
            isResting = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        timer = 0f;

    }
}
