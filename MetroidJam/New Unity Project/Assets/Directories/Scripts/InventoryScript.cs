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

    public List<ItemData> GetAllItems()
    {
        return inventory;
    }
}