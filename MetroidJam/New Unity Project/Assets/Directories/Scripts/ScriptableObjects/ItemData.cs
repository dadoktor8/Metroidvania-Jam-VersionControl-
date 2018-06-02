using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public Sprite itemSprite;
}