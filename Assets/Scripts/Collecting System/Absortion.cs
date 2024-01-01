using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absortion : MonoBehaviour
{
    private bool isMoving = false;
    private float absortionSpeed = 15f;
    private Vector3 targetPosition;
    private bool addedToInventory = false;

    private void Update()
    {
        if (!isMoving) return;
        MoveToTarget();
    }

    public void AddToInventory()
    {
        addedToInventory = true;
    }

    public bool AddedToInventory()
    {
        return addedToInventory;
    }

    public void Absorb(Vector3 target)
    {
        isMoving = true;
        targetPosition = target;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, absortionSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }
}
