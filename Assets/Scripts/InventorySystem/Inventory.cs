﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine; 
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]private Slot[] slots;
    [SerializeField]private Button primaryinventoryButton;
    [SerializeField]private GameObject inventoryPanel;
    [SerializeField]private GameObject equipWarning;
    [SerializeField]private GameObject detailPanel;
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemName;
    [SerializeField]private TextMeshProUGUI itemDescription;
    private Collectable currentItem;
    public Collectable equippedItem;
    private void Awake()
    {
        if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.parent.gameObject);}
        else { Destroy(gameObject); }
    }

    public bool CheckIfHaveMage()
    {
        bool isMage = false;
        foreach(Slot s in slots)
        {
            if(s.slotItem == GameController.Instance.cristalFireItem)
            {
               isMage = true;
            }
        }

        return isMage;
    }
    

    public void Open()
    {
        inventoryPanel.SetActive(true);
        UIController.Instance.SetSelectedButton(primaryinventoryButton);
        EnableAllSlots();
        DisableEmptySlots();
        DisableDetailPanel();
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void Close()
    {
        inventoryPanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }

    void UpdateDetailPanel()
    {
        detailPanel.SetActive(true);
        itemImage.sprite = currentItem.itemIcon;
        itemName.text = currentItem.itemName;
        itemDescription.text = currentItem.description;
        equipWarning.SetActive(!currentItem.isKeyItem);
    }

    public void DisableDetailPanel()
    {
        detailPanel.SetActive(false);
        itemImage.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
        equipWarning.SetActive(false);
    }

    void UnequipAllItems()
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem != null)
            {
                s.slotItem.isEquipped = false;
            }
        }
    }

    public bool CheckItens(Collectable keyItem)
    {
        int counter = 0;
        foreach(Slot s in slots)
        {
            if(s.slotItem == keyItem)
            {
                counter ++;
            }
        }
        return counter >= 2;
    }

    public void SubtractValue()
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == equippedItem)
            {
                s.currentValue--;
                if(s.currentValue <= 0)
                {
                    s.ResetSlot();
                    equippedItem = null;
                }
            }
        }

        DisableEmptySlots();
    }

    public void RemoveItem(Collectable item)
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == item)
            {
                s.slotItem = null;
                DisableEmptySlots();
                break;
            }
        }
    }

    public void EquipItem()
    {
        UnequipAllItems();
        currentItem.isEquipped = true;
        equippedItem = currentItem;
        UpdateSlots();
    }

    #region  SLOTS
    public void DisableEmptySlots()
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == null)
            {
                s.gameObject.SetActive(false);
            }
        }
    }

    void DisableAllSlots()
    {
        foreach(Slot s in slots)
        {
            s.gameObject.SetActive(false);
        }
    }

    public void EnableAllSlots()
    {
        foreach(Slot s in slots)
        {
            s.gameObject.SetActive(true);
        }

        UpdateSlots();
    }

    public void EnableKeyItensSlots()
    {
        DisableAllSlots();

        foreach(Slot s in slots)
        {
            if(s.slotItem != null && s.slotItem.isKeyItem)
            {
                s.gameObject.SetActive(true);
            }
        }
    }

    public void EnableCollectablesSlots()
    {
        DisableAllSlots();

        foreach(Slot s in slots)
        {
            if(s.slotItem != null && !s.slotItem.isKeyItem)
            {
                s.gameObject.SetActive(true);
            }
        }
    }
    public void UpdateSlots()
    {
        foreach(Slot s in slots)
        {
            s.LoadSlot();
        }
    }

    public bool isOpen()
    {
        return inventoryPanel.activeSelf;
    }

    #endregion

    #region ITEMS
    public void GetItemInSlot(Collectable item)
    {
        currentItem = item;
        UpdateDetailPanel();
    }

    void AddItemInEmptySlot(Collectable i)
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == null)
            {
                s.currentValue += i.value;
                s.SetSlot(i);
                break;
            }
        } 
    }

    public void AddItem(Collectable item)
    {
        UIController.Instance.TakeItemHUD(item);
        if(item.isKeyItem)
        {
            AddItemInEmptySlot(item);
        }
        else
        {
            bool hasItem = false;
            foreach(Slot s in slots)
            {
                if(s.slotItem == item)
                {
                    hasItem = true;
                    s.currentValue += item.value;
                    s.LoadSlot();
                    break;
                }
            }

            if(!hasItem)
            {
                AddItemInEmptySlot(item);
            }
        }
    }
    #endregion
}
