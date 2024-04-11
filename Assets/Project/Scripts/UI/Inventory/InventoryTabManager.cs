using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabManager : EventReceiver
{
    [SerializeField] private List<InventoryTab> tabs = new List<InventoryTab>();
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private ItemCardUI itemCardUI;
    private InventoryTab activeTab;
    public InventoryTab ActiveTab => activeTab;
    private Inventory inventory;

    private void OnEnable()
    {
        Refresh();
        Subscribe<TabSelectedEvent>(OnTabSelected);
        Subscribe<TabDeselectedEvent>(OnTabDeselected);
    }

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
    }

    private void OnTabSelected(TabSelectedEvent eventArgs)
    {
        InventoryTab selectedTab = tabs.Find(tab => tab.category == eventArgs.Category);
        if (selectedTab != null && selectedTab != activeTab) // Check if the selected tab is not already the active tab
        {
            ActivateTab(selectedTab);
        }
    }

    private void OnTabDeselected(TabDeselectedEvent eventArgs)
    {
        // Optional: Handle tab deselection if needed
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
        foreach (Transform child in activeTab.itemsContainer)
        {
            Destroy(child.gameObject);
        }

        if (inventory != null && itemUIPrefab != null)
        {
            List<IInventoryItem> items = inventory.GetItemsByCategory(category);
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    GameObject itemUI = Instantiate(itemUIPrefab, activeTab.itemsContainer);
                    InventoryItemUI inventoryItemUI = itemUI.GetComponent<InventoryItemUI>();
                    inventoryItemUI.SetupItem(item);

                    itemUI.GetComponent<Button>().onClick.AddListener(() => SelectItem(inventoryItemUI));
                }

                // Automatically select the first item in the category
                SelectItem(activeTab.itemsContainer.GetChild(0).GetComponent<InventoryItemUI>());
            }
            else
            {
                // Clear the item card if no items are available in the selected category
                if (itemCardUI != null)
                {
                    itemCardUI.ClearCard();
                }
            }
        }
    }


    private void SelectItem(InventoryItemUI inventoryItemUI)
    {
        if (inventoryItemUI != null)
        {
            inventoryItemUI.Select();
        }
    }

    public void Refresh()
    {
        if (activeTab != null)
        {
            DisplayItemsForCategory(activeTab.category);
        }
    }
}
