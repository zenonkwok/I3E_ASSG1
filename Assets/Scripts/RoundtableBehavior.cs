/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles the behavior of the roundtable in the final part of the game. Allowing the player to place items on it and summon the final collectible.
*/

using UnityEngine;
using System.Collections.Generic;   // For Lists

public class RoundtableBehavior : MonoBehaviour
{
    private List<string> requiredItems = new List<string>   /// List of required items to place on the roundtable
    {
        "job_Application",
        "magic_Ring",
        "crystal",
        "sculpture",
    };
    public bool items_Placed = false;   /// Whether the items have been placed on the roundtable
    public bool item_Summoned = false;  /// Whether the summonable object has been summoned
    [SerializeField]
    Transform items;    /// Transform for the items to be placed on the roundtable
    [SerializeField]
    GameObject summonableObject;    /// The object that will be summoned when the items are placed on the roundtable

    private AudioSource roundtableAudioSource;  /// Audio source for roundtable sounds
    [SerializeField]
    private AudioClip placeItemsSound;  /// Sound to play when items are placed on the roundtable
    [SerializeField]
    private AudioClip summonSound;  /// Sound to play when the summonable object is summoned

    void Start()
    {
        /// Ensure that the items and summonable object are not active at the start
        items.gameObject.SetActive(false);
        summonableObject.SetActive(false);

        roundtableAudioSource = GetComponent<AudioSource>();   /// Initializing audio source component for roundtable sounds
    }

    public void PlaceItems(PlayerBehavior player)   /// Script for placing items on the roundtable
    {
        foreach (string item in requiredItems)  /// Check if the player has all required items
        {
            if (!player.Collectibles.Contains(item))
            {
                /// If the player does not have all required items, display a message and return
                player.Popup("You need to collect all required items before placing them on the roundtable. " + string.Join(", ", requiredItems));  
                return;
            }
        }

        player.Popup("All items have been placed on the roundtable.");  /// Display a message indicating that all items have been placed

        /// Placing the items on the roundtable
        items_Placed = true;
        items.gameObject.SetActive(true);
        roundtableAudioSource.PlayOneShot(placeItemsSound); /// Play the sound for placing items on the roundtable
    }

    public void Summon()    /// Script for summoning the object when items are placed on the roundtable
    {
        if (item_Summoned)  /// Check if the summonable object has already been summoned
        {
            return;
        }

        /// Summoning the object
        summonableObject.SetActive(true);
        item_Summoned = true;
        roundtableAudioSource.PlayOneShot(summonSound); /// Play the sound for summoning the object
    }
}
