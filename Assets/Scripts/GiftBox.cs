using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public GameObject giftBox;

    [SerializeField]
    public GameObject coin;

    [SerializeField]
    Transform spawnPoint;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GameObject newCoin = Instantiate(coin, giftBox.transform.position, giftBox.transform.rotation);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

}
