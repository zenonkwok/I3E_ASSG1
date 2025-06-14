/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles all behaviors of the player in the game. All interactions within the game are handled here as well as keeping track of the player's collectibles and health.
*/

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
        // Initializing UI elements
        pointsText.text = "Points: " + totalPoints.ToString();
        healthText.text = "Health: " + health.ToString();
        totalCollectiblesText.text = collectiblesCollected.ToString() + "/" + totalCollectibles.ToString();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        popupBackground.gameObject.SetActive(true);
        endingScreen.gameObject.SetActive(false);
        endingText.gameObject.SetActive(false);
    }

    public void Popup(string uiText)    /// Script for displaying popup messages
    {
        popupText.text = uiText;    /// Setting the text of the popup

        /// Activating the popup text and background
        popupText.gameObject.SetActive(true);
        popupBackground.gameObject.SetActive(true);
    }

    void OnInteract()   /// Script for handling player interaction with objects
    {
        if (canInteract)    /// Check if theres an object to interact with
        {
            if (currentCollectible != null) /// Check if the focused object is a collectible
            {
                if (currentCollectible.collectibleType == "Key")    /// Checking if collectible is a key
                {
                    Keys.Add(currentCollectible.name);  /// Adding the key to the player's keys list
                }
                else if (currentCollectible.collectibleType == "Collectible")   /// Checking if collectible is a regular collectible
                {
                    Collectibles.Add(currentCollectible.name);  /// Adding the collectible to the player's collectibles list
                }

                currentCollectible.Collect(this);   /// Collecting the collectible
                totalPoints += currentCollectible.value; /// Adding the collectible's value to the player's total points
                collectiblesCollected++;    /// Incrementing the number of collectibles collected
                pointsText.text = "Points: " + totalPoints.ToString();  /// Updating the points text
                totalCollectiblesText.text = collectiblesCollected.ToString() + "/" + totalCollectibles.ToString(); /// Updating the total collectibles text

                if (collectiblesCollected >= totalCollectibles) /// Check if all collectibles have been collected
                {
                    CompleteGame(); /// Call the script to complete the game
                }
            }
            else if (currentDoor != null)   /// Check if the focused object is a door
            {
                currentDoor.Interact(); /// Interact with the door
            }

            else if (currentCannon != null) /// Check if the focused object is a cannon
            {
                currentCannon.Fire();   /// Fire the cannon
            }
            else if (currentDestroyable != null)    /// Check if the focused object is a destroyable object
            {
                currentDestroyable.DestroyObject(); /// Destroy the object
            }
            else if (currentRoundtable != null) /// Check if the focused object is a roundtable
            {
                if (currentRoundtable.items_Placed) /// Checking if the items have already been placed on the roundtable
                {
                    currentRoundtable.Summon(); /// Summon the object if items have been placed
                }
                else
                {
                    currentRoundtable.PlaceItems(this); /// Place items on the roundtable
                }

            }
        }

    }

    public void HazardDamage(int DamageAmount)  /// Script for applying damage to the player from hazards
    {
        health += DamageAmount; /// Apply the damage amount to the player's health
        healthText.text = "Health: " + health.ToString();   /// Update the health text

        durationtTimer = 0; /// Reset the timer for the damage overlay
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);    /// Set the overlay color to be fully opaque

        if (health <= 0)    /// Checking if the player has died
        {
            health = 0; /// Ensuring that health does not go below 0
            Respawn();  /// Call the respawn script
        }
    }

    void Respawn()  /// Script for respawning the player after death
    {
        health = maxHealth; /// Reset health to maximum
        healthText.text = "Health: " + health.ToString();   /// Update the health text

        var controller = GetComponent<CharacterController>();   /// Section for ensuring the player respawns at the respawn point
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = respawnPoint.position;
            controller.enabled = true;
        }
        else
        {
            transform.position = respawnPoint.position; /// Directly set the respawn position if theres no need for CharacterController
        }

        deathCount++;   /// Increment the death count
    }

    void OnTriggerEnter(Collider other) /// Script for detecting when the player enters a hazard zone
    {
        if (other.CompareTag("Hazard")) /// Checking that it is a hazard
        {
            HazardBehavior hazard = other.GetComponent<HazardBehavior>();
            if (hazard != null && health > 0)   /// Checking if there is a hazard and if the player is alive
            {
                currentHazard = hazard; /// Set the current hazard
                hazard.StartHazardDamage(this); /// Start the hazard damage coroutine
            }
        }
    }

    void OnTriggerExit(Collider other)  /// Script for detecting when the player exits a hazard zone
    {
        if (other.CompareTag("Hazard")) /// Checking that it is a hazard
        {
            HazardBehavior hazard = other.GetComponent<HazardBehavior>();
            if (hazard != null) /// Checking if there is a hazard
            {
                currentHazard = null;   /// Resetting the current hazard
                hazard.StopHazardDamage();  /// Stop the hazard damage coroutine
            }
        }
    }

    void CompleteGame() /// Script for completing the game
    {
        /// Setting ending UI elements to active
        endingScreen.gameObject.SetActive(true);
        endingText.gameObject.SetActive(true);
        endingText.text = "Congratulations! You have completed the game.\n \n" + "Total Deaths: " + deathCount.ToString();  /// Displaying the ending message with total deaths

        /// Deactivating popup text for safety
        popupBackground.gameObject.SetActive(false);
        popupText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Raycasting Script
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);    /// Raycast visualisation in editor

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance)) /// Checking if the raycast has hit an object within interaction distance
        {
            if (currentCollectible != null) /// Checking if there is a collectible currently highlighted
            {
                currentCollectible.Unhighlight();   /// Unhighlight the previous collectible
                currentCollectible = null;
            }

            if (hitInfo.collider.gameObject.CompareTag("Collectible"))  /// Checking if the object is a collectible
            {
                currentCollectible = hitInfo.collider.GetComponent<CollectibleBehavior>();
                if (currentCollectible != null)
                {
                    currentCollectible.Highlight(); /// Highlight the collectible
                }
                canInteract = true; /// Set canInteract to true for the collectible
                Popup("Press E to collect: " + currentCollectible.name);    /// Displaying the popup message for the collectible

            }
            else if (hitInfo.collider.CompareTag("Door"))   /// Checking if the object is a door
            {
                currentDoor = hitInfo.collider.GetComponent<DoorBehavior>();
                canInteract = true; /// Set canInteract to true for the door
            }
            else if (hitInfo.collider.CompareTag("Cannon")) /// Checking if the object is a cannon
            {
                currentCannon = hitInfo.collider.GetComponent<CannonBehavior>();
                canInteract = true; /// Set canInteract to true for the cannon
            }
            else if (hitInfo.collider.CompareTag("Destroyable"))    /// Checking if the object is destroyable
            {
                currentDestroyable = hitInfo.collider.GetComponent<DestroyableBehavior>();
                canInteract = true; /// Set canInteract to true for the destroyable object
                Popup("Press E to destroy this object.");   /// Displaying the popup message for the destroyable object
            }
            else if (hitInfo.collider.CompareTag("Roundtable")) /// Checking if the object is a roundtable
            {
                currentRoundtable = hitInfo.collider.GetComponent<RoundtableBehavior>();
                canInteract = true; /// Set canInteract to true for the roundtable
            }
            else if (hitInfo.collider.CompareTag("Warning") && !Collectibles.Contains("magic_Ring"))    /// Checking if it is a warning area and if the player has the required item
            {
                Popup("Warning: Toxic Waste Ahead. You need a magic_Ring to proceed safely.");  /// Displaying the warning message
            }
            else
            {
                /// Resetting all interactable objects if the raycast hits something else
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
            /// Resetting all interactable objects if the raycast does not hit anything
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
        if (overlay.color.a > 0)    /// If the overlay is visible
        {
            durationtTimer += Time.deltaTime;   /// Initializing the timer for the overlay fade out
            if (durationtTimer > duration)  /// Checking if the duration has passed
            {
                float tempAlpha = overlay.color.a;  /// Using deltaTime timer to fade out the overlay smoothly
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);    /// Fading out the overlay
            }
        }
    }

}


