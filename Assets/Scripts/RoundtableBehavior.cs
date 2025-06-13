using UnityEngine;
using System.Collections.Generic;

public class RoundtableBehavior : MonoBehaviour
{
    private List<string> requiredItems = new List<string>
    {
        "job_Application",
        "magic_Ring",
        "crystal",
        "sculpture",
    };
    public bool items_Placed = false;
    public bool item_Summoned = false;
    [SerializeField]
    Transform items;
    [SerializeField]
    GameObject summonableObject;

    private AudioSource roundtableAudioSource;
    [SerializeField]
    private AudioClip placeItemsSound;
    [SerializeField]
    private AudioClip summonSound;

    void Start()
    {
        items.gameObject.SetActive(false);
        summonableObject.SetActive(false);

        roundtableAudioSource = GetComponent<AudioSource>();
        /*Assert.IsNotNull(roundtableAudioSource);
        Assert.IsNotNull(placeItemsSound);
        Assert.IsNotNull(summonSound);*/
    }

    public void PlaceItems(PlayerBehavior player)
    {
        foreach (string item in requiredItems)
        {
            if (!player.Collectibles.Contains(item))
            {
                player.Popup("You need to collect all required items before placing them on the roundtable. " + string.Join(", ", requiredItems));
                return;
            }
        }

        player.Popup("All items have been placed on the roundtable.");
        items_Placed = true;
        items.gameObject.SetActive(true);
        roundtableAudioSource.PlayOneShot(placeItemsSound);
    }

    public void Summon()
    {
        if (item_Summoned)
        {
            Debug.Log("Summonable object has already been summoned.");
            return;
        }
        summonableObject.SetActive(true);
        item_Summoned = true;
        roundtableAudioSource.PlayOneShot(summonSound);
    }
}
