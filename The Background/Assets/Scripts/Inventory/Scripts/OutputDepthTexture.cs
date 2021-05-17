using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDepthTexture : MonoBehaviour
{
    Camera Camera;
    public RenderTexture MiniMapCamTexture;
    
    void Start()
    {
        Camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.targetTexture = MiniMapCamTexture;
    }
}
