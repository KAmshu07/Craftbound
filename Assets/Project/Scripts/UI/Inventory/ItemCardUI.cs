using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemCardUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemQuantityText;
    public GameObject starPrefab;
    public Transform starsParent;

    private List<GameObject> instantiatedStars = new List<GameObject>();

    private void OnEnable()
    {
        InventoryUIManager uiManager = FindObjectOfType<InventoryUIManager>();
        if (uiManager != null)
        {
            uiManager.OnItemSelected += SetupCard;
            if (uiManager.SelectedItem != null)
            {
                SetupCard(uiManager.SelectedItem);
            }
        }
    }

    private void OnDisable()
    {
        InventoryUIManager uiManager = FindObjectOfType<InventoryUIManager>();
        if (uiManager != null)
        {
            uiManager.OnItemSelected -= SetupCard;
        }
    }

    public void SetupCard(IInventoryItem itemData)
    {
        if (itemData == null) return;

        itemIcon.sprite = itemData.Icon;
        itemNameText.text = itemData.ItemName;
        itemDescriptionText.text = itemData.Description;
        itemQuantityText.text = $"x{itemData.Quantity}";
        SetupRarity(itemData.Rarity);
    }

    private void SetupRarity(int rarity)
    {
        foreach (var star in instantiatedStars)
        {
            Destroy(star);
        }
        instantiatedStars.Clear();

        for (int i = 0; i < rarity; i++)
        {
            var starInstance = Instantiate(starPrefab, starsParent);
            instantiatedStars.Add(starInstance);
        }
    }

    public void AnimateUse()
    {
        Debug.Log("Animate the item being used");
    }

    public void ClearCard()
{
    itemIcon.sprite = null;
    itemNameText.text = "";
    itemDescriptionText.text = "";
    itemQuantityText.text = "";

    foreach (var star in instantiatedStars)
    {
        Destroy(star);
    }
    instantiatedStars.Clear();
}


    public void OnUseButtonClicked(GameObject user, IInventoryItem itemData)
    {
        if (user == null || itemData == null) return;

        itemData.Use(user);
        AnimateUse();
    }
}
