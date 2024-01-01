using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
    public GameObject player;
    public GameObject backpack;

    public static Current Instance;

    private void Awake()
    {
        Instance = this;
    }
}
