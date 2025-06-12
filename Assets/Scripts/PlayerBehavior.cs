using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CollectibleBehavior currentCollectible;
    public DoorBehavior currentDoor;

    [SerializeField]
    Transform spawnPoint;
    [SerializeField]

    float interactionDistance = 5f;

    bool canInteract = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    void OnInteract()
    {
        if (canInteract)
        {
            if (currentCollectible != null)
            {
                Debug.Log("Interacting with collectible");
                currentCollectible.Collect(this);
            }
            else if (currentDoor != null)
            {
                Debug.Log("Interacting with door");
                currentDoor.Interact();
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            canInteract = true;
            currentCollectible = other.gameObject.GetComponent<CollectibleBehavior>();
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
            canInteract = true;
            currentDoor = other.gameObject.GetComponent<DoorBehavior>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            currentCollectible = null;
            canInteract = false;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            currentDoor = null;
            canInteract = false;
        }
        
    }

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (currentCollectible != null)
            {
                currentCollectible.Unhighlight();
            }
            canInteract = true;
            currentCollectible = hitInfo.collider.gameObject.GetComponent<CollectibleBehavior>();


            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehavior>();
                canInteract = true;
            }
            else if (hitInfo.collider.CompareTag("Door"))
            {
                currentDoor = hitInfo.collider.GetComponent<DoorBehavior>();
                canInteract = true;
            }
            else
            {
                currentCollectible = null;
                currentDoor = null;
                canInteract = false;
            }
        }
        else
        {
            currentCollectible = null;
            currentDoor = null;
            canInteract = false;
        }
    }
}


