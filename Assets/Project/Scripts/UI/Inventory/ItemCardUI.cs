using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemCardUI : MonoBehaviour
{
    // UI elements for the item card
    public Image itemIcon;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemQuantityText;
    public GameObject starPrefab; // Prefab for the star icon indicating rarity
    public Transform starsParent; // Parent object to instantiate stars under

    private List<GameObject> instantiatedStars = new List<GameObject>(); // Keep track of instantiated stars

    public void SetupCard(IInventoryItem itemData)
    { 
        // Set the item icon, name, description, and quantity
        itemIcon.sprite = itemData.Icon;
        itemNameText.text = itemData.ItemName;
        itemDescriptionText.text = itemData.Description;
        itemQuantityText.text = $"x{itemData.Quantity}";

        // Update the rarity display
        SetupRarity(itemData.Rarity);
    }

    private void SetupRarity(int rarity)
    {
        // Clear out the old stars
        foreach (var star in instantiatedStars)
        {
            Destroy(star);
        }
        instantiatedStars.Clear();

        // Instantiate new stars based on the rarity
        for (int i = 0; i < rarity; i++)
        {
            var starInstance = Instantiate(starPrefab, starsParent);
            instantiatedStars.Add(starInstance);
        }
    }

    // Optionally, you could have a method to animate or highlight the use of the item
    public void AnimateUse()
    {
        Debug.Log("Animate the item being used");
        // Add animations or effects to show the item being used
    }

    // This method can be called when the "Use" button is clicked in the UI.
    public void OnUseButtonClicked(GameObject user, IInventoryItem itemData)
    {
        itemData.Use(user);
        AnimateUse();
    }
}
