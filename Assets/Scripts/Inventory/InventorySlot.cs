using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;

    public InventorySlot()
    {
        this.item = null;
    }

    public InventorySlot(InventoryItem item)
    {
        this.item = item;
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
    }
}
