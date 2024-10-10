using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public float interactRange = 3.0f;  // The range of interaction
    public LayerMask interactableLayer; // Layer for interactable objects
    public GameObject subtitleText;     // UI Text to display object names
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                subtitleText.SetActive(true);
                subtitleText.GetComponent<UnityEngine.UI.Text>().text = interactable.objectName;

                if (Input.GetKeyDown(KeyCode.E))  // Press E to interact
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            subtitleText.SetActive(false);
        }
    }
}
