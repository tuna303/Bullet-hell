using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Hồi máu")]
    [SerializeField] private bool fullHeal = true;
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;

        // Nếu máu đã đầy thì không làm gì
        if (playerHealth.currentHealth >= playerHealth.maxHealth) return;

        if (fullHeal)
        {
            playerHealth.Heal(playerHealth.maxHealth); // Hồi đầy máu
        }
        else
        {
            playerHealth.Heal(healAmount);
        }

        Destroy(gameObject);
    }
}