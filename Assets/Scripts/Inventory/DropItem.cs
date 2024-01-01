using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Drop();
        }
    }

    public void Drop()
    {
        if (this.GetComponentInParent<SlotUI>() != null)
        {
            var slotUI = this.GetComponentInParent<SlotUI>();

            if (slotUI.slot.item != null)
            {
                var player = Current.Instance.player.transform;
                var itemDropped = Instantiate(slotUI.slot.item.itemPrefab, player.position + (player.forward * 2), Quaternion.identity);
                slotUI.ClearSlotUI();
            }
        }
    }
}
