using UnityEngine;

public class SelectionController : MonoBehaviour
{

    [SerializeField]
    Material selectedMaterial;
    Material defaultMaterial;

    MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        defaultMaterial = mr.material;
    }


    public void Select()
    {
        mr.material = selectedMaterial;
    }
    
    public void UnSelect()
    {
        mr.material = defaultMaterial;
    }

}
