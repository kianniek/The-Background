using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    private void Start()
    {
        GoBackUFaggot();
    }
    public void GoBackUFaggot()
    {
        if(Player == null)
        {
            Debug.Log("There is no Player GameObject to refer to!");
        }
        else
        {
            Player.transform.position = gameObject.transform.position;
            Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
