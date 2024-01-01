using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModelLayerMask;
    [SerializeField] private LayerMask deleteModelLayerMask;
    [SerializeField] private int defaultLayerInt = 8;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMaterialPositive;
    [SerializeField] private Material buildingMaterialNegative;

    private bool deleteModeEnabled;
    private Camera camera;

    [SerializeField] private Building spawnedBuilding;
    private Building targetBuilding;
    private Quaternion lastRotation;

    public BuildingData data;

    private void Start()
    {
        camera = Camera.main;
        ChoosePart(data);
    }

    private void ChoosePart(BuildingData data)
    {
        if (deleteModeEnabled)
        {
            if (targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
            }
            targetBuilding = null;
            deleteModeEnabled = false;
        }

        DeleteObjectPreview();

        var go = new GameObject
        {
            layer = defaultLayerInt,
            name = "Build Preview"
        };

        spawnedBuilding = go.AddComponent<Building>();
        spawnedBuilding.Init(data);
        spawnedBuilding.transform.rotation = lastRotation;
    }

    private void Update()
    {
        if (spawnedBuilding && Input.GetKeyDown(KeyCode.Escape)) DeleteObjectPreview();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            deleteModeEnabled = !deleteModeEnabled;
        }

        if (deleteModeEnabled)
            DeleteMode();
        else
            BuildMode();
    }

    private void DeleteObjectPreview()
    {
        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding.gameObject);
            spawnedBuilding = null;
        }
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, layerMask);
    }

    private void DeleteMode()
    {
        if (IsRayHittingSomething(deleteModelLayerMask, out RaycastHit hitInfo))
        {
            var detectedBuilding = hitInfo.collider.gameObject.GetComponent<Building>();

            if (detectedBuilding == null) return;
            if (targetBuilding == null) targetBuilding = detectedBuilding;

            if (detectedBuilding != targetBuilding && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
                targetBuilding = detectedBuilding;
            }

            if (detectedBuilding == targetBuilding && !targetBuilding.FlaggedForDelete)
            {
                targetBuilding.FlagForDelete(buildingMaterialNegative);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Destroy(hitInfo.collider.gameObject);
                targetBuilding = null;
            }
        }
        else
        {
            if (targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
                targetBuilding = null;
            }
        }
    }

    private void BuildMode()
    {
        if (targetBuilding != null && targetBuilding.FlaggedForDelete)
        {
            targetBuilding.RemoveDeleteFlag();
            targetBuilding = null;
        }

        if (spawnedBuilding == null) return;

        PositionBuildingPreview();
    }

    private void PositionBuildingPreview()
    {
        spawnedBuilding.UpdateMaterial(spawnedBuilding.IsOverlapping ? buildingMaterialNegative : buildingMaterialPositive);

        if (Input.GetKeyDown(KeyCode.R))
        {
            spawnedBuilding.transform.Rotate(0f, 45f, 0f);
            lastRotation = spawnedBuilding.transform.rotation;
        }

        if (IsRayHittingSomething(buildModelLayerMask, out RaycastHit hitInfo))
        {
            var gridPosition = WorldGrid.GridPositionFromWorldPoint(hitInfo.point, 1f);
            spawnedBuilding.transform.position = gridPosition;

            if (Input.GetMouseButtonDown(0) && !spawnedBuilding.IsOverlapping)
            {
                spawnedBuilding.PlaceBuilding();
                var dataCopy = spawnedBuilding.AssignedData;
                spawnedBuilding = null;
                ChoosePart(dataCopy);
            }
        }
    }
}
