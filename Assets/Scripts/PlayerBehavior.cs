using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerBehavior : MonoBehaviour
{
    public CollectibleBehavior currentCollectible;
    public DoorBehavior currentDoor;

    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    TextMeshProUGUI pointsText;

    float interactionDistance = 5f;
    public int totalPoints = 0;
    bool canInteract = false;

    public List<string> Keys = new List<string>();

    public List<string> Collectibles = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        pointsText.text = "Points: " + totalPoints.ToString();
    }
    
    void OnInteract()
    {
        if (canInteract)
        {
            if (currentCollectible != null)
            {
                Debug.Log("Interacting with collectible");
                // Checking if collectible is a key
                if (currentCollectible.collectibleType == "Key")
                {
                    Keys.Add(currentCollectible.name);
                    Debug.Log("Collected key: " + currentCollectible.name);
                }


                currentCollectible.Collect(this);
                totalPoints += currentCollectible.value; // Assuming CollectibleBehavior has a value property
                pointsText.text = "Points: " + totalPoints.ToString();
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
                currentCollectible = null;
            }
            canInteract = true;
            currentCollectible = hitInfo.collider.gameObject.GetComponent<CollectibleBehavior>();


            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehavior>();
                if (currentCollectible != null)
                {
                    currentCollectible.Highlight();
                }
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


