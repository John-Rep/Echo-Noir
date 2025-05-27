using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config instance;

    public Color lightColor;
    public float averageIntensity;
    public AnimationCurve animationCurve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

}
