/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles the behavior of the hazards in the game. It allows for hazards to deal damage to the player over time.
*/

using UnityEngine;
using System.Collections; // For Coroutines

public class HazardBehavior : MonoBehaviour
{
    // Amount of health to recover
    [SerializeField]
    int DamageAmount = -10; /// The amount of damage dealt by the hazard
    [SerializeField]
    float TickRate = 1; /// The time interval between damage ticks
    [SerializeField]
    string InvulnerabilityItem = null;  /// Name of item that grants invulnerability to the hazard if any

    private Coroutine damageCoroutine;  /// Coroutine for handling damage over time

    AudioSource hazardAudioSource;  /// Audio source for hazard damage sound

    void Start()
    {
        hazardAudioSource = GetComponent<AudioSource>();    /// Initializing audio source component for hazard sound
    }

    public void StartHazardDamage(PlayerBehavior player)    /// Script for starting hazard damage
    {
        if (damageCoroutine == null && player.health > 0)   /// Check if damage coroutine is not already running and player is alive
        {
            damageCoroutine = StartCoroutine(DamagePlayerOverTime(player)); /// Start the coroutine to damage the player over time
        }
            
    }

    public void StopHazardDamage()  /// Script for stopping hazard damage
    {
        if (damageCoroutine != null)    /// Check if the damage coroutine is running
        {
            StopCoroutine(damageCoroutine); /// Stopping the coroutine
            damageCoroutine = null;
        }
    }

    private IEnumerator DamagePlayerOverTime(PlayerBehavior player) /// Coroutine for damaging the player over time
    {
        while (true)
        {
            if (!string.IsNullOrEmpty(InvulnerabilityItem) && player.Collectibles.Contains(InvulnerabilityItem))    /// Checking if there is an invulnerability item and if the player posesses it
            {
                yield return new WaitForSeconds(TickRate);
                continue;
            }
            else if (player.health <= 0)    /// Checking if the player is dead
            {
                StopHazardDamage();
                yield break;
            }
            else    /// Damage the player as normal
            {
                player.HazardDamage(DamageAmount);  /// Apply damage to the player
                hazardAudioSource.Play();   /// Play the hazard damage sound
                yield return new WaitForSeconds(TickRate);  /// Wait for the specified tick rate before applying damage again
            }
            
        }
    }
}