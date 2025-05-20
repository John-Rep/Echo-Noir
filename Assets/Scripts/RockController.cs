using Unity.VisualScripting;
using UnityEngine;

public class RockController : MonoBehaviour
{
    GameObject gameManager;
    float intensityMultiplier = 5f;
    Rigidbody rb;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude < .5f)
        {
            Destroy(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            float intensity = rb.linearVelocity.magnitude * intensityMultiplier;
            gameManager.GetComponent<AudioController>().CreateSound(transform.position, "rock", intensity);
        }
    }

}
