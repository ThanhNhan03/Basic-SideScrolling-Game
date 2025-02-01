using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSfx;
    [SerializeField] int coinValue = 100;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            FindAnyObjectByType<GameSession>().AddToScore(coinValue);
            AudioSource.PlayClipAtPoint(coinPickupSfx, transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
