using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class PlayerModel : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed = 5f;

    [Header("Components")]
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMovementInput(Vector3 input)
    {
        input *= moveSpeed;
       input.y = rb.linearVelocity.y;

        rb.linearVelocity = input;
    }
}
