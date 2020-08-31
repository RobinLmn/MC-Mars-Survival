using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private CharacterController myCharController;
    [SerializeField] private float mySpeed;
    [SerializeField] private float myJumpHeight = 3f;

    [SerializeField] private Vector3 myVelocity;
    private const float gravity = -9.81f;

    [SerializeField] private Transform myGroundCheck;
    [SerializeField] private float myGroundDistance;
    [SerializeField] private LayerMask myGroundMask;
    private bool isGrounded;
    private bool isFlying = false;

    [SerializeField] private LayerMask myHitLayer;
    [SerializeField] private Transform myCameraTransform;
    [SerializeField] public MCC_WorldObject myWorldObj;

    [SerializeField] private PlayerManager playerManager;

    void Start()
    {
        myCharController = GetComponent<CharacterController>();
        UpdateSelectedSlotSprite(playerManager.contour[1]);
    }

    void Update()
    {
        Movement();
        Hit();
        InventorySelection();
    }

    private void InventorySelection()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            UpdateSelectedSlotSprite(playerManager.contour[0]);
            playerManager.mySelectedSlot++;
            if (playerManager.mySelectedSlot >= playerManager.myInventorySize)
            {
                playerManager.mySelectedSlot = 0;
            }
            UpdateSelectedSlotSprite(playerManager.contour[1]);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            UpdateSelectedSlotSprite(playerManager.contour[0]);
            playerManager.mySelectedSlot--;
            if (playerManager.mySelectedSlot < 0)
            {
                playerManager.mySelectedSlot = playerManager.myInventorySize -1;
            }
            UpdateSelectedSlotSprite(playerManager.contour[1]);
        }

        for (int i = 49; i < 54; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                UpdateSelectedSlotSprite(playerManager.contour[0]);
                playerManager.mySelectedSlot = i - 49;
                UpdateSelectedSlotSprite(playerManager.contour[1]);
            }
        }       
    }

    private void UpdateSelectedSlotSprite(Sprite sprite)
    {
       playerManager.slots[playerManager.mySelectedSlot].GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    private void Hit()
    {
        if (Input.GetButtonDown("Fire"))
        {
            RaycastHit hit;

            if (Physics.Raycast(myCameraTransform.position, myCameraTransform.forward, out hit, Mathf.Infinity, myHitLayer))
            {
                Item selectedItem = playerManager.myInventory.Get(playerManager.mySelectedSlot);

                if (selectedItem == Item.Hand)
                {
                    // Dig
                    Vector3 pos = hit.point + myCameraTransform.forward.normalized * 0.1f;
                    float voxelSize = myWorldObj.myWorldData.MyVoxelSize;
                    Vector3Int blockPos = new Vector3Int((int)(pos.x / voxelSize), (int)(pos.y / voxelSize), (int)(pos.z / voxelSize));
                    Debug.Log(blockPos);
                    MCS_Block block = myWorldObj.myWorldData.GetBlock(blockPos);

                    playerManager.myInventory.Add(block);
                    block.Dig();
                }
                else
                {
                    // Dig
                    Vector3 pos = hit.point + myCameraTransform.forward.normalized * -0.1f;
                    float voxelSize = myWorldObj.myWorldData.MyVoxelSize;
                    Vector3Int blockPos = new Vector3Int((int)(pos.x / voxelSize), (int)(pos.y / voxelSize), (int)(pos.z / voxelSize));
                    MCS_Block block = myWorldObj.myWorldData.GetBlock(blockPos);

                    playerManager.myInventory.Remove(playerManager.mySelectedSlot);

                    BlockType type = BlockType.Air;

                    switch (selectedItem)
                    {
                        case Item.Dirt:
                            type = BlockType.Dirt;
                            break;
                        case Item.Stone:
                            type = BlockType.Stone;
                            break;
                    }

                    block.Place(type);
                }
            }
        }
    }

    private void Movement()
    {
        isGrounded = Physics.CheckSphere(myGroundCheck.position, myGroundDistance, myGroundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        myCharController.Move(move * mySpeed * Time.deltaTime);

        if (!isFlying)
        {
            if (isGrounded && myVelocity.y < 0)
            {
                myVelocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                myVelocity.y = Mathf.Sqrt(myJumpHeight * -2f * gravity);
            }

            myVelocity.y += gravity * Time.deltaTime;

            myCharController.Move(myVelocity * Time.deltaTime);

            if (Input.GetKey(KeyCode.G))
            {
                isFlying = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.G))
            {
                isFlying = false;
            }

            if (Input.GetButton("Jump"))
            {
                myVelocity.y = Mathf.Sqrt(myJumpHeight * -2f * gravity);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                myVelocity.y = -Mathf.Sqrt(myJumpHeight * -2f * gravity);
            }
            else
            {
                myVelocity.y = 0;
            }

            myCharController.Move(myVelocity * Time.deltaTime);
        }
    }
}