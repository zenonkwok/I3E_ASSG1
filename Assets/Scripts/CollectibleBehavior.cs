/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles the behavior of collectibles in the game. Its primary function is to allow players to collect and store collectibles.
*/

using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;    /// MeshRenderer to change the material of the collectible
    [SerializeField]
    Material highlightMat;  /// Material when the collectible is highlighted
    [SerializeField]
    Material originalMat;   /// Original material of the collectible
    [SerializeField]
    AudioClip collectSound; /// Sound to play when the collectible is collected and destroyed
    public string collectibleType = "Key";  /// Type category of the collectible
    public int value = 10;  /// Point value of the collectible

    AudioSource CollectibleAudioSource; /// Audio source for collectible collection sound

    void Start()
    {
        CollectibleAudioSource = GetComponent<AudioSource>();   /// Initializing audio source component

        myMeshRenderer = GetComponent<MeshRenderer>();  /// Getting the MeshRenderer component
        
        originalMat = myMeshRenderer.material;  /// Storing the original material of the collectible
    }
    public void Highlight() /// Script for highlighting the collectible
    {
        myMeshRenderer.material = highlightMat; // Change the material to highlightMat when the collectible is highlighted
    }
    public void Unhighlight()   /// Script for unhighlighting the collectible
    {
        myMeshRenderer.material = originalMat;  /// Change the material back to originalMat when the collectible is unhighlighted
    }

    public void Collect(PlayerBehavior player)  /// Script for collecting the collectible
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);  /// Creating an audio source to play the collection sound at the collectible's position
        CollectibleAudioSource.Play();  /// Playing the collectible collection sound
        Destroy(gameObject); /// Destroying the collectible after collection
    }
    
}
