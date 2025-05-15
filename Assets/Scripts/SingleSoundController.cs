using UnityEngine;
using UnityEngine.Rendering;

public class SingleSoundController : MonoBehaviour
{

    float duration;
    float currentTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > duration)
        {
            Destroy(gameObject);
        }
    }

    public void SetDuration(float Duration)
    {
        duration = Duration;
    }
}
