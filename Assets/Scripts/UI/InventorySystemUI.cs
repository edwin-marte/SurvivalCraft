using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventorySystemUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public MouseItem mouseItem;

    public List<GameObject> uiSlots = new List<GameObject>();

    public void CreateSlotsUI((Storage, Storage) originDestination, List<InventorySlot> slots, Transform container)
    {
        foreach (var slot in slots)
        {
            var slotUIGameObject = Instantiate(slotPrefab, transform.position, Quaternion.identity, container);
            var slotUI = slotUIGameObject.GetComponent<SlotUI>();

            slotUI.slot = slot;
            slotUI.mouseItem = mouseItem;
            slotUI.originDestination = originDestination;
            slotUI.inventorySystemUI = this;
            slotUI.SetupSlotUI();
            uiSlots.Add(slotUIGameObject);
        }
    }

    public void TakeAll()
    {
        var backack = Current.Instance.backpack.GetComponent<InventorySystem>();
        foreach (GameObject slotGameObject in uiSlots.FindAll(x => x.GetComponent<SlotUI>().originDestination.Item1 != backack))
        {
            var slotUI = slotGameObject.GetComponent<SlotUI>();
            slotUI.MoveItemToDestination();
            slotUI.UpdateSlotUI();
        }
    }

    public void PutAll()
    {
        var backack = Current.Instance.backpack.GetComponent<InventorySystem>();
        foreach (GameObject slotGameObject in uiSlots.FindAll(x => x.GetComponent<SlotUI>().originDestination.Item1 == backack))
        {
            var slotUI = slotGameObject.GetComponent<SlotUI>();
            slotUI.MoveItemToDestination();
            slotUI.UpdateSlotUI();
        }
    }

    public void UpdateSlotUIBySlot(InventorySlot targetSlot)
    {
        uiSlots.FirstOrDefault(x => x.GetComponent<SlotUI>().slot == targetSlot).GetComponent<SlotUI>().UpdateSlotUI();
    }
}
