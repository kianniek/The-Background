using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.UI;
[ExecuteInEditMode]
public class PickupText : MonoBehaviour
{
    public GameObject ItemPickedUp;
    public string currentText = "";
    public InventoryPlayer InvPlayer;
    public TextMeshPro DisplayText;

    // Start is called before the first frame update
    void Start()
    {
        DisplayText = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        //DisplayText.text.Replace()
        currentText = DisplayText.text;
        DisplayText.text = InvPlayer.GroundColliderName;
        if(ItemPickedUp == null) { return; }
    }
}
