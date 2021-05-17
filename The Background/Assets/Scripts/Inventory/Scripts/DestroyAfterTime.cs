using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [Tooltip("Time is in seconds")]
    public float DestroyAfterTminus;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DestroyObject(DestroyAfterTminus);
    }
    void DestroyObject(float time)
    {
        time -= Time.deltaTime;
        DestroyAfterTminus = time;
        if (time <= 0f)
        {
            Destroy(this.gameObject);
        }

    }
}
