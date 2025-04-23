using UnityEngine;
using System;

public class ActionNode : ITreeNode
{
    Action action;

    //No se necesita ref al siguiente nodo o acción ya que al ser una acción, solo se ejecuta
    public ActionNode(Action action)
    {
        this.action = action;
    }

    public void Execute()
    {
        action();
    }

    public bool IsFinished()
    {
        return true;
    }
}
