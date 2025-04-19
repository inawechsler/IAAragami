using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Camera")]
    private float lookSenseH = .05f;
    private float lookSenseV = .05f;
    private float lookLimitV = 90f;
    private Vector2 cameraRot = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f; 
    Quaternion targetRotation;

    [Header("Components")]
    InputController inputController;
    [SerializeField] Camera playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputController = GetComponent<InputController>();
    }
    public void LookDir(Vector3 inputDir)
    {

        targetRotation = Quaternion.LookRotation(inputDir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
            Time.deltaTime * rotationSpeed
        );

    }

    private void LateUpdate()
    {
        ManageCamera();
    }

    private void ManageCamera()
    {
        cameraRot.x += lookSenseH * inputController.lookInput.x;
        cameraRot.y = Mathf.Clamp(cameraRot.y - lookSenseV * inputController.lookInput.y, -lookLimitV, lookLimitV);

        playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * inputController.lookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);
        //playerCamera.transform.rotation = Quaternion.Euler(cameraRot.y, cameraRot.x, 0f);


    }
}
