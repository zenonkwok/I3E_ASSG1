using UnityEngine;
using System.Collections;

public class Despawn : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit the wall, starting destruction sequence.");
            StartCoroutine(DestroyWallsAfterDelay(5f, other.gameObject));
        }
    }

    IEnumerator DestroyWallsAfterDelay(float delay, GameObject projectile)
    {
        yield return new WaitForSeconds(delay);
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Breakable Wall");
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
        Destroy(projectile);
    }
}
