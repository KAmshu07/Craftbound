using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : SerializedScriptableObject
{
    public List<Ingredient> ingredients;
    public Item output;
    public int outputQuantity;

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int quantity;
    }

    // This method now returns a string with error messages if crafting cannot be completed.
    public string CanCraft(Inventory inventory)
    {
        List<string> errors = new List<string>();
        foreach (var ingredient in ingredients)
        {
            int currentQuantity = inventory.GetItemCount(ingredient.item);

            if (currentQuantity < ingredient.quantity)
            {
                if (currentQuantity == 0)
                {
                    errors.Add($"Missing: {ingredient.item.ItemName}");
                }
                else
                {
                    errors.Add($"Insufficient quantity of {ingredient.item.ItemName} (Need {ingredient.quantity}, Have {currentQuantity})");
                }
            }
        }

        string errorResult = errors.Count > 0 ? string.Join("\n", errors) : "OK";
        return errorResult;
    }

    public void Craft(Inventory inventory)
    {
        string canCraft = CanCraft(inventory);
        if (canCraft == "OK")
        {
            foreach (var ingredient in ingredients)
            {
                inventory.RemoveItem(ingredient.item, ingredient.quantity);
            }
            inventory.AddItem(output, outputQuantity);
            Debug.Log("Crafting successful!");
        }
        else
        {
            Debug.Log(canCraft); // Or use this message to display in the UI
        }
    }
}
