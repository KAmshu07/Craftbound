using UnityEngine;
using UnityEngine.UI;

public class InventoryTab : MonoBehaviour
{
    [SerializeField] private Button tabButton;
    public GameObject contentPanel; // Panel that gets activated/deactivated when the tab is selected/deselected
    public Transform itemsContainer; // The content area of the scroll view where item prefabs will be instantiated
    private InventoryTabManager tabManager;
    public ItemCategory category; // The category this tab represents

    public void Init(InventoryTabManager manager, ItemCategory category)
    {
        tabManager = manager;
        this.category = category;

        if (tabButton != null)
        {
            tabButton.onClick.AddListener(() => tabManager.ActivateTab(this));
        }
        else
        {
            Debug.LogError("Tab button is not assigned for " + gameObject.name);
        }
    }

    public void Select()
    {
        if (contentPanel != null)
        {
            contentPanel.SetActive(true);
        }

        if (tabButton != null)
        {
            tabButton.interactable = false;
        }

        // Optionally, request the inventory system to display items of this tab's category
        tabManager.DisplayItemsForCategory(category);
    }

    public void Deselect()
    {
        if (contentPanel != null)
        {
            contentPanel.SetActive(false);
        }

        if (tabButton != null)
        {
            tabButton.interactable = true;
        }
    }
}
