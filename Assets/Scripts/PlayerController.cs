using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 3f, sprintSpeed = 6f, crouchSpeed = 1.5f, maxLookAngle = 30;
    public float stepSpeed = .7f, sprintStepSpeed = .3f, walkIntensity = 4.5f, sprintIntensity = 10f, throwSpeed = 8f;
    float timeSinceStep = 0f;
    InputAction moveAction, sprintAction, crouchAction, lookAction, throwAction, pickupAction;
    Vector2 move, look;
    public Vector2 rotationSpeed = new Vector2(20f, 5f);
    float sprint, crouch;
    [SerializeField]
    GameObject view, gameManager, selection;
    public GameObject rock;
    public int rockCount = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        view = transform.GetChild(0).GameObject();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        lookAction = InputSystem.actions.FindAction("Look");
        throwAction = InputSystem.actions.FindAction("Interact");
        pickupAction = InputSystem.actions.FindAction("Jump");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (move.magnitude > 0)
        {
            if (finalSpeed == speed && timeSinceStep > stepSpeed)
            {
                gameManager.GetComponent<AudioController>().CreateSound(transform.position + transform.forward, "step", walkIntensity);
                timeSinceStep = 0;
            }
            else if (finalSpeed == sprintSpeed && timeSinceStep > sprintStepSpeed)
            {
                gameManager.GetComponent<AudioController>().CreateSound(transform.position + transform.forward, "run", sprintIntensity);
                timeSinceStep = 0;
            }
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
    }

    void Update()
    {
        if (throwAction.WasPressedThisFrame() && rockCount > 0)
        {
            ThrowRock();
        }
        if (pickupAction.WasPressedThisFrame() && selection != null)
        {
            PickUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && other.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude < 1f)
        {
            if (selection != null)
            {
                selection.GetComponent<SelectionController>().UnSelect();
            }
            selection = other.gameObject;
            selection.GetComponent<SelectionController>().Select();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item") && selection == other.gameObject)
        {
            selection.GetComponent<SelectionController>().UnSelect();
            selection = null;
        }
    }

    void ThrowRock()
    {
        GameObject newRock = Instantiate(rock);
        newRock.transform.position = view.transform.position + view.transform.forward - new Vector3(0, .4f, 0);
        Vector3 direction = transform.forward + new Vector3(0, Mathf.Tan(-view.transform.localEulerAngles.x * Mathf.Deg2Rad), 0);
        newRock.GetComponent<Rigidbody>().linearVelocity = direction * throwSpeed;
        rockCount -= 1;
    }

    void PickUp()
    {
        Destroy(selection);
        rockCount += 1;
    }
}
