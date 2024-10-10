using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName;  // Show this under the cursor
    public Sprite silhouetteSprite;  

    public void Interact()
    {
        // Attempt to add the object to the inventory
        bool wasAdded = InventoryManager.instance.AddToInventory(this);

        if (wasAdded)
        {
            Destroy(gameObject);  // Remove the object from the scene after adding to the inventory
        }
        else
        {
            // Display message when 5 items in inventory
            Debug.Log("Inventory is full. Can't add " + objectName);
        }
    }


}
