using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billBoard : MonoBehaviour
{
    public Camera cameraToLookFor;
    private void LateUpdate()
    {
        transform.forward = cameraToLookFor.transform.forward;
    }
}
