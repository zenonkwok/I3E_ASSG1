using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    Material highlightMat;
    [SerializeField]
    Material originalMat;
    [SerializeField]
    AudioClip collectSound;
    public string collectibleType = "Key";
    public int value = 10;

    AudioSource CollectibleAudioSource;

    void Start()
    {
        CollectibleAudioSource = GetComponent<AudioSource>();

        myMeshRenderer = GetComponent<MeshRenderer>();
        
        originalMat = myMeshRenderer.material;
    }
    public void Highlight()
    {
        // Change the material to highlightMat when the collectible is highlighted
        myMeshRenderer.material = highlightMat;
    }
    public void Unhighlight()
    {
        myMeshRenderer.material = originalMat;
    }

    public void Collect(PlayerBehavior player)
    {
        // Add the value of the collectible to the player's score or inventory
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        CollectibleAudioSource.Play();
        Debug.Log("Collected " + value + " points!");
        Destroy(gameObject); // Destroy the collectible after collection
    }
    
}
