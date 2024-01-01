using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftSystem : MonoBehaviour
{
    public CraftStation craftStation; // Station which the player is interacting with

    private InventorySystem backapackInventory;

    private void Start()
    {
        backapackInventory = Current.Instance.backpack.GetComponent<InventorySystem>();
    }

    public (bool, string) Craft(Item item)
    {
        var canPerformCraft = canCraft(item);
        if (canPerformCraft.Item1)
        {
            backapackInventory.AddItem(item);
        }
        return canPerformCraft;
    }

    public (bool, string) canCraft(Item item)
    {
        bool canCraft = true;
        string errorMsj = "";

        if (backapackInventory.InventoryFull())
        {
            errorMsj = "The backack is full";
            canCraft = false;
        }

        foreach (var material in item.materials)
        {
            if (!EnoughMaterial(material))
            {
                if (!canCraft)
                {
                    errorMsj += " and you don't have enough materials in the backpack";
                }
                else
                {
                    errorMsj = "You don't have enough materials in the backpack";
                    canCraft = false;
                }
            }
        }

        if (canCraft)
        {
            foreach (var material in item.materials)
            {
                float materialsToRemove = material.Item2();
                float materialsRemoved = 0f;
                foreach (var slot in backapackInventory.slots)
                {
                    if (slot.item != null && slot.item == material.Item1())
                    {
                        if (materialsRemoved == materialsToRemove)
                        {
                            break;
                        }
                        else
                        {
                            materialsRemoved++;
                            backapackInventory.ClearSlot(slot);
                        }
                    }
                }
            }
        }

        return (canCraft, errorMsj);
    }

    private bool EnoughMaterial(Pair<InventoryItem, float> material)
    {
        var matsAvailable = MaterialsAvailable(material);
        return matsAvailable >= material.Item2();
    }

    public float MaterialsAvailable(Pair<InventoryItem, float> material)
    {
        float amount = 0f;
        List<InventorySlot> slots = new List<InventorySlot>();
        foreach (var slot in backapackInventory.slots)
        {
            if (slot.item != null && slot.item == material.Item1())
            {
                amount += 1f;
            }
        }
        return amount;
    }
}
