using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMesh : MonoBehaviour
{
    public Transform Mesh;
    public Vector3 offsetLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Mesh.transform.position + offsetLoc;
        gameObject.transform.rotation = Mesh.transform.rotation;
    }
}
