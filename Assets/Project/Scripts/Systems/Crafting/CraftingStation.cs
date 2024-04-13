using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCraftingStation", menuName = "Crafting/Station")]
public class CraftingStation : SerializedScriptableObject
{
    [Header("Station Properties")]
    [LabelText("Station Name")]
    public string stationName;

    [PreviewField(50)]
    [LabelText("Icon")]
    public Sprite icon;

    [InlineEditor(InlineEditorModes.GUIAndPreview)]
    [LabelText("Prefab")]
    public GameObject stationPrefab;

    [LabelText("Description")]
    [TextArea]
    public string description;

    [Header("Supported Recipes")]
    [ListDrawerSettings(ShowFoldout = true)]
    public List<CraftingRecipe> supportedRecipes;

    [Button("Interact", ButtonSizes.Large)]
    public void Interact()
    {
        Debug.Log($"Interacting with {stationName}");
        // Further implementation for interacting with the station
    }

    // Define how this station handles the crafting of items
    public void CraftItem(Inventory inventory, CraftingRecipe recipe)
    {
        if (supportedRecipes.Contains(recipe))
        {
            string craftResult = recipe.CanCraft(inventory);
            if (craftResult == "OK")
            {
                recipe.Craft(inventory);
                Debug.Log($"Crafting {recipe.output.ItemName} at {stationName} was successful!");
            }
            else
            {
                Debug.Log($"Cannot craft {recipe.output.ItemName} at {stationName}: {craftResult}");
            }
        }
        else
        {
            Debug.Log($"{stationName} does not support crafting {recipe.output.ItemName}");
        }
    }
}
