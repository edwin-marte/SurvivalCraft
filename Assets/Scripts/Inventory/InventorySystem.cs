using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int capacity;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Start()
    {
        this.slots = GenerateSlots(capacity);
    }

    public List<InventorySlot> GenerateSlots(int capacity)
    {
        List<InventorySlot> currentSlots = new List<InventorySlot>();

        for (int i = 0; i < capacity; i++)
        {
            currentSlots.Add(new InventorySlot());
        }

        return currentSlots;
    }

    public InventorySlot AddItem(InventoryItem item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.SetItem(item);
                return slot;
            }
        }
        return null;
    }

    public void ClearSlot(InventorySlot targetSlot)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot == targetSlot)
            {
                slot.item = null;
                break;
            }
        }
    }

    public bool InventoryFull()
    {
        bool isFull = true;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                isFull = false;
            }
        }
        return isFull;
    }
}
