using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public GameObject soundObject;
    public void CreateSound(Vector3 location, AudioResource audio, float duration)
    {
        GameObject soundSphere = Instantiate(soundObject, location, new Quaternion(0, 0, 0, 0));
        AudioSource sound = soundSphere.GetComponent<AudioSource>();
        sound.resource = audio;
        sound.Play();
        soundSphere.GetComponent<SingleSoundController>().SetDuration(duration);
    }
}
