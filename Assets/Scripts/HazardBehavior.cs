using UnityEngine;
using System.Collections; // For Coroutines

public class HazardBehavior : MonoBehaviour
{
    // Amount of health to recover
    [SerializeField]
    int DamageAmount = -10;
    [SerializeField]
    float TickRate = 1;
    [SerializeField]
    string InvulnerabilityItem = null;

    private Coroutine damageCoroutine;

    public void StartHazardDamage(PlayerBehavior player)
    {
        if (damageCoroutine == null && player.health > 0)
        {
            damageCoroutine = StartCoroutine(DamagePlayerOverTime(player));
        }
            
    }

    public void StopHazardDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamagePlayerOverTime(PlayerBehavior player)
    {
        while (true)
        {
            if (!string.IsNullOrEmpty(InvulnerabilityItem) && player.Collectibles.Contains(InvulnerabilityItem))
            {
                yield return new WaitForSeconds(TickRate);
                continue;
            }
            else if (player.health <= 0)
            {
                StopHazardDamage();
                yield break;
            }
            else
            {
                player.HazardDamage(DamageAmount);
                yield return new WaitForSeconds(TickRate);
            }
            
        }
    }
}