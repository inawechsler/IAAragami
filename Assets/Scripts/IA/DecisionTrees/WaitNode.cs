using UnityEngine;

public class WaitNode : ITreeNode
{
    private float _waitTime;      
    private float _elapsedTime;   
    private bool _hasStarted;      // Indica si la espera ha comenzado

    public WaitNode(float waitTime)
    {
        _waitTime = waitTime;
        _elapsedTime = 0f;
        _hasStarted = false;
    }

    public void Execute()
    {
        if (!_hasStarted)
        {
            _hasStarted = true;
            _elapsedTime = 0f;
        }

        _elapsedTime += Time.deltaTime;
        Debug.Log($"WaitNode: Esperando {_elapsedTime}/{_waitTime}");
    }

    public bool IsFinished()
    {
        bool finished = _hasStarted && _elapsedTime >= _waitTime;

        if (finished)
        {
            _hasStarted = false;
            _elapsedTime = 0f;
        }

        return finished;
    }
}