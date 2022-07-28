using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip diamondPickupSFX;
    private void OnTriggerEnter2D(Collider2D collision) {
        AudioSource.PlayClipAtPoint(diamondPickupSFX, Camera.main.transform.position);
        Destroy(gameObject);
    }

}
