/*
* Author: Kwok Ze Yong, Zenon
* Date: 14 June 2025
* Description: This script handles the behavior of the destroyable rocks in the game. Allowing for the player to destroy them when interacted with.
*/

using UnityEngine;

public class DestroyableBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;    /// MeshRenderer to change the material of the destroyable object
    [SerializeField]
    AudioClip destroySound; /// Sound to play when the destroyable object is destroyed
    AudioSource destroyableAudioSource; /// Audio source for object destruction sound

    void Start()
    {       
        destroyableAudioSource = GetComponent<AudioSource>();   /// Initializing audio source component for destruction sound
    }

    public void DestroyObject() /// Script for destroying the object
    {
        AudioSource.PlayClipAtPoint(destroySound, transform.position);  /// Creating an audio source to play the destruction sound at the object's position
        destroyableAudioSource.Play();  /// Playing the destruction sound
        Destroy(gameObject);    /// Destroying the object
    }
}
