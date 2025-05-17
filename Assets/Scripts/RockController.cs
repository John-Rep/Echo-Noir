using Unity.VisualScripting;
using UnityEngine;

public class RockController : MonoBehaviour
{
    GameObject gameManager;
    float intensityMultiplier = 3f;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void OnTriggerEnter()
    {
        float intensity = GetComponentInParent<Rigidbody>().linearVelocity.magnitude * intensityMultiplier;
        gameManager.GetComponent<AudioController>().CreateSound(transform.position, "rock", intensity);
    }
}
