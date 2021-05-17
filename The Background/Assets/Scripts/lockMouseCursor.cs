using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockMouseCursor : MonoBehaviour
{
    public bool LockMouseCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        if (LockMouseCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!LockMouseCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
