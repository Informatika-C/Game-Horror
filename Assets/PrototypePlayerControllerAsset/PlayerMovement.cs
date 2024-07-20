using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;

    [SerializeField]
    float moveSpeed = 10f;

    [SerializeField]
    float gravity = 9.81f;

    CharacterController characterController;

    public void OnMove(CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Vector3 processedInput = moveSpeed * Time.fixedDeltaTime * new Vector3(movementInput.x, 0, movementInput.y);
        processedInput.y = -gravity * Time.fixedDeltaTime;
        
        characterController.Move(processedInput);
    }
}
