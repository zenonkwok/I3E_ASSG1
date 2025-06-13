using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections.Generic; // For Lists
using System.Collections; // For Coroutines

public class PlayerBehavior : MonoBehaviour
{

    // Assignments for interactable objects and corresponding behavior scripts
    public CollectibleBehavior currentCollectible;
    public DoorBehavior currentDoor;
    public CannonBehavior currentCannon;
    public HazardBehavior currentHazard;
    public DestroyableBehavior currentDestroyable;


    // Serialized fields for Unity Editor
    [SerializeField]
    Transform spawnPoint; // The GameObject from which raycasting comes from
    [SerializeField]
    Transform respawnPoint; // The GameObject where the player will respawn after death
    [SerializeField]
    TextMeshProUGUI pointsText; // For the UI text that displays points 
    [SerializeField]
    TextMeshProUGUI healthText; // For the UI text that displays health
    [SerializeField]
    TextMeshProUGUI popupText; // For the UI text that displays popups
    [SerializeField]
    UnityEngine.UI.Image overlay;

    // Additional fields for player's behavior
    float interactionDistance = 5f; // Distance within which the player can interact with objects via raycast
    public int totalPoints = 0; // Total points collected by the player
    public int health = 100;  // Player's health
    public int maxHealth = 100; // Maximum health of the player
    bool canInteract = false;   // Whether the focused object can be interacted with

    // Variables for damage overlay
    public float duration;  
    public float fadeSpeed;
    private float durationtTimer;



    // Lists to store keys and collectibles
    public List<string> Keys = new List<string>();
    public List<string> Collectibles = new List<string>();

    void Start()
    {
        pointsText.text = "Points: " + totalPoints.ToString();
        healthText.text = "Health: " + health.ToString();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    public void Popup(string uiText)
    {
        popupText.text = uiText;
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
                else if (currentCollectible.collectibleType == "Collectible")
                {
                    Collectibles.Add(currentCollectible.name);
                    Debug.Log("Collected collectible: " + currentCollectible.name);
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

            else if (currentCannon != null)
            {
                Debug.Log("Firing Cannon");
                currentCannon.Fire();
            }
            else if (currentDestroyable != null)
            {
                Debug.Log("Destroying object");
                currentDestroyable.DestroyObject();
            }
        }

    }

    public void HazardDamage(int DamageAmount)
    {
        health += DamageAmount;
        healthText.text = "Health: " + health.ToString();
        durationtTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        if (health <= 0)
        {
            Debug.Log("Player has died");
            health = 0; // Ensure that health does not go below 0
            Respawn();
        }
    }

    void Respawn()
    {
        health = maxHealth; // Reset health to maximum
        healthText.text = "Health: " + health.ToString();
        Debug.Log("Respawning player at respawn point");
        transform.position = respawnPoint.position; // Reset player position to spawn point
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            HazardBehavior hazard = other.GetComponent<HazardBehavior>();
            if (hazard != null && health > 0)
            {
                Debug.Log("Player has entered hazard zone");
                currentHazard = hazard;
                hazard.StartHazardDamage(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            HazardBehavior hazard = other.GetComponent<HazardBehavior>();
            if (hazard != null)
            {
                Debug.Log("Player has exited hazard zone");
                currentHazard = null;
                hazard.StopHazardDamage();
            }
        }
    }


    void Update()
    {
        // Raycasting Script
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (currentCollectible != null)
            {
                currentCollectible.Unhighlight();
                currentCollectible = null;
            }

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
            else if (hitInfo.collider.CompareTag("Cannon"))
            {
                currentCannon = hitInfo.collider.GetComponent<CannonBehavior>();
                canInteract = true;
            }
            else if (hitInfo.collider.CompareTag("Destroyable"))
            {
                currentDestroyable = hitInfo.collider.GetComponent<DestroyableBehavior>();
                canInteract = true;
            }
            else
            {
                currentCollectible = null;
                currentDoor = null;
                currentCannon = null;
                currentDestroyable = null;
                canInteract = false;
            }
        }
        else
        {
            currentCollectible = null;
            currentDoor = null;
            currentCannon = null;
            currentDestroyable = null;
            canInteract = false;
        }


        // Damage Overlay Fade out Script
        if (overlay.color.a > 0)    
        {
            durationtTimer += Time.deltaTime;
            if (durationtTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

}


