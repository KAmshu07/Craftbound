using UnityEngine;

public class ItemPickup : MonoBehaviour, IPickable
{
    private Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public Item GetItem()
    {
        return item;
    }

    public void Pickup(GameObject picker)
    {
        if (item != null)
        {
            item.Pickup(picker);
            Debug.Log($"Picked up {item.itemName} x{GetItemAmount()} by {picker.name}");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Item is null. Make sure to set the item before calling Pickup.");
        }
    }

    private int GetItemAmount()
    {
        if (item is Wood woodItem)
        {
            return woodItem.woodAmount;
        }
        else if (item is Fibre fibreItem)
        {
            return fibreItem.fibreAmount;
        }
        else if (item is Resin resinItem)
        {
            return resinItem.resinAmount;
        }
        else if (item is Fruit fruitItem)
        {
            return fruitItem.fruitAmount;
        }
        else if (item is Stone stoneItem)
        {
            return stoneItem.stoneAmount;
        }
        else if(item is Flint flintItem)
        {
            return flintItem.flintAmount;
        }
        else if (item is CopperOre copperOreItem)
        {
            return copperOreItem.copperAmount;
        }
        else if (item is IronOre ironOreItem)
        {
            return ironOreItem.ironAmount;
        }
        else if (item is TinOre tinOreItem)
        {
            return tinOreItem.tinAmount;
        }
        else if (item is Coal coalItem)
        {
            return coalItem.coalAmount;
        }
        else if (item is TitaniumOre titaniumOreItem)
        {
            return titaniumOreItem.titaniumAmount;
        }
        else if (item is Diamond diamondItem)
        {
            return diamondItem.diamondAmount;
        }
        return 1;
    }
}
