using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 2.0f;
    public float maxPitch = 90.0f;
    public float minPitch = -90.0f;
    public float jumpForce = 8.0f;
    public float gravity = 30.0f;

    private CharacterController characterController;
    private Transform pitchController;
    private float pitch = 0.0f;
    private Vector3 velocity;
    private bool isGrounded;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        pitchController = transform.Find("PitchController");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleMouseLook();
    }
    void CheckGrounded()
    {
        isGrounded = characterController.isGrounded;
        Debug.Log(isGrounded);
    }
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        moveDirection = transform.TransformDirection(moveDirection);

        ApplyGravity();
        HandleJump();

        characterController.Move((moveDirection * speed + velocity) * Time.deltaTime);
    }
    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }
    }
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * sensitivity);

        pitch -= mouseY * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        pitchController.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
    }
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            float jumpVelocity = Mathf.Sqrt(2 * jumpForce * gravity);

            // Apply the jump force
            velocity.y = jumpVelocity;
        }
    }




}
