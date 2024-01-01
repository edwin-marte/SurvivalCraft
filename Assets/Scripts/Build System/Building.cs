using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    private BuildingData assingnedData;
    private BoxCollider boxCollider;
    private GameObject graphic;
    private Transform colliders;
    private bool isOverlapping;

    public BuildingData AssignedData => assingnedData;
    public bool IsOverlapping => isOverlapping;

    private Renderer _renderer;
    private Material defaultMaterial;

    private bool flaggedForDelete;
    public bool FlaggedForDelete => flaggedForDelete;

    public void Init(BuildingData data)
    {
        this.assingnedData = data;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = assingnedData.buildingSize;
        boxCollider.center = new Vector3(0, (assingnedData.buildingSize.y + 0.2f) * 0.5f, 0f);
        boxCollider.isTrigger = true;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        graphic = Instantiate(data.prefab, transform);
        _renderer = graphic.GetComponentInChildren<Renderer>();
        defaultMaterial = _renderer.material;

        colliders = graphic.transform.Find("Colliders");
        if (colliders != null)
        {
            colliders.gameObject.SetActive(false);
        }
    }

    public void PlaceBuilding()
    {
        boxCollider.enabled = false;
        if (colliders != null)
        {
            GetComponent<Collider>().gameObject.SetActive(true);
        }
        UpdateMaterial(defaultMaterial);
        gameObject.layer = 7;
        gameObject.name = assingnedData.displayName + " - " + transform.position;
    }

    public void UpdateMaterial(Material newMaterial)
    {
        if (_renderer.material == null) return;
        if (_renderer.material != newMaterial) _renderer.material = newMaterial;
    }

    public void PlaceMaterial()
    {
        _renderer.material = defaultMaterial;
    }

    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterial(deleteMat);
        flaggedForDelete = true;
    }

    public void RemoveDeleteFlag()
    {
        UpdateMaterial(defaultMaterial);
        flaggedForDelete = false;
    }

    private void OnTriggerStay(Collider other)
    {
        isOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isOverlapping = false;
    }
}
