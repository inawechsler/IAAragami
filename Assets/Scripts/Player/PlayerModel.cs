using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class PlayerModel : MonoBehaviour, IMove
{
    [Header("Movement")]
    private float moveSpeed = 5f;

    [Header("Components")]
    Rigidbody rb;
    InputController inputController;    
    public Transform Position { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController>();
        Position = transform;
    }
    public void Move(Vector3 input)
    {
        input *= moveSpeed;
       input.y = rb.linearVelocity.y;

        rb.linearVelocity = input;
    }

    public Vector3 CalculateMovementDirection()
    {
        // crear el vector de movimiento usando el input
        // moveInput.y positivo (W) = adelante
        // moveInput.x positivo (D) = derecha
        Vector3 movement = Position.forward * -inputController.moveInput.y +
            Position.right * -inputController.moveInput.x;

        return movement.normalized;
    }
}
