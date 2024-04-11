using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemCardUI : EventReceiver
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
        Subscribe<ItemSelectedEvent>(OnItemSelected);
    }

    private void OnItemSelected(ItemSelectedEvent eventArgs)
    {
        SetupCard(eventArgs.Item);
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
