using UnityEngine;

public class WaitNode : ITreeNode
{
    private float _waitTime;
    private float _elapsedTime;
    private bool _isWaiting;
    

    public WaitNode(float waitTime)
    {
        _waitTime = waitTime;
        _elapsedTime = 0f;
        _isWaiting = false;
    }

    public void Execute()
    {
        if (!_isWaiting)
        {
            _isWaiting = true;
            _elapsedTime = 0f;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _waitTime)
        {

            _isWaiting = false;
        }
    }

    public bool IsFinished()
    {
        return !_isWaiting;
    }


}
