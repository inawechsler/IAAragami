using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Camera")]
    private float lookSenseH = .1f;
    private float lookSenseV = .1f;
    private float lookLimitV = 90f;
    private Vector2 cameraRot = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

    [Header("Components")]
    PlayerController playerController;
    [SerializeField] Camera playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ManageCamera();
    }

    private void ManageCamera()
    {
        cameraRot.x += lookSenseH * playerController.lookInput.x;
        cameraRot.y = Mathf.Clamp(cameraRot.y - lookSenseV * playerController.lookInput.y, -lookLimitV, lookLimitV);

        playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * playerController.lookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);
        playerCamera.transform.rotation = Quaternion.Euler(cameraRot.y, cameraRot.x, 0f);


    }
}
