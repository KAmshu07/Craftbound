using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryTabManager tabManager;
    [SerializeField] private ItemCardUI itemCardUI;
    [SerializeField] private GameObject inventoryUI;

    public delegate void ItemSelectedHandler(IInventoryItem selectedItem);
    public event ItemSelectedHandler OnItemSelected;

    public IInventoryItem SelectedItem { get; private set; }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryUI;
        }

        if (tabManager != null)
        {
            tabManager.OnItemSelected += HandleItemSelected;
        }

        if (itemCardUI != null)
        {
            OnItemSelected += itemCardUI.SetupCard;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventoryUI;
        }

        if (tabManager != null)
        {
            tabManager.OnItemSelected -= HandleItemSelected;
        }

        if (itemCardUI != null)
        {
            OnItemSelected -= itemCardUI.SetupCard;
        }
    }

    private void UpdateInventoryUI()
    {
        if (tabManager != null)
        {
            tabManager.Refresh();
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

    private void HandleItemSelected(IInventoryItem selectedItem)
    {
        SelectedItem = selectedItem;
        OnItemSelected?.Invoke(selectedItem);
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
