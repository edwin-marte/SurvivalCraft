using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Material positiveGhostMaterial;
    public Material negativeGhostMaterial;
    public float maxPlacementDistance = 10f;
    public LayerMask groundLayerMask;

    private bool deleteMode = false;
    private bool canRotate = true;
    private Quaternion rotationIncrement = Quaternion.Euler(0f, 45f, 0f);

    private GameObject currentGhost;
    private GameObject currentBuilding;

    private void Update()
    {
        ValidatePlacement();

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            deleteMode = false;
            Destroy(currentGhost);
            CreateGhost();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            deleteMode = true;
            Destroy(currentGhost);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitBuildMode();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RotateGhost();
        }

        if (!deleteMode)
        {
            HandleBuildMode();
        }
        else
        {
            HandleDeleteMode();
        }
    }

    private void HandleBuildMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentGhost != null && CanPlaceBuilding())
            {
                PlaceBuilding();
            }
        }

        if (currentGhost != null)
        {
            UpdateGhostPosition();
        }
    }

    private void HandleDeleteMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Building"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void ExitBuildMode()
    {
        deleteMode = true;
        Destroy(currentGhost);
        currentBuilding = null; // Reset currentBuilding since we can't place objects without being in build mode
    }

    private void CreateGhost()
    {
        currentGhost = Instantiate(buildingPrefab, GetMousePosition(), Quaternion.identity);
        currentGhost.GetComponent<Collider>().enabled = false;
        SetGhostMaterial(positiveGhostMaterial);
        AdjustGhostHeight();
        currentBuilding = null;
    }

    private void PlaceBuilding()
    {
        if (currentBuilding == null)
        {
            currentBuilding = Instantiate(buildingPrefab, AdjustBuildingHeight(), currentGhost.transform.rotation);
        }

        Destroy(currentGhost);
        CreateGhost();
    }

    private void UpdateGhostPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            Vector3 targetPosition = hit.point;
            Vector3 playerPosition = transform.position;
            Vector3 direction = targetPosition - playerPosition;
            float distance = direction.magnitude;

            if (distance > maxPlacementDistance)
            {
                direction = direction.normalized * maxPlacementDistance;
                targetPosition = playerPosition + direction;
            }

            currentGhost.transform.position = targetPosition;
            AdjustGhostHeight();
        }
    }

    private (Vector3, float) AdjustPosition()
    {
        Renderer ghostRenderer = currentGhost.GetComponentInChildren<Renderer>();
        float ghostHeightOffset = ghostRenderer.bounds.size.y / 2f;
        return (currentGhost.transform.position, ghostHeightOffset);
    }

    private Vector3 AdjustBuildingHeight()
    {
        var newPosition = AdjustPosition();
        newPosition.Item1.y = GetGroundHeight(currentGhost.transform.position) + newPosition.Item2;
        return newPosition.Item1;
    }

    private void AdjustGhostHeight()
    {
        var newPosition = AdjustPosition();
        newPosition.Item1.y = GetGroundHeight(currentGhost.transform.position) + (newPosition.Item2 + 0.05f);
        currentGhost.transform.position = newPosition.Item1;
    }

    private void SetGhostMaterial(Material material)
    {
        Renderer[] renderers = currentGhost.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    private float GetGroundHeight(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * 100f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            return hit.point.y;
        }
        return 0f;
    }

    private bool ValidatePlacement()
    {
        if (currentGhost == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.layer);
            if (groundLayerMask == (groundLayerMask | (1 << hit.collider.gameObject.layer)))
            {
                SetGhostMaterial(positiveGhostMaterial);
                return true;
            }
        }

        SetGhostMaterial(negativeGhostMaterial);
        return false;
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private bool CanPlaceBuilding()
    {
        Vector3 playerPosition = transform.position;
        float distanceToGhost = Vector3.Distance(currentGhost.transform.position, playerPosition);
        return ((distanceToGhost <= maxPlacementDistance) && ValidatePlacement());
    }

    private void RotateGhost()
    {
        if (canRotate)
        {
            currentGhost.transform.rotation *= rotationIncrement;
        }
    }
}
