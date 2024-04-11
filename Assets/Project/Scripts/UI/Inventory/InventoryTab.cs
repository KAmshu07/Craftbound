using UnityEngine;
using UnityEngine.UI;

public class InventoryTab : EventReceiver
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
            tabButton.onClick.AddListener(Select);
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

        // Raise an event when the tab is selected
        EventDispatcher.Publish(new TabSelectedEvent(category));

        // Optionally, request the inventory system to display items of this tab's category
        tabManager?.DisplayItemsForCategory(category);
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

        // Raise an event when the tab is deselected
        EventDispatcher.Publish(new TabDeselectedEvent(category));
    }
}

// Define events for tab selection
public class TabSelectedEvent
{
    public ItemCategory Category { get; private set; }

    public TabSelectedEvent(ItemCategory category)
    {
        Category = category;
    }
}

public class TabDeselectedEvent
{
    public ItemCategory Category { get; private set; }

    public TabDeselectedEvent(ItemCategory category)
    {
        Category = category;
    }
}
