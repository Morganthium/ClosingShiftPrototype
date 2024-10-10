using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [System.Serializable]
    public class InventorySlot
    {
        public Sprite itemSprite;
        public GameObject itemObject;
    }

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public Transform inventoryUI;  // Reference to the UI grid to display items
    public GameObject inventorySlotPrefab;  // Prefab for the UI element representing an inventory slot
    public int maxInventorySize = 5;  // Maximum number of inventory slots

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize inventory slots
        for (int i = 0; i < maxInventorySize; i++)
        {
            // Create an empty inventory slot
            InventorySlot newSlot = new InventorySlot();
            inventorySlots.Add(newSlot);

            // Instantiate the UI element for this slot
            Instantiate(inventorySlotPrefab, inventoryUI);
        }
    }

    public bool AddToInventory(InteractableObject item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].itemObject == null)
            {
                // Add the item to the first empty slot
                inventorySlots[i].itemSprite = item.silhouetteSprite;
                inventorySlots[i].itemObject = item.gameObject;

                // Update the UI for this slot
                UpdateInventoryUI();

                // Hide the item in the game world
                item.gameObject.SetActive(false);

                return true;  // Successfully added
            }
        }

        Debug.Log("Inventory is full");
        return false;  // Inventory was full, item not added
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.Log("Invalid inventory slot index");
            return;
        }

        GameObject item = inventorySlots[slotIndex].itemObject;
        if (item != null)
        {
            // Get the main camera's transform
            Transform cameraTransform = Camera.main.transform;

            // Position the item in front of the camera
            item.transform.position = cameraTransform.position + cameraTransform.forward * 2.0f;

            // Reset the item's rotation relative to the camera
            item.transform.rotation = Quaternion.identity;

            // Re-enable the item in the game world
            item.SetActive(true);

            // Clear the inventory slot
            inventorySlots[slotIndex].itemObject = null;
            inventorySlots[slotIndex].itemSprite = null;

            // Update the UI for this slot
            UpdateInventoryUI();
        }
    }


    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryUI.childCount; i++)
        {
            Image slotImage = inventoryUI.GetChild(i).GetComponent<Image>();

            if (i < inventorySlots.Count && inventorySlots[i].itemSprite != null)
            {
                slotImage.sprite = inventorySlots[i].itemSprite;
                slotImage.color = Color.white;  // Ensure the slot is visible
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0);  // Make the slot invisible
            }
        }
    }

    private void Update()
    {
        // Check for key presses to use items
        if (Input.GetKeyDown(KeyCode.Alpha1)) { UseInventoryItem(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { UseInventoryItem(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { UseInventoryItem(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { UseInventoryItem(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { UseInventoryItem(4); }
    }

    public void UseInventoryItem(int slotIndex)
    {
        RemoveFromInventory(slotIndex);
    }
}
