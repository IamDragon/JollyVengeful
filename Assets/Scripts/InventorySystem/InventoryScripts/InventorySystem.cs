using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;


    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
            inventorySlots.Add(new InventorySlot());

    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        // Check whether item exist in inventory
        if (ContainsItem(itemToAdd, out List<InventorySlot> inventorySlots))
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        // Gets the first availablt slot
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> inventorySlot)
    {
        // Retrieve a list of item slots where the slot contains same item data as the item to be added
        inventorySlot = InventorySlots.Where(item => item.ItemData == itemToAdd).ToList();
        return inventorySlot == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        // Get first slot that has no itemdata
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
