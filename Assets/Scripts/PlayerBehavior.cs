using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CoinBehavior currentCoin;
    public DoorBehavior currentDoor;

    [SerializeField]
    GameObject projectile;

    [SerializeField]

    //float interactionDistance = 5f;

    bool canInteract = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    void OnInteract()
    {
        if (canInteract)
        {
            if (currentCoin != null)
            {
                Debug.Log("Interacting with coin");
                currentCoin.Collect(this);
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
            Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
            canInteract = true;
            currentCoin = other.gameObject.GetComponent<CoinBehavior>();
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.gameObject.GetComponent<DoorBehavior>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            currentCoin = null;
            canInteract = false;
        }
        
    }

    /*void Update()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (currentCoin != null)
            {
                currentCoin.Unhighlight();
            }
            canInteract = true;
            currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehavior>();
            currentCoin.Highlight();


            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                currentCoin = hitInfo.collider.GetComponent<CoinBehavior>();
                canInteract = true;
            }
            else if (hitInfo.collider.CompareTag("Door"))
            {
                currentDoor = hitInfo.collider.GetComponent<DoorBehavior>();
                canInteract = true;
            }
            else
            {
                currentCoin = null;
                currentDoor = null;
                canInteract = false;
            }
        }
        else
        {
            currentCoin = null;
            currentDoor = null;
            canInteract = false;
        }
    }*/
}


