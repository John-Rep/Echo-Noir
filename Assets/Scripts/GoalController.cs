using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    Transform[] doors = new Transform[2];
    void Start()
    {
        doors[0] = transform.GetChild(0).transform;
        doors[1] = transform.GetChild(1).transform;
    }

    public void Open()
    {
        foreach (Transform door in doors)
        {
            door.localEulerAngles = new Vector3(0, 90, 0);
        }
    }
}
