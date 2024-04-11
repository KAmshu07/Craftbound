using UnityEngine;

public class InventoryUIManager : EventReceiver
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryTabManager tabManager;
    [SerializeField] private ItemCardUI itemCardUI;
    [SerializeField] private GameObject inventoryUI; // Assign the inventory UI GameObject in the inspector

    public IInventoryItem SelectedItem { get; private set; }

    private void OnEnable()
    {
        Subscribe<InventoryChangedEvent>(UpdateInventoryUI);
        Subscribe<ItemSelectedEvent>(HandleItemSelected);
    }

    private void UpdateInventoryUI(InventoryChangedEvent eventArgs)
    {
        if (tabManager != null)
        {
            tabManager.Refresh();
        }
    }

    private void HandleItemSelected(ItemSelectedEvent eventArgs)
    {
        SelectedItem = eventArgs.Item;
        if (itemCardUI != null)
        {
            itemCardUI.SetupCard(SelectedItem);
        }
    }

    public void ClearItemCard()
    {
        if (itemCardUI != null)
        {
            itemCardUI.ClearCard();
        }
    }

    public void ClearSelectedItem()
    {
        SelectedItem = null;
        ClearItemCard();
    }

    public void OpenInventory()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(true);

            if (tabManager != null && tabManager.ActiveTab != null)
            {
                tabManager.ActivateTab(tabManager.ActiveTab);
            }
        }
    }

    public void CloseInventory()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
            ClearSelectedItem();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryUI.activeSelf)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }
}
