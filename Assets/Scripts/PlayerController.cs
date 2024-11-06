using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintMultiplier = 2f;

    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float upDownRange = 80f;
    [SerializeField] int health = 100;

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravityForce = 20f;
    [SerializeField] float moveDamping = 0.5f;

    private Vector3 currentMovement = Vector3.zero;
    private CharacterController characterController;

    [SerializeField] private PlayerControlInput playerControlInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction shootAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    //private string verticalInput = "Vertical";
    //private string horizontalInput = "Horizontal";
    //private string mouseXInput = "Mouse X";
    //private string mouseYInput = "Mouse Y";
    //[SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    //[SerializeField] private KeyCode jumpKey = KeyCode.Space;


    private float verticalRotation;

    private Camera playerCamera;

    private void Awake()
    {
        playerControlInput = new PlayerControlInput();
        playerControlInput.Player.Enable();

        moveAction = playerControlInput.Player.Move;
        lookAction = playerControlInput.Player.Look;
        jumpAction = playerControlInput.Player.Jump;
        sprintAction = playerControlInput.Player.Sprint;
        interactAction = playerControlInput.Player.Interact;
        shootAction = playerControlInput.Player.Shoot;

        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;
        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float speedMultiplier = sprintAction.ReadValue<float>() > 0 ? sprintMultiplier : 1f;

        if (characterController.isGrounded)
        {
            float verticalInput = moveInput.y;
            float horizontalInput = moveInput.x;
        
            Vector3 horizontalMovement = new Vector3(horizontalInput, 0, verticalInput).normalized;
            horizontalMovement = transform.rotation * horizontalMovement * walkSpeed * speedMultiplier;

            currentMovement.x = horizontalMovement.x;
            currentMovement.z = horizontalMovement.z;
        }
        
        HandleJumpingAndGravity();

        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleRotation()
    {
        float mouseXrotation = lookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXrotation, 0);

        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleJumpingAndGravity()
    {

        if (characterController.isGrounded)
        {
            currentMovement.y = -1f;

            if (jumpAction.ReadValue<float>() > 0)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            // damping in horizontal movement
            currentMovement.x = Mathf.Lerp(currentMovement.x, 0, moveDamping * Time.deltaTime);
            currentMovement.z = Mathf.Lerp(currentMovement.z, 0, moveDamping * Time.deltaTime);

            // gravity
            currentMovement.y -= gravityForce * Time.deltaTime;
        }
    }

    public void HandleDamageAndDeath(int damage)
    {
        health -= damage;
        Debug.Log("Player health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

}
