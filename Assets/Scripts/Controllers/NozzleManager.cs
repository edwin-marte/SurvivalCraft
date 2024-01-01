using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> muzzles;

    private GunMode currentMode;

    private void Start()
    {
        currentMode = GunMode.mining;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DisableMuzzles();
            muzzles[0].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DisableMuzzles();
            muzzles[1].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Building nozzle
        }
    }

    private void DisableMuzzles()
    {
        foreach (GameObject obj in muzzles)
        {
            obj.SetActive(false);
        }
    }
}

public enum GunMode
{
    mining,
    vacuuming,
    building,
    deconstructing
}
