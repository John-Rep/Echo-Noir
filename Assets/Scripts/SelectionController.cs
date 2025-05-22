using UnityEngine;

public class SelectionController : MonoBehaviour
{

    MeshRenderer mr;

    void Start()
    {
        mr = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
    }


    public void Select()
    {
        mr.enabled = true;
    }
    
    public void UnSelect()
    {
        mr.enabled = false;
    }

}
