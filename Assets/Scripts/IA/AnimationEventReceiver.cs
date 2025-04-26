using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private MeleeModel aiModel;

    private void Awake()
    {

        aiModel = GetComponentInParent<MeleeModel>();

        if (aiModel == null)
        {
            Debug.LogError("No se encontr� el componente AIModel. Los eventos de animaci�n no funcionar�n.");
        }

    }

    // Estos m�todos ser�n llamados por los eventos de animaci�n
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