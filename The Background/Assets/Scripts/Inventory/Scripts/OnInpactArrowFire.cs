using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInpactArrowFire : MonoBehaviour
{
    public GameObject Explosion;
    private new Rigidbody rigidbody;
    public float ExplotionForce = 700;
    [Tooltip("can not be less then 0")]
    public float ExplotionRadius;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        ExplosionEffect();
    }
    void ExplosionEffect()
    {
        Instantiate(Explosion, this.transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplotionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(ExplotionForce, transform.position, ExplotionRadius);
            }
        }
        Destroy(this.gameObject);
    }
}
