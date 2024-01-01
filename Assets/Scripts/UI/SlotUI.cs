using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySlot slot;
    public MouseItem mouseItem;
    public InventorySystemUI inventorySystemUI;
    public (Storage, Storage) originDestination;

    public void SetupSlotUI()
    {
        UpdateSlotUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (mouseItem.GetItem() != null && slot.item == null)
            {
                // Place item in this slot
                slot.item = mouseItem.GetItem();
                UpdateSlotUI();
                mouseItem.ClearMouseItem();
            }
            else if (mouseItem.GetItem() == null && slot.item != null)
            {
                // Take the item from this slot
                mouseItem.SetMouseItem(slot.item);
                ClearSlotUI();
            }
            else if (mouseItem.GetItem() != null && slot.item != null)
            {
                // Swap
                if (mouseItem.GetItem().name != slot.item.name)
                {
                    var tempItem = mouseItem.GetItem();
                    mouseItem.ClearMouseItem();
                    mouseItem.SetMouseItem(slot.item);
                    slot.item = null;
                    slot.item = tempItem;
                    UpdateSlotUI();
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            MoveItemToDestination();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (originDestination.Item1 != Current.Instance.backpack.GetComponent<Storage>()) return;
        this.transform.Find("DropButton").gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (originDestination.Item1 != Current.Instance.backpack.GetComponent<Storage>()) return;
        this.transform.Find("DropButton").gameObject.SetActive(false);
    }

    public void MoveItemToDestination()
    {
        if (originDestination.Item1 != null && originDestination.Item2 != null && slot.item != null)
        {
            var newSlot = originDestination.Item2.AddItem(slot.item);
            if (newSlot != null)
            {
                inventorySystemUI.UpdateSlotUIBySlot(newSlot);
                ClearSlotUI();
            }
        }
    }

    public void UpdateSlotUI()
    {
        if (slot.item != null)
        {
            this.GetComponent<Image>().sprite = slot.item.itemIcon;
        }
    }

    public void ClearSlotUI()
    {
        slot.item = null;
        this.GetComponent<Image>().sprite = null;
    }
}
