using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventoryToggel : MonoBehaviour
{
    public GameObject[] InventoryUI;
    public bool IsActive;
    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Tab))
        {
            IsActive = !IsActive;
        }
        if (IsActive)
        {
            for (int i = 0; i < InventoryUI.Length; i++)
            {
                InventoryUI[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < InventoryUI.Length; i++)
            {
                InventoryUI[i].SetActive(false);
            }
        }
    }
}
