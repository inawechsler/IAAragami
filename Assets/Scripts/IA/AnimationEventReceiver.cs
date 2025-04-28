using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private MeleeModel aiModel;

    private void Awake()
    {

        aiModel = GetComponentInParent<MeleeModel>();

    }

    // Estos m�todos se llaman por los eventos en la animaci�n
    public void EnableAttackCollider()
    {
        if (aiModel != null)
            aiModel.EnableAttackCollider();
    }

    public void DisableAttackCollider()
    {
        if (aiModel != null)
            aiModel.DisableAttackCollider();
    }
}