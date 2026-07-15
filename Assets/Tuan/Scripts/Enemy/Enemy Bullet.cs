using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float knockbackForce = 5f;

    private Vector2 direction;
    private float timer;
    
    private void OnEnable()
    {
        timer = lifeTime;
    }

    public void Fire(Vector2 dir, float spd, int dmg)
    {
        direction = dir.normalized;
        this.speed = spd;
        this.damage = dmg;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Movement victimMovement = collision.gameObject.GetComponent<Movement>();
            if (victimMovement != null)
            {
                victimMovement.TakeDamage(damage);
            }  

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            
            gameObject.SetActive(false); 
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}