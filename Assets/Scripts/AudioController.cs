using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public GameObject soundObject;
    public AudioResource[] stepAudios, runAudios;
    public void CreateSound(Vector3 location, string audio, float intensity)
    {
        GameObject soundSphere = Instantiate(soundObject, location, new Quaternion(0, 0, 0, 0));
        AudioSource sound = soundSphere.GetComponent<AudioSource>();
        sound.resource = getSound(audio);
        sound.Play();
        soundSphere.GetComponent<SingleSoundController>().SetIntensity(intensity);
    }

    private AudioResource getSound(string sound)
    {
        switch (sound)
        {
            case "step":
                int index = Random.Range(0, stepAudios.Length);
                return stepAudios[index];
            case "run":
                index = Random.Range(0, runAudios.Length);
                return runAudios[index];
            case "rock":
                return stepAudios[2];
            default:
                return null;
        }
    }
}
