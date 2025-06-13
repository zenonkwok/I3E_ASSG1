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
    public RoundtableBehavior currentRoundtable;


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
    TextMeshProUGUI totalCollectiblesText; // For the UI text that displays total collectibles
    [SerializeField]
    TextMeshProUGUI endingText; // For the UI text that displays the ending message
    [SerializeField]
    UnityEngine.UI.Image endingScreen; // For the UI background on the ending screen
    [SerializeField]
    UnityEngine.UI.Image overlay;
    [SerializeField]
    UnityEngine.UI.Image popupBackground;

    // Additional fields for player's behavior
    float interactionDistance = 5f; // Distance within which the player can interact with objects via raycast
    public int totalPoints = 0; // Total points collected by the player
    public int health = 100;  // Player's health
    public int maxHealth = 100; // Maximum health of the player
    public int deathCount = 0; // Number of times the player has died
    public int totalCollectibles = 9; // Total number of collectibles in the game
    public int collectiblesCollected = 0; // Number of collectibles collected by the player
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
        totalCollectiblesText.text = collectiblesCollected.ToString() + "/" + totalCollectibles.ToString();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        popupBackground.gameObject.SetActive(true);
        endingScreen.gameObject.SetActive(false);
        endingText.gameObject.SetActive(false);
    }

    public void Popup(string uiText)
    {
        popupText.text = uiText;
        popupText.gameObject.SetActive(true);
        popupBackground.gameObject.SetActive(true);
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
                collectiblesCollected++;
                pointsText.text = "Points: " + totalPoints.ToString();
                totalCollectiblesText.text = collectiblesCollected.ToString() + "/" + totalCollectibles.ToString();

                if (collectiblesCollected >= totalCollectibles)
                {
                    CompleteGame();
                }
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
            else if (currentRoundtable != null)
            {
                Debug.Log("Interacting with roundtable");
                if (currentRoundtable.items_Placed)
                {
                    currentRoundtable.Summon();
                }
                else
                {
                    currentRoundtable.PlaceItems(this);
                }

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
        deathCount++;
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

    void CompleteGame()
    {
        endingScreen.gameObject.SetActive(true);
        endingText.gameObject.SetActive(true);
        endingText.text = "Congratulations! You have completed the game.\n" + "Total Deaths: " + deathCount.ToString();
        popupBackground.gameObject.SetActive(false);
        popupText.gameObject.SetActive(false);
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
                Popup("Press E to collect: " + currentCollectible.name);

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
                Popup("Press E to destroy this object.");
            }
            else if (hitInfo.collider.CompareTag("Roundtable"))
            {
                currentRoundtable = hitInfo.collider.GetComponent<RoundtableBehavior>();
                canInteract = true;
                //Popup("Press E to place down items.");
            }
            else if (hitInfo.collider.CompareTag("Warning") && !Collectibles.Contains("magic_Ring"))
            {
                Popup("Warning: Toxic Waste Ahead. You need a magic_Ring to proceed safely.");
            }
            else
            {
                currentCollectible = null;
                currentDoor = null;
                currentCannon = null;
                currentDestroyable = null;
                currentRoundtable = null;
                canInteract = false;
                popupBackground.gameObject.SetActive(false);
                popupText.gameObject.SetActive(false);
            }
        }
        else
        {
            currentCollectible = null;
            currentDoor = null;
            currentCannon = null;
            currentDestroyable = null;
            currentRoundtable = null;
            canInteract = false;
            popupBackground.gameObject.SetActive(false);
            popupText.gameObject.SetActive(false);
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
                popupText.gameObject.SetActive(false);
            }
        }
    }

}


