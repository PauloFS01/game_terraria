using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] int liveValue = 1;
    [SerializeField] AudioClip diamondPickupSFX;
    private void OnTriggerEnter2D(Collider2D collision) {
        AudioSource.PlayClipAtPoint(diamondPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToLive(liveValue);
        Destroy(gameObject);
    }

}
