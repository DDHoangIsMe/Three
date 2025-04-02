using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    private Canvas canvas_;
    private ItemData[] listItems = new ItemData[SettingData.MAX_ITEM];
    private int[] listCount = new int[SettingData.MAX_ITEM];
    [Range(0, SettingData.MAX_ITEM)]
    private int currentItem_ = 0;

    void Awake()
    {
        if (canvas_ == null)
        {
            canvas_ = GameObject.Find("CanvasInventory").GetComponent<Canvas>();
        }
    }

    public void ChooseItem(int index)
    {
        currentItem_ = index - 1;
    }

    public ItemData GetItem()
    {
        return listItems[currentItem_];
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        string name = item.GetName();
        for (int i = 0; i < SettingData.MAX_ITEM; i++)
        {
            if (listItems[i] != null)
            {
                if (listItems[i].GetName() == name)
                {
                    listCount[i] += amount;
                    UpdateSlot(i);
                    return true;
                }    
            }
        }
        for (int i = 0; i < SettingData.MAX_ITEM; i++)
        {
            if (listItems[i] == null)
            {
                listItems[i] = item;
                listCount[i] = amount;
                UpdateSlot(i);
                return true;
            }
        }
        return false;
    }

    public ItemData UseItem()
    {
        if (listCount[currentItem_] > 0)
        {
            listCount[currentItem_]--;
            ItemData item = listItems[currentItem_];
            if (listCount[currentItem_] == 0)
            {
                listItems[currentItem_] = null;
            }
            UpdateSlot(currentItem_);
            return item;
        }
        return null;
    }

    private void UpdateSlot(int index)
    {
        Transform slot = canvas_.transform.GetChild(0).GetChild(index);
        if (listCount[index] == 0)
        {
            slot.Find("Image").GetComponent<Text>().text = "";
            slot.Find("Quantity").GetComponent<Image>().sprite = null;
            slot.Find("Image").gameObject.SetActive(false);
            slot.Find("Quantity").gameObject.SetActive(false);
        }
        else{
            slot.Find("Image").gameObject.SetActive(true);
            slot.Find("Quantity").gameObject.SetActive(listCount[index] > 1);
            slot.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(
                $"{ObjectData.SPRITE_PATH}{listItems[index].GetName()}"
            );
            slot.Find("Quantity").GetComponent<Text>().text = listCount[index].ToString();
        }
        slot.gameObject.SetActive(listCount[index] > 0);
    }
}
