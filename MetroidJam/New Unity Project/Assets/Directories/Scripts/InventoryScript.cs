using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<ItemData> inventory = new List<ItemData>();

    public void AddToInventory(ItemData item)
    {
        inventory.Add(item);
    }

    public void RemoveFromInventory(ItemData item)
    {
        inventory.Remove(item);
    }

    public bool CheckInventory(ItemData item)
    {
        return inventory.Contains(item);
    }

    public bool CheckInventoryByName(string itemName)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == itemName)
                return true;
        }
        return false;
    }

    public bool ConsumeItemByName(string itemName)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemName == itemName)
            {
                inventory.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public List<ItemData> GetAllItems()
    {
        return inventory;
    }
}