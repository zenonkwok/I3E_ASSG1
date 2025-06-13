using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    float fireStrength = 2f;

    AudioSource cannonAudioSource;

    void Start()
    {
        cannonAudioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
        if (player != null && player.Collectibles.Contains("cannonball"))
        {
            Debug.Log("Firing cannon!");

            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

            Vector3 fireForce = spawnPoint.forward * fireStrength;

            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);

            player.Collectibles.Remove("cannonball");

            cannonAudioSource.Play();
        }
        else
        {
            player.Popup("You need a cannonball to fire the cannon.");
        }
    }
}
