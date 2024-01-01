using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReferences : MonoBehaviour
{
    public static UIReferences Instance;

    public GameObject backpackPanel;
    public GameObject backpackContainer;
    public GameObject storageContainer;

    //Crafting
    public GameObject craftStationUIPanel;
    public Image currentItemIcon;
    public TextMeshProUGUI currentItemName;
    public TextMeshProUGUI currentItemDescription;
    public GameObject itemScrollViewContent;
    public GameObject materialScrollViewContent;
    public GameObject craftButton;
    public TextMeshProUGUI errorMsj;

    private void Awake()
    {
        Instance = this;
    }
}
