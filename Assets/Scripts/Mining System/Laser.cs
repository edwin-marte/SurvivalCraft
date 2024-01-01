using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject beam;
    [SerializeField] private float maxLength;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private LayerMask oreGroupLayer;

    public Animator armAnimator;
    public Animator headAnimator;

    public static event Action OnInteract;
    public static event Action OnStopInteract;

    private bool isInteracting;

    private bool isOreGroup = false;

    private void Awake()
    {
        DeactivateBeam();
    }

    private void ActivateBeam()
    {
        beam.SetActive(true);
    }

    private void DeactivateBeam()
    {
        beam.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isOreGroup)
        {
            ActivateBeam();
            armAnimator.SetBool("Mining", true);
            headAnimator.SetBool("Mining", true);

            if (!isInteracting)
            {
                isInteracting = true;
                OnInteract?.Invoke();
            }
        }
        else
        {
            DeactivateBeam();
            armAnimator.SetBool("Mining", false);
            headAnimator.SetBool("Mining", false);

            if (isInteracting)
            {
                isInteracting = false;
                OnStopInteract?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLength, oreGroupLayer) && hit.distance <= 3f)
        {
            hitParticles.transform.position = hit.point;
            Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);
            beam.GetComponentInChildren<LineRenderer>().SetPosition(1, localHitPoint);
            isOreGroup = true;
        }
        else
        {
            isOreGroup = false;
        }

        // Visualize the raycast with Debug.DrawRay
        Debug.DrawRay(ray.origin, ray.direction * maxLength, Color.red);
    }
}
