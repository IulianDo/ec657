using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerControls;
    private PlayerInput.PlayerActions actions;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeigth = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDimension = 2f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private bool debug;
    [SerializeField] float sprintSpeed;

    //debugging for what direction the user is facing
    void OnDrawGizmos()
	{
        if(!debug)
		{
            return;
		}
        Debug.DrawLine(transform.position, transform.position + transform.forward * 5, Color.red);
	}

    //initialise variables
    void Awake()
    {
        playerControls = new PlayerInput();
        actions = playerControls.Player;
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
    }

    // Allows for movement set up with player input
    void Move()
        {
        Vector2 direction = actions.Movement.ReadValue<Vector2>();
        Vector3 move = transform.right * direction.x + transform.forward * direction.y;
        // Check if Shift key is held down for sprinting
        float currentSpeed = actions.Sprint.IsPressed() ? sprintSpeed : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);


    }

    // Allows you to jump when on the ground
    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDimension, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (actions.Jump.IsPressed() && isGrounded)
        {
            velocity.y = Mathf.Sqrt( jumpHeigth * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    //for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDimension);
    }

    //--------------------------------------------------------//

}
