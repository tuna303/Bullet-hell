using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    private int currentHealth;
    public GameObject deathPrefab;

    [HideInInspector] public RoomInfo roomOwner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // Báo cho RoomInfo biết enemy này đã chết
            if (roomOwner != null)
            {
                roomOwner.NotifyEnemyDied();
                Debug.Log($"EnemyHealth [{gameObject.name}]: Đã báo chết cho RoomInfo.");
            }
            else
            {
                Debug.LogWarning($"EnemyHealth [{gameObject.name}]: roomOwner = null! Enemy chết nhưng không ai biết.");
            }

            Destroy(gameObject);
            if (deathPrefab != null) deathPrefab.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log($"EnemyHealth [{gameObject.name}]: Trúng đạn, máu còn {currentHealth - 1}/{maxHealth}");
            TakeDamage(1);
        }
    }
}