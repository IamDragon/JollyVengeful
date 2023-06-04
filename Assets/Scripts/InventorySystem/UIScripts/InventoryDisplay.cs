using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    { }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)      //Slot value - the "under the hood" inventory slot.
            {
                slot.Key.UpdateUISlot(updatedSlot);     //Slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.ctrlKey.isPressed;

        Debug.Log("SlotClicked method invoked");
        bool slotIsEmpty = clickedUISlot.AssignedInventorySlot.ItemData == null;
        bool mouseSlotIsEmpty = mouseInventoryItem.AssignedInventorySlot.ItemData == null;


        //Clicked slot has an item - mouse doesn't have an item -> pick up that item
        if (!slotIsEmpty && mouseSlotIsEmpty)
        {
            //If player is holding shift key -> split the stack
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot slotWithSplittedStack))
            {
                mouseInventoryItem.UpdateMouseSlot(slotWithSplittedStack);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }

        //Clicked slot doesn't have an item - mouse does have an item -> place the mouse item into the empty slot
        if (slotIsEmpty && !mouseSlotIsEmpty)
        {
            DropItem(clickedUISlot);
            return;
        }

        if (!slotIsEmpty && !mouseSlotIsEmpty)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;
            bool isRoomInStack = clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize);

            if (isSameItem && isRoomInStack)
            {
                FillStackRemoveMouseItem(clickedUISlot);
                return;
            }
            else if (isSameItem && !clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                // If stack is full -> Swap item. Else -> Fill stack with mouse inventory 
                if (leftInStack < 1)
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
                else
                {
                    FillStack(clickedUISlot, leftInStack);
                    return;
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void DropItem(InventorySlot_UI clickedUISlot)
    {
        clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
        clickedUISlot.UpdateUISlot();

        mouseInventoryItem.ClearSlot();
    }

    private void FillStackRemoveMouseItem(InventorySlot_UI clickedUISlot)
    {
        clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
        clickedUISlot.UpdateUISlot();

        mouseInventoryItem.ClearSlot();
    }

    private void FillStack(InventorySlot_UI clickedUISlot, int leftInStack)
    {
        int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
        clickedUISlot.UpdateUISlot();

        var updatedMouseItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(updatedMouseItem);
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
        clickedUISlot.ClearSlot();

        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
