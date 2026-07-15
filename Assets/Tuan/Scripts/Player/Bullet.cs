using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    [SerializeField] private float damage = 1;
    private Vector2 direction;

    [SerializeField] private float lifeTime = 3f;

    private float timer;
    
    private void OnEnable()
    {
        timer = lifeTime;
    }

    public void Fire(Vector2 dir, float spd, float dmg)
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

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Viên đạn vừa đụng trúng: " + other.gameObject.name);
    //     if (other.CompareTag("Player")) 
    //     {
    //         return; 
    //     }

    //     if (other.CompareTag("Wall"))
    //     {
    //         gameObject.SetActive(false);
    //     }
    //     else if (other.CompareTag("Enemy"))
    //     {
    //         Debug.Log("Enemy -10 hp" + other.gameObject.name);
    //         gameObject.SetActive(false);
    //     }
    // }
     void OnClo(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            EnemyHealthManager victimHealth = collision.gameObject.GetComponent<EnemyHealthManager>();
            if (victimHealth != null)
            {
                victimHealth.TakeDamage((int)damage);
            }  
        }
    }
}
