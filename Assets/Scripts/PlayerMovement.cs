using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float noiseLevel = 0f;
    public float maxNoise = 10f;
    public Transform cam;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Si aucune caméra n’est assignée dans l’inspector, on utilise celle marquée "Main Camera"
        if (cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * v + right * h;
        controller.SimpleMove(move * speed);

        noiseLevel = move.magnitude > 0.1f ? maxNoise : 0f;
    }
}
