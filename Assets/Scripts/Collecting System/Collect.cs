using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    [SerializeField] private float maxLength;
    [SerializeField] private LayerMask itemLayer;

    private bool isOre = false;
    private Absortion absortion = null;
    private ItemObject itemObject = null;
    private InventorySystem inventorySystem;

    private void Update()
    {
        if (Input.GetMouseButton(0) && isOre && absortion != null)
        {
            absortion.Absorb(transform.position);
            if (!absortion.AddedToInventory())
            {
                inventorySystem = Current.Instance.backpack.GetComponent<InventorySystem>();

                if (inventorySystem != null && itemObject != null)
                {
                    inventorySystem.AddItem(itemObject.GetItem());
                }
                absortion.AddToInventory();
            }
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLength, itemLayer) && hit.distance <= 3f)
        {
            isOre = true;
            absortion = hit.collider.GetComponent<Absortion>();
            itemObject = hit.collider.GetComponent<ItemObject>();
        }
        else
        {
            isOre = false;
            absortion = null;
        }

        // Visualize the raycast with Debug.DrawRay
        Debug.DrawRay(ray.origin, ray.direction * maxLength, Color.red);
    }
}
