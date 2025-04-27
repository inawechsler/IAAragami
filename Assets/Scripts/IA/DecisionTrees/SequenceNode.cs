//using System.Collections.Generic;
//using UnityEngine;
//public class SequenceNode : ITreeNode
//{
//    private List<ITreeNode> _nodes;
//    private int _currentNodeIndex;
//    private bool _hasStarted = false;

//    public SequenceNode(List<ITreeNode> nodes)
//    {
//        _nodes = nodes;
//        _currentNodeIndex = 0;
//        _hasStarted = false;
//    }

//    public void Execute()
//    {
//        // Marcar que la secuencia ha comenzado
//        _hasStarted = true;

//        if (_currentNodeIndex < _nodes.Count)
//        {
//            // Ejecuta el nodo actual
//            _nodes[_currentNodeIndex].Execute();

//            // Verifica si el nodo actual ha terminado
//            if (_nodes[_currentNodeIndex].IsFinished())
//            {
//                _currentNodeIndex++;
//                Debug.Log($"SequenceNode: Avanzando al nodo {_currentNodeIndex}/{_nodes.Count}");
//            }
//        }
//        else
//        {
//            // Reinicia la secuencia cuando todos los nodos han terminado
//            Debug.Log("SequenceNode: Secuencia completada, reiniciando");
//            _currentNodeIndex = 0;
//            _hasStarted = false;
//        }
//    }

//    public bool IsFinished()
//    {
//        // La secuencia está terminada cuando ha comenzado y ha ejecutado todos sus nodos
//        return _currentNodeIndex >= _nodes.Count;
//    }
//}