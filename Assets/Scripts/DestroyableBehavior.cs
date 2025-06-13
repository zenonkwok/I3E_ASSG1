using UnityEngine;

public class DestroyableBehavior : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    Material highlightMat;
    [SerializeField]
    Material originalMat;

    [SerializeField]
    AudioClip destroySound;
    AudioSource destroyableAudioSource;

    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();

        originalMat = myMeshRenderer.material;
        
        destroyableAudioSource = GetComponent<AudioSource>();
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
        AudioSource.PlayClipAtPoint(destroySound, transform.position);
        destroyableAudioSource.Play();
        Destroy(gameObject);
    }
}
