using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator charactorAnimator;
    Rigidbody characterRB;
    //Transform characterTransform;
    Vector3 movementInput;
    Vector3 movementVector;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    private float sprint = 1;
    private bool grounded = true;


    void Start()
    {
        charactorAnimator = transform.Find("Kuratchi_l_rigged_ver.1.0").GetComponent<Animator>();
        characterRB = GetComponent<Rigidbody>();    
        //characterTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        movementVector = transform.forward * movementInput.y + transform.right * movementInput.x;
        movementVector.y = 0;
        transform.position += movementVector * sprint * movementSpeed * Time.deltaTime;
    }

    private void OnJump(InputValue input)
    {
        if (!grounded) return;

        charactorAnimator.SetBool("isRunning", false);
        charactorAnimator.SetTrigger("isJumping");
        characterRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnSprint(InputValue input)
    {
        charactorAnimator.SetBool("isRunning", true);
        sprint = 2;
    }

    private void OnSprintStop(InputValue input)
    {
        charactorAnimator.SetBool("isRunning", false);
        sprint = 1;
    }

    private void OnMovement(InputValue input)
    {
        Debug.Log("Pressed WASD");

        charactorAnimator.SetBool("isWalking", true);
        movementInput = input.Get<Vector2>();
    }

    private void OnMovementStop(InputValue input)
    {
        charactorAnimator.SetBool("isWalking", false);
        charactorAnimator.SetBool("isRunning", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
