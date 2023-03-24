using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mouse : MonoBehaviour
{
    public GameObject mouseItemUI;
    public Image mouseCursor;
    public ItemSlotInfo itemSlot;
    public Image itemImage;
    public TextMeshProUGUI stacksText;

    public ItemPanel sourceItemPanel;
    public int splitSize;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            mouseCursor.enabled = false;
            mouseItemUI.SetActive(false);
        }
        else
        {
            mouseCursor.enabled = true;

            if (itemSlot.item != null)
            {
                mouseItemUI.SetActive(true);
            }
            else
            {
                mouseItemUI.SetActive(false);
            }
        }
        if (itemSlot.item != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && splitSize < itemSlot.stacks)
            {
                splitSize++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && splitSize > 1)
            {
                splitSize--;
            }
            stacksText.text = "" + splitSize;
            if (splitSize == itemSlot.stacks) sourceItemPanel.stacksText.gameObject.SetActive(false);
            else
            {
                sourceItemPanel.stacksText.gameObject.SetActive(true);
                sourceItemPanel.stacksText.text = "" + (itemSlot.stacks - splitSize);
            }
        }
    }

    public void SetUI()
    {
        stacksText.text = "" + splitSize;
        itemImage.sprite = itemSlot.item.GiveItemImage();
    }

    public void EmptySlot()
    {
        itemSlot = new ItemSlotInfo(null, 0);
    }
}
