using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabManager : MonoBehaviour
{
    [SerializeField] private List<InventoryTab> tabs = new List<InventoryTab>();
    [SerializeField] private GameObject itemUIPrefab;
    private InventoryTab activeTab;
    public InventoryTab ActiveTab => activeTab;
    private Inventory inventory;

    public delegate void ItemSelectedHandler(IInventoryItem selectedItem);
    public event ItemSelectedHandler OnItemSelected;

    private void OnEnable()
    {
        Refresh();
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

        // Automatically select the first item in the category
        if (activeTab.itemsContainer.childCount > 0)
        {
            GameObject firstItemUI = activeTab.itemsContainer.GetChild(0).gameObject;
            InventoryItemUI inventoryItemUI = firstItemUI?.GetComponent<InventoryItemUI>();
            if (inventoryItemUI != null)
            {
                SelectItem(inventoryItemUI);
            }
        }
        else
        {
            StartCoroutine(DelayedClearItemCard());
        }
    }


    private IEnumerator DelayedSelectFirstItem()
    {
        yield return null; // Wait for one frame

        if (activeTab.itemsContainer.childCount > 0)
        {
            GameObject firstItemUI = activeTab.itemsContainer.GetChild(0).gameObject;
            InventoryItemUI inventoryItemUI = firstItemUI?.GetComponent<InventoryItemUI>();
            if (inventoryItemUI != null)
            {
                SelectItem(inventoryItemUI);
            }
        }
    }

    private IEnumerator DelayedClearItemCard()
    {
        yield return new WaitForEndOfFrame();

        InventoryUIManager uiManager = FindObjectOfType<InventoryUIManager>();
        if (uiManager != null)
        {
            uiManager.ClearItemCard();
        }
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
            foreach (var item in items)
            {
                GameObject itemUI = Instantiate(itemUIPrefab, activeTab.itemsContainer);
                InventoryItemUI inventoryItemUI = itemUI.GetComponent<InventoryItemUI>();
                inventoryItemUI.SetupItem(item);

                itemUI.GetComponent<Button>().onClick.AddListener(() => SelectItem(inventoryItemUI));
            }
        }
    }

    private void SelectItem(InventoryItemUI inventoryItemUI)
    {
        if (inventoryItemUI != null)
        {
            inventoryItemUI.Select();
            OnItemSelected?.Invoke(inventoryItemUI.ItemData);
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
