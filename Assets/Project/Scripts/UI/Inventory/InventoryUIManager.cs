using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryTabManager tabManager;

    private void OnEnable()
    {
        inventory.OnInventoryChanged += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= UpdateInventoryUI;
    }

    private void UpdateInventoryUI()
    {
        tabManager.Refresh();
    }
}
