using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 3f, sprintSpeed = 6f, crouchSpeed = 1f, maxLookAngle = 30, speedSoundThreshold = .04f;
    public float stepSpeed = .3f;
    [SerializeField] float timeSinceStep = 0f;
    InputAction moveAction, sprintAction, crouchAction, lookAction;
    Vector2 move, look;
    public AudioResource moveSound;
    public Vector2 rotationSpeed = new Vector2(20f, 5f);
    float sprint, crouch;
    public GameObject view, gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crouch = crouchAction.ReadValue<float>();
        sprint = sprintAction.ReadValue<float>();
        float finalSpeed = speed;
        if (crouch > 0)
        {
            finalSpeed = crouchSpeed;
        }
        else if (sprint > 0)
        {
            finalSpeed = sprintSpeed;
        }
        move = moveAction.ReadValue<Vector2>() * finalSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.transform.position + transform.forward * move.y + transform.right * move.x);

        float currentSpeed = move.magnitude;
        if (currentSpeed > speedSoundThreshold && timeSinceStep > stepSpeed)
        {
            gameManager.GetComponent<AudioController>().CreateSound(transform.position + transform.forward, moveSound, stepSpeed + .2f);
            timeSinceStep = 0;
        }
        timeSinceStep += Time.fixedDeltaTime;

        look = lookAction.ReadValue<Vector2>() * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, look.x, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        float newLook = -look.y + view.transform.localEulerAngles.x;
        if (newLook > 180)
        {
            newLook = Math.Clamp(newLook, 360 - maxLookAngle, 360);
        }
        else
        {
            newLook = Math.Clamp(newLook, -maxLookAngle, maxLookAngle);
        }
        
        view.transform.localEulerAngles = new Vector3(newLook, 0f, 0f);

        Debug.DrawRay(view.transform.position, view.transform.forward * 5f, Color.red);
    }
}
