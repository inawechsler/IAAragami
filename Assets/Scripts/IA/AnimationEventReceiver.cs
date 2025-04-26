using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private MeleeModel aiModel;

    private void Awake()
    {

        aiModel = GetComponentInParent<MeleeModel>();

        if (aiModel == null)
        {
            Debug.LogError("No se encontró el componente AIModel. Los eventos de animación no funcionarán.");
        }

    }

    // Estos métodos serán llamados por los eventos de animación
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