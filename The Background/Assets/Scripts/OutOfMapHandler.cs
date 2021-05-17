using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class OutOfMapHandler : MonoBehaviour
{
    public Vector3 BoxSize = new Vector3(500,3,500);
    public GameObject SpawnPlace;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        gameObject.GetComponent<BoxCollider>().size = BoxSize;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SpawnPlace.GetComponent<SpawnPlayer>().GoBackUFaggot();
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, BoxSize);
    }
}
