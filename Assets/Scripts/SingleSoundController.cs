using UnityEngine;
using UnityEngine.Rendering;

public class SingleSoundController : MonoBehaviour
{

    float intensity;
    float fadeSpeed = 6f;
    float waitTime = .5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            intensity -= fadeSpeed * Time.deltaTime;
            GetComponent<Light>().intensity = intensity;
            if (intensity <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
    }
}
