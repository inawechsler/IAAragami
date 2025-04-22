using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AIModel : MonoBehaviour, IMove
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Components")]
    Rigidbody rb;
    public Transform Position { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        //    // crear el vector de movimiento usando el input
        //    // moveInput.y positivo (W) = adelante
        //    // moveInput.x positivo (D) = derecha
        //    Vector3 movement = Position.forward * -inputController.moveInput.y +
        //        Position.right * -inputController.moveInput.x;

          return Vector3.zero;
    }
}
