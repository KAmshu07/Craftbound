using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Image iconImage;
    public Text quantityText;
    public Transform rarityStarsParent; // A parent object where you'll instantiate star icons
    public GameObject starPrefab; // Prefab for a single star
    private IInventoryItem currentItemData; // Store the current item data

    public void SetupItem(IInventoryItem itemData)
    {
        currentItemData = itemData; // Store the item data
        iconImage.sprite = itemData.Icon;
        quantityText.text = $"x{itemData.Quantity}";
        SetupRarityStars(itemData.Rarity);
    }

    private void SetupRarityStars(int rarity)
    {
        // Clear the current stars
        foreach (Transform child in rarityStarsParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate stars based on rarity
        for (int i = 0; i < rarity; i++)
        {
            Instantiate(starPrefab, rarityStarsParent);
        }
    }

    public void Select()
    {

    }

    public void Deselect()
    {

    }

    public IInventoryItem ItemData => currentItemData;
}
