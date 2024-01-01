using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CraftSystemUI : MonoBehaviour
{
    public CraftSystem craftSystem;
    public UIControls uiControls;

    public GameObject itemCardPrefab;
    public GameObject materialCardPrefab;

    public List<GameObject> itemCards;
    public List<Pair<InventoryItem, GameObject>> materialCards;

    public void OpenCraftStation()
    {
        CreateItemCards();
        SelectItem(craftSystem.craftStation.itemsCanCraft.FirstOrDefault());
        UIReferences.Instance.craftStationUIPanel.SetActive(true);
        uiControls.OnShowingingUI();
    }

    public void CloseCraftStation()
    {
        foreach (GameObject item in itemCards)
        {
            Destroy(item);
        }
        itemCards.Clear();

        foreach (var material in materialCards)
        {
            Destroy(material.Item2());
        }
        materialCards.Clear();
        ClearErrorMsj();
        UIReferences.Instance.craftStationUIPanel.SetActive(false);
        uiControls.OnHidingUI();
    }

    private void CreateItemCards()
    {
        foreach (var item in craftSystem.craftStation.itemsCanCraft)
        {
            var itemCard = Instantiate(itemCardPrefab, Vector3.zero, Quaternion.identity, UIReferences.Instance.itemScrollViewContent.transform);
            UpdateItemCards(itemCard, item);
            itemCards.Add(itemCard);
        }
    }

    private void UpdateItemCards(GameObject itemCard, Item item)
    {
        itemCard.GetComponent<Image>().sprite = item.itemIcon;
        itemCard.GetComponent<Button>().onClick.RemoveAllListeners();
        itemCard.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));
    }

    private void SelectItem(Item item)
    {
        UIReferences.Instance.currentItemIcon.sprite = item.itemIcon;
        UIReferences.Instance.currentItemName.text = item.itemName;
        UIReferences.Instance.currentItemDescription.text = item.itemDescription;

        RemoveAllMaterialCards();
        CreateMaterialCards(item.materials);

        UIReferences.Instance.craftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        UIReferences.Instance.craftButton.GetComponent<Button>().onClick.AddListener(() => Craft(item));
    }

    private void Craft(Item item)
    {
        var craft = craftSystem.Craft(item);
        if (!craft.Item1)
        {
            StartCoroutine(ShowErrorMessage(craft.Item2));
        }

        foreach (var material in item.materials)
        {
            foreach (var materialCard in materialCards)
            {
                if (material.Item1() == materialCard.Item1())
                {
                    UpdateMaterialCards(materialCard.Item2().transform, material);
                }
            }
        }
    }

    private IEnumerator ShowErrorMessage(string errorMsj)
    {
        UIReferences.Instance.errorMsj.text = errorMsj;
        UIReferences.Instance.errorMsj.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        ClearErrorMsj();
    }

    private void ClearErrorMsj()
    {
        UIReferences.Instance.errorMsj.text = "";
        UIReferences.Instance.errorMsj.gameObject.SetActive(false);
    }

    private void UpdateMaterialCards(Transform materialCard, Pair<InventoryItem, float> material)
    {
        materialCard.Find("MaterialName").GetComponent<TextMeshProUGUI>().text = material.Item1().itemName;
        materialCard.Find("MaterialImage").GetComponent<Image>().sprite = material.Item1().itemIcon;
        materialCard.Find("MaterialsNeeded").GetComponent<TextMeshProUGUI>().text = $"x{material.Item2()}";
        materialCard.Find("MaterialsAvailable").GetComponent<TextMeshProUGUI>().text = $"x{craftSystem.MaterialsAvailable(material)}";
    }

    private void CreateMaterialCards(List<Pair<InventoryItem, float>> materials)
    {
        foreach (var material in materials)
        {
            var materialCard = Instantiate(materialCardPrefab, Vector3.zero, Quaternion.identity, UIReferences.Instance.materialScrollViewContent.transform);
            UpdateMaterialCards(materialCard.transform, material);
            materialCards.Add(new Pair<InventoryItem, GameObject>(material.Item1(), materialCard));
        }
    }

    public void RemoveAllMaterialCards()
    {
        foreach (Transform obj in UIReferences.Instance.materialScrollViewContent.transform)
        {
            Destroy(obj.gameObject);
        }

        materialCards.Clear();
    }
}
