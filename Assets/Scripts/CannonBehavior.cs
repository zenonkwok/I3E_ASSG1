using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;  /// The object to be instantiated as the projectile
    [SerializeField]
    Transform spawnPoint;   /// The point from which the projectile will be spawned
    [SerializeField]
    float fireStrength = 2f;    /// The strength of the force applied to the projectile when fired

    AudioSource cannonAudioSource;  /// Audio source for cannon firing sound

    void Start()
    {
        cannonAudioSource = GetComponent<AudioSource>();    ///Initializing audio source component
    }

    public void Fire()  /// Script for firing the cannon
    {
        PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
        if (player != null && player.Collectibles.Contains("cannonball"))   /// Checking if the player has a cannonball
        {
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);   ///Instantiating the projectile at the spawn point

            Vector3 fireForce = spawnPoint.forward * fireStrength;  /// Calculating the force to be applied to the projectile

            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);    /// Firing the projectile

            player.Collectibles.Remove("cannonball");   /// Removing the cannonball from the player's collectibles

            cannonAudioSource.Play();   /// Playing the cannon firing sound
        }
        else
        {
            player.Popup("You need a cannonball to fire the cannon.");  /// Displaying a message if the player does not have a cannonball
        }
    }
}
