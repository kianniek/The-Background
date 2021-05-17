using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IgnoreParentRotation : MonoBehaviour
{
    private float _rotation;
    private float _counterRotation;
    [Range(-90f, 90f)]
    public float StartAngleX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _counterRotation = gameObject.GetComponentInParent<Transform>().rotation.x;
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            // This is a UI element since it has a RectTransform component on it
            _rotation = gameObject.GetComponent<RectTransform>().rotation.x;
        }
        else if(gameObject.GetComponent<Transform>() != null)
        {
            _rotation = gameObject.GetComponent<Transform>().rotation.x;
        }
    }

    private void LateUpdate()
    {
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            
            var rotationVector = gameObject.GetComponent<RectTransform>().rotation.eulerAngles;
            rotationVector.x = StartAngleX;
            gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationVector);
        }
        else if (gameObject.GetComponent<Transform>() != null)
        {
            var rotationVector = gameObject.GetComponent<Transform>().rotation.eulerAngles;
            rotationVector.x = StartAngleX;
            gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(rotationVector);
        }
    }
}
