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
            float multiplier;
            intensity -= fadeSpeed * Time.deltaTime;
            if (intensity > 1)
            {
                multiplier = intensity;
            }
            else
            {
                multiplier = Config.instance.animationCurve.Evaluate(intensity);
            }
            GetComponent<Light>().color = Config.instance.lightColor * multiplier / Config.instance.averageIntensity;
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
