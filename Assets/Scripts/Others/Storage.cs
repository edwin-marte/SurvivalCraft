using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : InventorySystem, IInteractable
{
    public void Interact()
    {
        UIControls.Instance.OpenBackpack(this);
        UIControls.Instance.OpenStorage((this, UIControls.Instance.playerBackpack), slots, UIReferences.Instance.storageContainer.transform);
    }
}
