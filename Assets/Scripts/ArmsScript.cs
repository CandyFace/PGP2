﻿using UnityEngine;
using System.Collections;

// Attach to the arms on the player character
// Lasted Edited: 29-10-2015 10:10

public class ArmsScript : MonoBehaviour {

    public GameObject rightArm;
    public GameObject objectSpaceInHand;
    public Color[] clrArray = new Color[2]; // Debug Coloring
	public bool collectedThisFrame;

    private GameObject objInRightArm;
    private GameObject playerObject;
    private GameObject[] bagSlots;
    private InventorySystem inventory;

    private RaycastHit hit;
    private RaycastHit sndHit;
    private float handSpeed;
    private bool isCarryingItem;

    private RigidbodyConstraints originalConstraints;


	void Start () {
        handSpeed = 1.2f;
        inventory = GameObject.FindGameObjectWithTag("Bagpack").GetComponent<InventorySystem>();
        bagSlots = GameObject.FindGameObjectsWithTag("BagSlot");
        playerObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

	void Update () {
        // Carry object at this point.
        if (inventory.HasBagOpen)
        {
            // If bag is open, make rightArm select in the bag.

			Debug.DrawLine(rightArm.transform.position, rightArm.transform.position + rightArm.transform.forward, Color.cyan);

            if (Physics.Raycast(rightArm.transform.position, rightArm.transform.forward, out hit, 2.2f))
            {
                
                if (hit.collider.gameObject.tag == "Bagpack")
                {
                    rightArm.GetComponent<Renderer>().material.color = clrArray[0];
                }
                else
                {
                    rightArm.GetComponent<Renderer>().material.color = clrArray[1];
                }

//				Debug.Log(hit.collider.gameObject.tag);

                if (Input.GetButtonUp("PS4_X") || Input.GetKeyDown(KeyCode.E))
                {
					Debug.Log(hit.collider.gameObject.name);

                    if (hit.collider.transform.tag == "Bagpack" || hit.collider.transform.tag == "BagSlot")
                    {
                        if (isCarryingItem)
                        {
                            // Drop Item in the slot;
                            inventory.AddItemFromHandToBag(objInRightArm);
                            objInRightArm = null;
                        }
						else if (hit.collider.transform.tag == "BagSlot" && !isCarryingItem)
						{
							hit.collider.gameObject.GetComponent<BagSlot>().HasOpenSpot = true;
							PickUpItem(hit.collider.gameObject.transform.GetChild(0).gameObject);
							isCarryingItem = true;
							hit.collider.gameObject.transform.GetChild(0).transform.parent = null;


//							for (int i = 0; i < 12; i++)
//							{
//								if (hit.collider.gameObject.GetComponent<Pickable>().InvSpot == bagSlots[i].name)
//								{
//									hit.collider.gameObject.GetComponent<Pickable>().IsInInventory = false;
//									bagSlots[i].GetComponent<BagSlot>().HasOpenSpot = true;
//									break;
//								}
//							}
						}

                        
                    }
                    else if (hit.collider.transform.tag != "BagSlot" && hit.collider.transform.tag != "Bagpack" && hit.collider.gameObject.GetComponent<Pickable>().IsInInventory)
                    {
                        if (!isCarryingItem)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                if (hit.collider.gameObject.GetComponent<Pickable>().InvSpot == bagSlots[i].name)
                                {
                                    hit.collider.gameObject.GetComponent<Pickable>().IsInInventory = false;
                                    bagSlots[i].GetComponent<BagSlot>().HasOpenSpot = true;
                                    break;
                                }
                            }
                            PickUpItem(hit.collider.gameObject);
                            isCarryingItem = true;

                        }
                    }
                }
                else
                {
                     //Debug.Log("Empty space in bag.");
                }
            }
            // Movement of Right Arm
            float up, right;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                up = 1.0f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                up = -1.0f;
            }
            else
            {
                up = 0.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                right = -1.0f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                right = 1.0f;
            }
            else
            {
                right = 0.0f;
            }

            Vector3 armPos = rightArm.transform.localPosition;
            armPos.x = Mathf.Clamp(armPos.x, -0.5f, 1.2f);
            armPos.y = Mathf.Clamp(armPos.y, -0.25f, 1.0f);
            armPos += new Vector3(Input.GetAxis("PS4_DPadHorizontal"), Input.GetAxis("PS4_DPadVertical"), 0.0f) * Time.deltaTime * handSpeed;
            armPos += new Vector3(right, up, 0.0f) * Time.deltaTime * handSpeed;
            rightArm.transform.localPosition = armPos;

        }
	}

    public bool IsCarryingItem
    {
        get { return isCarryingItem; }
        set { isCarryingItem = value;}
    }

    public void PickUpItem(GameObject obj)
    {
        originalConstraints = obj.GetComponent<Rigidbody>().constraints;
		isCarryingItem = true;
        objInRightArm = obj;
        obj.GetComponent<Pickable>().IsInInventory = false;
        obj.GetComponent<Pickable>().InvSpot = "Hand";
        obj.transform.parent = objectSpaceInHand.transform;
        obj.transform.position = objectSpaceInHand.transform.position;
        obj.transform.rotation = objectSpaceInHand.transform.rotation;
		obj.layer = 2;
        if (obj.tag == "Coin")
        {
            obj.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        }
        else if (obj.tag == "Room1Tablet")
        {
            obj.transform.localScale = new Vector3(30.0f, 30.0f, 30.0f);
        }
        playerObject.GetComponent<RayCast>().setStoredPickUpItem(obj);
        if (obj.GetComponent<Rigidbody>() != null)
        {
            if (obj.GetComponent<Rigidbody>().useGravity)
            {
                obj.GetComponent<Rigidbody>().useGravity = false;
            }
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (!obj.GetComponent<BoxCollider>().isTrigger)
        {
            obj.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

	public void RemoveItem(GameObject item)
	{
		isCarryingItem = false;
		objInRightArm = null;
		item.transform.parent = null;
		item.layer = 0;
	}

    public void DropItem(GameObject item)
    {
        isCarryingItem = false;
        objInRightArm = null;
        item.transform.parent = null;
        item.transform.rotation = Quaternion.identity;
        item.layer = 0;
        // Checks for Pickable Scripts
        if (item.GetComponent<Pickable>() != null)
        {
            item.GetComponent<Pickable>().CanPickUp = true;
        }
        else
        {
            Debug.Log(item.name + ": needs the Pickable Script!");
        }
        // Checks for Rigidbody
        if (item.GetComponent<Rigidbody>() != null)
        {
            item.GetComponent<Rigidbody>().useGravity = true;
            item.GetComponent<Rigidbody>().constraints = originalConstraints;
        }
        else
        {
            Debug.Log(item.name + ": needs a rigidbody!");
        }

        if (item.GetComponent<BoxCollider>() != null)
        {
            item.GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            Debug.Log(item.name + ": needs a 3D BoxCollider!");
        }
        playerObject.GetComponent<RayCast>().setStoredPickUpItem(null);
    }

}
