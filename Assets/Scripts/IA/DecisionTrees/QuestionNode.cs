using System;
using UnityEngine;

public class QuestionNode : MonoBehaviour, ITreeNode
{
    Func<bool> question;

    ITreeNode trueNode;
    ITreeNode falseNode;

    public QuestionNode(Func<bool> question, ITreeNode trueNode, ITreeNode falseNode)
    {
        this.question = question;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public void Execute()
    {
        if (question())
        {
            trueNode.Execute();

        } else
        {
            falseNode.Execute();
        }
    }
}
