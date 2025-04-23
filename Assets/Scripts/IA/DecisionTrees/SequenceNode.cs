using System.Collections.Generic;
using UnityEngine;

//Creo SequenceNode porq necesito una clase que los ejecute en secuencia
public class SequenceNode : ITreeNode
{
    private List<ITreeNode> _nodes;
    private int _currentNodeIndex;

    public SequenceNode(List<ITreeNode> nodes)
    {
        _nodes = nodes;
        _currentNodeIndex = 0;
    }

    public void Execute()
    {
        if (_currentNodeIndex < _nodes.Count)
        {
            _nodes[_currentNodeIndex].Execute();

            Debug.Log("bool: " + (_nodes[_currentNodeIndex] is ActionNode ||
                 (_nodes[_currentNodeIndex].IsFinished())));

            if (_nodes[_currentNodeIndex] is ActionNode ||
                (_nodes[_currentNodeIndex].IsFinished()))
            {
                _currentNodeIndex++;
            }
        }
        else
        {
            _currentNodeIndex = 0;
        }
    }

    public bool IsFinished()
    {
        return _currentNodeIndex >= _nodes.Count;
    }
}