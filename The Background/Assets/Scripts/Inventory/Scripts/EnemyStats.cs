using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int Health;
    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Die();
        }
    }

    public void Hit(int damage)
    {
        Health -=  damage;
    }
    public void Die()
    {
        //Die animation

        //LootDrop

        //when animation done destroy gameobject
        Destroy(this);
    }
}
