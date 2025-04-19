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
        var shoot = new ActionNode(()=> Debug.Log("Shoot"));
        var patrol = new ActionNode(()=> Debug.Log("Patrol"));
        var reload = new ActionNode(()=> Debug.Log("Reload"));
        var died = new ActionNode(()=> Debug.Log("Died"));

        var qHasAmmo = new QuestionNode(QHasBullets, shoot, reload);
        var qHasLife = new QuestionNode(() => 100 < 9, died, qHasAmmo);

        rootNode = qHasLife;
        //Desde el root llegamos a los demas nodos 
    }

    bool QHasBullets()
    {
        return 1 > 9;
    }
}
