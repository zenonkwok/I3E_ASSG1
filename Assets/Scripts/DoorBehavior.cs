/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles the behavior of all doors in the game. Allowing players to open and close them, as well as having locksthat prevent them from opening without a key.
*/

using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool DoorOpen = false;   /// Whether the door is currently open or closed
    [SerializeField]
    string requiredKeyName; /// The name of the key required to open the door if there is one
    [SerializeField]
    float autoCloseDistance = 5f; /// Distance at which the door will automatically close

    AudioSource doorAudioSource;    /// Audio source for door opening/closing sound

    void Start()
    {
        doorAudioSource = GetComponent<AudioSource>();  /// Initializing audio source component for door sound
    }

    public void Interact()  /// Script for interacting with the door
    {
        if (requiredKeyName != null && requiredKeyName != "")   ///Checking if the door requires a key
        {
            PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
            if (player != null && player.Keys.Contains(requiredKeyName))    /// Check if the player has the required key
            {
                ToggleDoor();   /// Toggle the door if the player has the required key
            }
            else
            {
                player.Popup("You need the key: " + requiredKeyName + " to open this door.");   /// Display a message if the player does not have the required key
            }
        }
        else
        {
            ToggleDoor();   /// Toggle the door if no key is required
        }
    }

    public void ToggleDoor()    /// Script for opening or closing the door
    {
        if (DoorOpen == false)
        {
            /// Calculations for rotation of door
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y += 90f;
            transform.eulerAngles = doorRotation;

            DoorOpen = true; /// Set the door state to open
        }
        else if (DoorOpen == true)
        {
            /// Calculations for rotation of door
            Vector3 doorRotation = transform.eulerAngles;
            doorRotation.y -= 90f;
            transform.eulerAngles = doorRotation;
            DoorOpen = false; /// Set the door state to closed
        }
        doorAudioSource.Play(); /// Play the door opening/closing sound

    }

    void Update()
    {
        /// Script for automatically closing the door after the player moves away
        if (DoorOpen)
        {
            PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);   /// Calculating distance between door and player
                if (distance > autoCloseDistance)   /// Checking if the player is far enough
                {
                    ToggleDoor();   /// Closing the door
                }
            }

        }
    }

}
