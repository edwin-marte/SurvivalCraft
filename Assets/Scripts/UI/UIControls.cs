using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public Storage playerBackpack;
    public InventorySystemUI inventorySystemUI;
    public FirstPersonController characterController;
    public CraftSystemUI craftSystemUI;

    public Animator armAnimator;
    public Animator headAnimator;

    public static UIControls Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UIReferences.Instance.backpackPanel.activeSelf)
            {
                CloseStorage();
            }
            else
            {
                if (!IsUIPanelOpened())
                {
                    OpenBackpack(null);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIReferences.Instance.backpackPanel.activeSelf)
            {
                CloseStorage();
            }
            if (UIReferences.Instance.craftStationUIPanel.activeSelf)
            {
                craftSystemUI.CloseCraftStation();
            }
        }
    }

    private bool IsUIPanelOpened()
    {
        if (UIReferences.Instance.backpackPanel.activeSelf) return true;
        if (UIReferences.Instance.craftStationUIPanel.activeSelf) return true;
        return false;
    }

    public void OpenStorage((Storage, Storage) originDestination, List<InventorySlot> slots, Transform container)
    {
        UIReferences.Instance.backpackPanel.SetActive(true);
        inventorySystemUI.CreateSlotsUI(originDestination, slots, container);
        OnShowingingUI();
    }

    public void OpenBackpack(Storage destiny)
    {
        OpenStorage((playerBackpack, destiny), playerBackpack.slots, UIReferences.Instance.backpackContainer.transform);
    }

    private void CloseStorage()
    {
        UIReferences.Instance.backpackPanel.SetActive(false);
        OnHidingUI();

        foreach (GameObject slotUI in inventorySystemUI.uiSlots)
        {
            Destroy(slotUI);
        }

        inventorySystemUI.uiSlots.Clear();
    }

    public void OnShowingingUI()
    {
        // Set Idle
        armAnimator.SetFloat("Speed", 0f);
        headAnimator.SetFloat("Speed", 0f);

        characterController.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnHidingUI()
    {
        characterController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
