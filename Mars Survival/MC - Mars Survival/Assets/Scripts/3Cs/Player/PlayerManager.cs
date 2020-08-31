using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public MC_Inventory myInventory;
    public int myInventorySize = 5;
    [SerializeField] public Sprite[] blockTextures;
    [SerializeField] public Sprite[] contour;
    [SerializeField] public Transform[] slots;
    public int mySelectedSlot;

    private void Start()
    {
        myInventory = new MC_Inventory(myInventorySize, slots, blockTextures);
        mySelectedSlot = 0;
    }
}
