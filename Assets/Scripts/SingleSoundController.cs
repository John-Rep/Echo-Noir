using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SingleSoundController : MonoBehaviour
{

    float intensity;
    float fadeSpeed = 5f;
    float waitTime = 1f;
    public bool monsterGenerated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider[] hits;
        hits = Physics.OverlapSphere(transform.position, intensity);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Monster") && !monsterGenerated)
            {
                Debug.Log("New Sound Detected by Monster");
                hit.GetComponent<MonsterFollow>().NewChase(transform.position);
            }
        }
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
