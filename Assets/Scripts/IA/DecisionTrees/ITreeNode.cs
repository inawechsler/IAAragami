using UnityEngine;

//Interfaz para que se puedan utilizar QuestionNode, ActionNode, etc.
public interface ITreeNode
{
    //Ejecutan algo al llegar aqu�
    void Execute();

    bool IsFinished();

}
