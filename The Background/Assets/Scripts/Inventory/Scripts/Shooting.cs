using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float Damage = 10f;
    public float Range = 100f;

    //public ParticleSystem muzzleFlash;
    public GameObject ImpactEffect;
    public GameObject barrel;
    public LayerMask Player;

    //private bool inAir = false;
    //private HingeJoint grabHinge;
    public int speed;

    public float Force = 10f;
    public float Radius = 5f;
    public float Upforse = 1f;

    RaycastHit hitRay;

    private void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.isKinematic = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        /*if (Input.GetButtonDown("Fire3"))
        {
            GrapplingShot();
        }*/
        GunAim();
        //GunLETGO();
    }
    
    void Shoot()
    {
        //muzzleFlash.Play();
        
        if (Physics.Raycast(barrel.transform.position, barrel.transform.forward, out hitRay, Range))
        {
            Debug.Log(hitRay.transform.name);

            Target target = hitRay.transform.GetComponent<Target>();
            
            if(target != null)
            {
                target.TakeDamage(Damage);
            }

            Explosion();

            GameObject impactGO = Instantiate(ImpactEffect, hitRay.point, Quaternion.LookRotation(hitRay.normal));
            Destroy(impactGO, 2f);


        }
    }
    void GunAim()
    {
        RaycastHit hit;
        Player = ~LayerMask.GetMask("Player");
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Player))
        {
            Debug.Log(hit.collider.name);
        }
    }
    /*void OnCollisionEnter(Collision col)
    {
        if (inAir == true)
        {
            charcon.Move(Vector3.zero);
            inAir = false;
            grabHinge = gameObject.AddComponent<HingeJoint>();
            grabHinge.connectedBody = col.rigidbody;
            //This stops the hook once it collides with something, and creates a HingeJoint to the object it collided with.
        }
    }
    void GrapplingShot()
    {
        charcon.Move(Vector3.forward * speed);
        inAir = true;
        //This is the direction your hook moves multiplied by speed.
    }*/

    //void GunLETGO()
    //{

    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        transform.parent = null;
    //        rigidbody.isKinematic = false;
    //    }
    //}
    void Explosion()
    {
        Vector3 ExplosionPosision = hitRay.point;
        Collider[] colliders = Physics.OverlapSphere(ExplosionPosision, Radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(Force, ExplosionPosision, Radius, Upforse, ForceMode.Impulse);
            }
            
        }

    }
}
