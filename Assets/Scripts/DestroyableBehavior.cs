using UnityEngine;

public class DestroyableBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    Material highlightMat;
    [SerializeField]
    Material originalMat;

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

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
