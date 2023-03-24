using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    public Inventory inventory;
    private Mouse mouse;
    public ItemSlotInfo itemSlot;
    public Image itemImage;
    public TextMeshProUGUI stacksText;

    private bool click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerPress = this.gameObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        click = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (click)
        {
            OnClick();
            click = false;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        OnClick();
        click = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (click)
        {
            OnClick();
            click = false;
        }
    }

    public void PickupItem()
    {
        mouse.itemSlot = itemSlot;
        mouse.sourceItemPanel = this;
        if (Input.GetKey(KeyCode.LeftShift) && itemSlot.stacks > 1) mouse.splitSize = itemSlot.stacks / 2;
        else mouse.splitSize = itemSlot.stacks;
        mouse.SetUI();
    }
    public void FadeOut()
    {
        itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
    }
    public void DropItem()
    {
        itemSlot.item = mouse.itemSlot.item;
        if (mouse.splitSize < mouse.itemSlot.stacks)
        {
            itemSlot.stacks = mouse.splitSize;
            mouse.itemSlot.stacks -= mouse.splitSize;
            mouse.EmptySlot();
        }
        else
        {
            itemSlot.stacks = mouse.itemSlot.stacks;
            inventory.ClearSlot(mouse.itemSlot);
        }
    }
    public void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
    {
        //Hold item for transfer
        ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

        slotA.item = slotB.item;
        slotA.stacks = slotB.stacks;

        slotB.item = tempItem.item;
        slotB.stacks = tempItem.stacks;
    }
    public void StackItem(ItemSlotInfo source, ItemSlotInfo destination, int amount)
    {
        int slotsAvailable = destination.item.MaxStacks() - destination.stacks;
        if (slotsAvailable == 0) return;

        if (amount > slotsAvailable)
        {
            source.stacks -= slotsAvailable;
            destination.stacks = destination.item.MaxStacks();
        }
        if (amount <= slotsAvailable)
        {
            destination.stacks += amount;
            if (source.stacks == amount) inventory.ClearSlot(source);
            else source.stacks -= amount;
        }
    }

    public void OnClick()
    {
        if (inventory != null)
        {
            mouse = inventory.mouse;

            //Grab item if mouse slot is empty
            if (mouse.itemSlot.item == null)
            {
                if (itemSlot.item != null)
                {
                    PickupItem();
                    FadeOut();
                }
            }
            else
            {
                //Clicked on original slot
                if (itemSlot == mouse.itemSlot)
                {
                    inventory.RefreshInventory();
                }
                //Clicked on empty slot
                else if (itemSlot.item == null)
                {
                    DropItem();
                    inventory.RefreshInventory();
                }
                //Clicked on occupied slot of different item type
                else if (itemSlot.item.GiveName() != mouse.itemSlot.item.GiveName())
                {
                    SwapItem(itemSlot, mouse.itemSlot);
                    inventory.RefreshInventory();
                }
                //Clicked on occupided slot of same type
                else if (itemSlot.stacks < itemSlot.item.MaxStacks())
                {
                    StackItem(mouse.itemSlot, itemSlot, mouse.splitSize);
                    inventory.RefreshInventory();
                }
            }
        }
    }
}
