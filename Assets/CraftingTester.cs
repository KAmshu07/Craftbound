using UnityEngine;
using UnityEngine.UI;

public class CraftingTester : MonoBehaviour
{
    public Inventory playerInventory;
    public CraftingRecipe woodenAxeRecipe;
    public Button craftButton;

    void Start()
    {
        // Ensure the button is hooked up to attempt crafting when clicked
        craftButton.onClick.AddListener(TryCraftAxe);
    }

    public void TryCraftAxe()
    {
        string result = woodenAxeRecipe.CanCraft(playerInventory);
        if (result == "OK")
        {
            woodenAxeRecipe.Craft(playerInventory);
            Debug.Log("Crafting successful!");
        }
        else
        {
            Debug.Log($"Crafting failed: {result}");
        }
    }

}
