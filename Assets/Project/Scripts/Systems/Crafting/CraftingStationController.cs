using UnityEngine;

public class CraftingStationController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CraftingStation craftingStation; // Link to the CraftingStation scriptable object

    public void Interact()
    {
        Debug.Log($"Player is interacting with {craftingStation.stationName}");
        OpenCraftingUI(); // Opens the crafting UI
    }

    private void OpenCraftingUI()
    {
        // Implementation to open the crafting UI and pass the supported recipes
        //CraftingUIManager.Instance.ShowCraftingUI(craftingStation.supportedRecipes);
    }
}
