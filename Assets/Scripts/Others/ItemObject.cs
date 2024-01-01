using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItem item;

    public InventoryItem GetItem()
    {
        return this.item;
    }
}
