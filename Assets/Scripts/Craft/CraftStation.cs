using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftStation : MonoBehaviour, IInteractable
{
    public int level = 1;
    public List<Item> itemsCanCraft;
    public CraftSystemUI craftSystemUI;

    public void Interact()
    {
        // Open Craft Station
        craftSystemUI.OpenCraftStation();
    }
}
