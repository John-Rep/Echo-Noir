using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public GameObject soundObject;
    public AudioResource[] stepAudios, runAudios, rockAudios;
    public void CreateSound(Vector3 location, string audio, float intensity)
    {
        GameObject soundSphere = Instantiate(soundObject, location, new Quaternion(0, 0, 0, 0));
        AudioSource sound = soundSphere.GetComponent<AudioSource>();
        sound.resource = getSound(audio);
        sound.Play();
        soundSphere.GetComponent<SingleSoundController>().SetIntensity(intensity);
    }

    public void CreateMonsterSound(Vector3 location, string audio, float intensity)
    {
        GameObject soundSphere = Instantiate(soundObject, location, new Quaternion(0, 0, 0, 0));
        AudioSource sound = soundSphere.GetComponent<AudioSource>();
        sound.resource = getSound(audio);
        sound.Play();
        soundSphere.GetComponent<SingleSoundController>().SetIntensity(intensity);
        soundSphere.GetComponent<SingleSoundController>().monsterGenerated = true;
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
                index = Random.Range(0, rockAudios.Length);
                return rockAudios[index];
            default:
                return null;
        }
    }
}
