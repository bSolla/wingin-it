﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    private RectTransform inventoryRect;
    private float inventoryWidth, inventoryHeight;
    public int slots;
    public int rows;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    public GameObject slotPrefab;
    private List<GameObject> allSlots;
    private int emptySlot;

    /* public static int EmptySlot
    {
        get { return emptySlot; }
        set { emptySlot = value; }
    } */


    void Start()
    {
        CreateLayout();
    }

    void Update()
    {
        
    }

    private void CreateLayout ()
    {
        allSlots = new List<GameObject>();
        emptySlot = slots;
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
        
        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject) Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                slotRect.localPosition = inventoryRect.localPosition + new Vector3 (slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                allSlots.Add(newSlot);
            }
        }

    }

    public bool AddItem(Item item)
    {
        Debug.Log ("running the additem function");
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else 
        {
            foreach (GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if (!temp.isEmpty)
                {
                    if (temp.CurrentItem.type == item.type && temp.isAvailable)
                    {
                        temp.AddItem(item);
                        return true;
                    }
                }
            }
            if (emptySlot > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        Debug.Log ("placing into empty slot");
        if (emptySlot > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if (temp.isEmpty)
                {
                    temp.AddItem(item);
                    emptySlot--;
                    return true;
                }
            }
        }

        return false;
    }
    
}