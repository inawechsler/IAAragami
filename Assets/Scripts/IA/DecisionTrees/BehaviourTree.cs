using NUnit.Framework.Constraints;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    ITreeNode rootNode;
    private void Start()
    {
        InitTree();
    }

    void InitTree()
    {
        var patrol = new ActionNode(()=> Debug.Log("Patrol"));
        var reload = new ActionNode(()=> Debug.Log("Reload"));
        var died = new ActionNode(()=> Debug.Log("Died"));

        var qHasLife = new QuestionNode(QHasLife, died, patrol);

        rootNode = qHasLife;
        //Desde el root llegamos a los demas nodos 
    }

    bool QHasLife()
    {
        return 1 > 9;
    }
}
