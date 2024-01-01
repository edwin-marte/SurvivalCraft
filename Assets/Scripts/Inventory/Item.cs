using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item")]
public class Item : InventoryItem
{
    public bool isConsumable;
    public List<Pair<InventoryItem, float>> materials;
}
