using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class PlayerModel : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 movementSpeed = Vector2.zero;
    private float moveSpeed = 5f;

    [Header("Components")]
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector3(
            movementSpeed.x * moveSpeed,
            rb.linearVelocity.y,
            movementSpeed.y * moveSpeed
        );
    }
    public void SetMovementInput(Vector2 input)
    {
        movementSpeed = input;
    }
}
