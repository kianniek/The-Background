using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ArrowScript : MonoBehaviour
{
    public int damage;
    public LayerMask StickLayer;
    private GameObject Arrow;
    private Rigidbody ArrowRB;
    private BoxCollider[] boxColliders;
    private bool isStuck = false;
    public bool arrowRotation = true;
    private void Start()
    {
        Arrow = this.gameObject;
        ArrowRB = Arrow.GetComponent<Rigidbody>();
        boxColliders = this.gameObject.GetComponents<BoxCollider>();
        Physics.IgnoreCollision(ArrowRB.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }
    private void OnTriggerEnter(Collider other)
    {
        isStuck = true;
        if (ArrowRB)
        {
            if (other.GetComponent<EnemyStats>())
            {
                EnemyStats stats = other.GetComponent<EnemyStats>();
                stats.Hit(damage);
            }
            if (other.gameObject.layer == StickLayer)
            {
                
                this.gameObject.transform.SetParent(other.transform);
                ArrowRB.constraints = RigidbodyConstraints.FreezeAll;
                for (int i = 0; i < boxColliders.Length; i++)
                {
                    boxColliders[i].enabled = false;
                }
            }
        }
    }
    private void Update()
    {
        Object.Destroy(this.gameObject, 10);
    }
    void FixedUpdate()
    {
        if (arrowRotation)
        {
            if (!isStuck)
            {
                if (ArrowRB.velocity != Vector3.zero)
                {
                    ArrowRB.rotation = Quaternion.LookRotation(ArrowRB.velocity);
                }
            }
        }
       
        
    }
}
