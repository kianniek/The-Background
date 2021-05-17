using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class InventoryPlayer : MonoBehaviour
{

    //public MouseItem mouseItem = new MouseItem();

    public InventoryObject inventory;
    public InventoryObject equipment;
    public string GroundColliderName;
    public Canvas[] CanvasUIWorldSpace;
    private bool PressedPickup;
    
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < CanvasUIWorldSpace.Length; i++)
        {
            CanvasUIWorldSpace[i].gameObject.SetActive(false);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        //var CanvasUI = other.GetComponentInChildren<Canvas>();
        var groundItem = other.GetComponent<GroundItem>();
        
        if (groundItem)
        {
            groundItem.gameObject.GetComponentInChildren(typeof(Canvas), true).gameObject.SetActive(true);
            if (PressedPickup)
            {
                Item _item = new Item(groundItem.item);
                if (inventory.AddItem(_item, 1))
                {
                    GroundColliderName = groundItem.item.name;
                    Destroy(other.gameObject);
                    PressedPickup = false;
                }
            }
            
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        //var CanvasUI = other.GetComponentInChildren<Canvas>();
        var groundItem = other.GetComponent<GroundItem>();
        if(groundItem != null)
        {
            groundItem.gameObject.GetComponentInChildren(typeof(Canvas), true).gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ExampleCoroutine());
        }
        IEnumerator ExampleCoroutine()
        {
            PressedPickup = true;
            yield return new WaitForSeconds(0.3f);
            PressedPickup = false;
        }
        
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }
}
