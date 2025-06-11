using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    Material highlightMat;
    Material originalMat;
    public int value = 10;

    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        
        originalMat = myMeshRenderer.material;
    }
    public void Highlight()
    {
        // Change the material to highlightMat when the coin is highlighted
        myMeshRenderer.material = highlightMat;
    }
    public void Unhighlight()
    {
        myMeshRenderer.material = originalMat;
    }

    public void Collect(PlayerBehavior player)
    {
        // Add the value of the coin to the player's score or inventory
        Debug.Log("Collected " + value + " coins!");
        Destroy(gameObject); // Destroy the coin after collection
    }
    
}
