using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabManager : MonoBehaviour
{
    [SerializeField] private List<InventoryTab> tabs = new List<InventoryTab>();
    [SerializeField] private GameObject itemUIPrefab;
    public GameObject itemCardUI;
    private InventoryTab activeTab;
    private Inventory inventory;
    private InventoryItemUI selectedItemUI; // Store the currently selected item UI

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogError("Player not found. Make sure there is a GameObject with the tag 'Player' that has an Inventory component.");
        }

        foreach (var tab in GetComponentsInChildren<InventoryTab>(includeInactive: true))
        {
            tabs.Add(tab);
            tab.Init(this, tab.category);
        }

        if (tabs.Count > 0)
        {
            activeTab = tabs[0];
            activeTab.Select();
        }
    }

    public void ActivateTab(InventoryTab selectedTab)
    {
        if (activeTab != null)
        {
            activeTab.Deselect();
        }

        activeTab = selectedTab;
        activeTab.Select();

        DisplayItemsForCategory(activeTab.category);
    }

    public void DisplayItemsForCategory(ItemCategory category)
    {
        // Clear existing items from the container
        foreach (Transform child in activeTab.itemsContainer)
        {
            Destroy(child.gameObject);
        }

        // Display new items for the selected category
        List<IInventoryItem> items = inventory.GetItemsByCategory(category);
        foreach (var item in items)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, activeTab.itemsContainer);
            InventoryItemUI inventoryItemUI = itemUI.GetComponent<InventoryItemUI>();
            inventoryItemUI.SetupItem(item); // Set the data for the item UI element

            // Add listener for selection
            itemUI.GetComponent<Button>().onClick.AddListener(() => SelectItem(inventoryItemUI));
        }
    }

    private void SelectItem(InventoryItemUI inventoryItemUI)
    {
        if (selectedItemUI != null)
        {
            // Deselect the previous item
            selectedItemUI.Deselect();
        }

        selectedItemUI = inventoryItemUI;
        selectedItemUI.Select(); // Highlight the selected item
        itemCardUI.GetComponent<ItemCardUI>().SetupCard(selectedItemUI.ItemData); // Update the item card with the selected item's data
    }

    public void Refresh()
    {
        DisplayItemsForCategory(activeTab.category);
    }
}
