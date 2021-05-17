using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUiToScreen : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] public Transform lookAt;
    [SerializeField] public Vector3 offset;

    [Header("Logic")]
    private Camera cam;
    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);
        if(transform.position != pos)
        {
            transform.position = pos;
        }
    }
}
