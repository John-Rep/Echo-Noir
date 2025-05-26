using Unity.VisualScripting;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public bool active = false;
    GoalController goal;

    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<GoalController>();
    }

    public void Activate()
    {
        active = true;
        transform.GetChild(0).localEulerAngles = new Vector3(50, 0, 0);
        goal.Open();
    }
}
