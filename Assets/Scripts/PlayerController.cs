using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 3f, sprintSpeed = 6f, crouchSpeed = 1.5f, maxLookAngle = 30;
    public float stepSpeed = .7f, sprintStepSpeed = .3f, walkIntensity = 4.5f, sprintIntensity = 10f, throwSpeed = 8f, throwAngle = 35f, throwModificationMultiplier = .2f;
    float timeSinceStep = 0f;
    InputAction moveAction, sprintAction, crouchAction, lookAction, throwAction, interactAction;
    Vector2 move, look;
    public Vector2 rotationSpeed = new Vector2(20f, 5f);
    float sprint, crouch;
    Vector3 throwStartAdjustment = new Vector3(0, .4f, 0);
    GameObject view, gameManager, selection;
    public GameObject rock;
    public int rockCount = 2;
    bool throwMode = false;
    LineRenderer lr;
    UIManager UI;

    public event Action<int> OnRockInteraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        view = transform.GetChild(0).GameObject();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
        throwAction = InputSystem.actions.FindAction("Throw");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        OnRockInteraction?.Invoke(rockCount);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!UI.paused)
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
                    gameManager.GetComponent<AudioController>().CreateSound(transform.position, "step", walkIntensity);
                    timeSinceStep = 0;
                }
                else if (finalSpeed == sprintSpeed && timeSinceStep > sprintStepSpeed)
                {
                    gameManager.GetComponent<AudioController>().CreateSound(transform.position, "run", sprintIntensity);
                    timeSinceStep = 0;
                }
            }
            timeSinceStep += Time.fixedDeltaTime;

            look = lookAction.ReadValue<Vector2>() * rotationSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, look.x, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);

            if (!throwMode)
            {
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
            else
            {
                throwSpeed += look.y * throwModificationMultiplier;
            }
        }
        

    }

    void Update()
    {
        if (!UI.paused)
        {
            if (throwAction.WasPressedThisFrame() && throwMode)
            {
                throwMode = false;
                lr.enabled = false;
            }
            else if (throwAction.WasPressedThisFrame() && rockCount > 0)
            {
                lr.enabled = true;
                throwMode = true;
            }
            if (throwMode && interactAction.WasPressedThisFrame())
            {
                ThrowRock();
                lr.enabled = false;
                throwMode = false;
            }
            else if (interactAction.WasPressedThisFrame() && selection != null)
            {
                PickUp();
            }
            if (throwMode)
            {
                DrawTrajectory();
            }
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
        newRock.transform.position = view.transform.position + view.transform.forward - throwStartAdjustment;
        Vector3 direction = transform.forward + new Vector3(0, Mathf.Tan(throwAngle * Mathf.Deg2Rad), 0);
        newRock.GetComponent<Rigidbody>().linearVelocity = direction.normalized * throwSpeed;
        rockCount -= 1;
        OnRockInteraction?.Invoke(rockCount);
    }

    void PickUp()
    {
        Destroy(selection);
        rockCount += 1;
        OnRockInteraction?.Invoke(rockCount);
    }

    void DrawTrajectory()
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 newPosition = view.transform.position + view.transform.forward - throwStartAdjustment;
        float x = 0, y = 0;
        int index = -1;
        float step = .02f, time = 0;
        do
        {
            time += step;
            positions.Add(newPosition);
            x = throwSpeed * Mathf.Cos(throwAngle * Mathf.Deg2Rad) * time;
            y = -4.9f * time * time + throwSpeed * Mathf.Sin(throwAngle * Mathf.Deg2Rad) * time;

            newPosition = positions[0] + transform.forward * x + transform.up * y;
            index++;
        } while (!Physics.Raycast(positions[index], newPosition - positions[index], Vector3.Distance(positions[index], newPosition)) && index < 200);
        
        lr.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i] += transform.right * (positions.Count - i) / positions.Count;
        }
        lr.SetPositions(positions.ToArray());
    }
}
