using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    Material highlightMat;
    [SerializeField]
    Material originalMat;
    public int value = 10;

    public string collectibleType = "Key";

    void Start()
    {
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
        Debug.Log("Collected " + value + " points!");
        Destroy(gameObject); // Destroy the collectible after collection
    }
    
}
