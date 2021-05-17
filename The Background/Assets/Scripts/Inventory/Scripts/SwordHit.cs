using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public KeyCode attackButton;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(attackButton))
        {
            if (other.GetComponent<EnemyStats>())
            {
                EnemyStats stats = other.GetComponent<EnemyStats>();
                stats.Hit(damage);
            }
        }
    }
}
