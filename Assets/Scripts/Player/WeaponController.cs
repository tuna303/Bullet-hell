using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Mục tiêu Bám đuôi")]
    [SerializeField] private Transform targetPlayer;     // Kéo Player vào đây

    [Header("Cài đặt Súng")]
    [SerializeField] private SpriteRenderer gunRenderer; 
    [SerializeField] private Transform firePoint;        
    
    [Header("Cài đặt Bắn")]
    [SerializeField] private float fireRate = 0.15f;     
    private float fireTimer;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main; 
    }

    // Dùng LateUpdate thay cho Update để tránh lỗi giật hình (Jitter) khi bám đuôi
    void LateUpdate()
    {
        // 1. Kịch bản bám đuôi
        if (targetPlayer != null)
        {
            transform.position = targetPlayer.position;
        }

        // 2. Kịch bản ngắm và bắn
        Aim();
        Shoot();
    }

   private void Aim()
    {
        // 1. Chuyển tọa độ chuột trên màn hình thành tọa độ trong Game World
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // [CÚ CHỐT SỬA LỖI]: Ép trục Z của chuột về 0 để cùng mặt phẳng 2D với Player
        mousePos.z = 0f;

        // 2. Tính Vector hướng từ súng chỉ tới chuột
        Vector3 aimDirection = (mousePos - transform.position).normalized;

        // [BẪY LỖI]: Chỉ cho phép súng xoay khi vector hướng khác 0 (chuột không đè lên chính giữa người)
        if (aimDirection != Vector3.zero)
        {
            // 3. Dùng Lượng giác tính ra góc (Angle)
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            // 4. Áp dụng góc xoay cho WeaponPivot
            transform.eulerAngles = new Vector3(0, 0, angle);

            // 5. Sửa lỗi súng bị lộn ngược khi chĩa sang trái
            if (angle > 90 || angle < -90)
            {
                gunRenderer.flipY = true;
            }
            else
            {
                gunRenderer.flipY = false;
            }
        }
    }
   private void Shoot()
    {
        fireTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            if (BulletPool.Instance != null && BulletPool.Instance.pool != null)
            {
                GameObject bullet = BulletPool.Instance.pool.Get();
                
                // 1. Cập nhật vị trí
                bullet.transform.position = firePoint.position;
                
                // 2. Cập nhật góc xoay chuẩn theo chuột
                bullet.transform.rotation = transform.rotation;
                
                // 3. [CÚ CHỐT]: Gọi viên đạn và ra lệnh kích nổ vật lý
                bullet.GetComponent<Bullet>().FireBullet();
            }
            fireTimer = fireRate;
        }
    }
}