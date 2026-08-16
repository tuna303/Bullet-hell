using UnityEngine;

public class PistolEnemy : MonoBehaviour
{
    
    public Transform enemyBody;

  
    public SpriteRenderer gunSprite;
    public Transform firePoint;
    public float fireRate = 1.5f;
    
    private float nextFireTime = 0f;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        // 1. Luôn đi theo quái vật NHƯNG nằm độc lập bên ngoài Hierarchy
        if (enemyBody != null)
        {
            transform.position = enemyBody.position;
        }

        if (player == null) return;

        // 2. Chĩa súng
        TrackPlayer();

        // 3. Bóp cò
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void TrackPlayer()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Xoay trục súng theo góc
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // --- XỬ LÝ LỖI SÚNG NGỬA BỤNG ---
        if (gunSprite != null)
        {
            // Nếu góc ngắm lớn hơn 90 hoặc nhỏ hơn -90 (tức là ngắm sang NỬA TRÁI màn hình)
            if (angle > 90f || angle < -90f)
            {
                gunSprite.flipY = true;  // Lật trục Y để báng súng hướng xuống dưới
            }
            else
            {
                gunSprite.flipY = false; // Trả về bình thường khi ngắm sang phải
            }
        }
    }

   private void Shoot()
    {
        // Kiểm tra kho đạn quái vật
        if (EnemyBulletPool.Instance == null) return;
        
        // Lấy đạn từ EnemyBulletPool
        GameObject bullet = EnemyBulletPool.Instance.pool.Get();
        
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        // Gọi lệnh Fire từ kịch bản EnemyBullet
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.FireBullet();
        }
    }
}