using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MC_Inventory
{
    public Sprite[] mySprites;
    public Transform[] mySlotsGO;
    private MC_ItemSlot[] myInventory;
    private int myInventorySize;

    public MC_Inventory(int size, Transform[] slots, Sprite[] sprites)
    {
        myInventory = new MC_ItemSlot[size];
        myInventorySize = size;
        mySlotsGO = slots;
        mySprites = sprites;

        for (int slot = 0; slot < size; slot++)
        {
            myInventory[slot] = new MC_ItemSlot(Item.Hand, 0, slot);
        }
    }

    public bool Add(MC_Item aItem)
    {
        if (aItem.myItem == Item.Hand)
        {
            return false;
        }

        bool foundEmptySlot = false;
        int emptySlotIndex = -1;

        for (int slots = 0; slots < myInventorySize; slots++ )
        {
            if (myInventory[slots].myItem == aItem.myItem && myInventory[slots].Add())
            {
                mySlotsGO[slots].GetChild(2).GetComponent<TextMeshProUGUI>().text = myInventory[slots].myCount.ToString();
                return true;
            }
            else if (!foundEmptySlot && myInventory[slots].myItem == Item.Hand)
            {
                emptySlotIndex = slots;
                foundEmptySlot = true;
            }
        }

        if (emptySlotIndex != -1)
        {
            myInventory[emptySlotIndex].AddFirst(aItem.myItem);
            mySlotsGO[emptySlotIndex].GetChild(1).GetComponent<Image>().sprite = mySprites[(int)aItem.myItem];
            mySlotsGO[emptySlotIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text = myInventory[emptySlotIndex].myCount.ToString();
            return true;
        }

        return false;
    }

    public bool Remove(int aIndex)
    {
        bool sucess = myInventory[aIndex].Remove();
        mySlotsGO[aIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text = myInventory[aIndex].myCount.ToString();

        if (myInventory[aIndex].myCount == 0)
        {
            mySlotsGO[aIndex].GetChild(1).GetComponent<Image>().sprite = mySprites[(int)Item.Hand];
        }

        return sucess;
    }

    public Item Get(int aIndex)
    {
        return myInventory[aIndex].myItem;
    }
}

public class MC_ItemSlot
{
    public int myIndex;
    public Item myItem;
    public int myCount;
    private static int myMaxSlotSize = 64;

    public MC_ItemSlot(Item aItem, int aCount, int aIndex)
    {
        myItem = aItem;
        myCount = aCount;
        myIndex = aIndex;
    }

    public bool Remove()
    {
        if (myCount > 0)
        {
            myCount--;
            if (myCount == 0)
            {
                myItem = Item.Hand;
            }

            return true;
        }
        return false;
    }

    public bool Add()
    {
        if (myCount < myMaxSlotSize)
        {
            myCount++;
            return true;
        }
        return false;
    }

    public void AddFirst(Item aItem)
    {
        myItem = aItem;
        myCount = 1;
    }
}

public enum Item
{
    // TOOLS
    
    Hand = 3,

    // BLOCS

    Dirt = 0,
    Stone = 1,

    // OTHER
}
