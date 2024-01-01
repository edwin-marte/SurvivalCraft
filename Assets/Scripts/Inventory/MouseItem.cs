using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseItem : MonoBehaviour
{
    public Vector2 offset = Vector2.zero;
    public GameObject imageGameObject;

    private InventoryItem item;

    private void Start()
    {
        ClearMouseItem();
    }

    private void SetupIcon()
    {
        imageGameObject.GetComponent<Image>().sprite = item.itemIcon;
    }

    public InventoryItem GetItem()
    {
        return this.item;
    }

    public void SetMouseItem(InventoryItem item)
    {
        this.item = item;
        SetupIcon();
        ShowIcon();
    }

    public void ClearMouseItem()
    {
        this.item = null;
        imageGameObject.GetComponent<Image>().sprite = null;
        HideIcon();
    }

    public void ShowIcon()
    {
        imageGameObject.SetActive(true);
    }

    public void HideIcon()
    {
        imageGameObject.SetActive(false);
    }

    private void Update()
    {
        if (item == null) return;

        this.GetComponent<RectTransform>().position = (Input.mousePosition + (Vector3)offset);
    }
}
