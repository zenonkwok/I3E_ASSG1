/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script is for the wall in the third room of the game. Acting as a trigger to despawn the walls and projectile after they collide.
*/

using UnityEngine;
using System.Collections;   // For Coroutines

public class Despawn : MonoBehaviour
{
    void OnTriggerEnter(Collider other) /// Detection for projectiles hitting the wall
    {
        if (other.gameObject.CompareTag("Projectile"))  /// Check if the object that triggered the collider is a projectile
        {
            StartCoroutine(DestroyWallsAfterDelay(5f, other.gameObject));   /// Start a countdown to despawn the walls and projectile
        }
    }

    IEnumerator DestroyWallsAfterDelay(float delay, GameObject projectile)  /// Coroutine countdown script
    {
        yield return new WaitForSeconds(delay); /// Waiting for the specified delay before executing the next line of code
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Breakable Wall");   /// Finding all game objects with the tag "Breakable Wall"
        foreach (GameObject wall in walls)  /// Looping through each wall found
        {
            Destroy(wall);  /// Destroying each wall found with the tag "Breakable Wall"
        }
        Destroy(projectile);    /// Destroying the projectile that hit the wall
    }
}
