using UnityEngine;

/// <summary>
/// Boss hình vuông với 3 phase tự động xoay vòng:
/// 
/// GREEN  (Xanh lá): Dash thẳng tới Player. Khi chạm tường → dừng & bắn 8 hướng.
/// RED    (Đỏ)    : Sấy đạn theo chuỗi — bắn liên tục đạn hướng về Player.
/// YELLOW (Vàng)   : Bắn đạn 8 hướng đan xen toàn màn hình, mật độ dày.
/// 
/// Yêu cầu trên Boss:
/// - Rigidbody2D (Dynamic, Gravity Scale = 0)
/// - Collider2D (không IsTrigger) để va chạm tường & Player
/// - Tag: "Enemy"
/// - SpriteRenderer (hình vuông)
/// - Animator (với 3 animation: "Green", "Red", "Yellow")
/// - BossHealth (quản lý máu, tích hợp RoomInfo)
/// 
/// Yêu cầu trong scene:
/// - Tường có Tag: "Wall" và Collider2D
/// - Player có Tag: "Player"
/// - Bullet Prefab có BossBullet + Tag "EnemyBullet" + DamageDealer
/// </summary>
public class BossController : MonoBehaviour
{
    public enum BossState { Green, Red, Yellow }

    [Header("State")]
    public BossState currentState = BossState.Green;
    public float stateChangeInterval = 5f;
    private float stateTimer;

    // ==================== GREEN - DASH + 8 HƯỚNG ====================
    [Header("Green - Dash tới Player, chạm tường → bắn 8 hướng")]
    public float greenDashSpeed = 15f;
    public float greenDashPrepareTime = 1.2f;
    public int greenBurstBulletCount = 8;
    public float greenBulletSpeed = 7f;
    public int greenBulletDamage = 1;
    private bool greenIsDashing;
    private float greenDashTimer;
    private Vector2 greenDashDirection;
    private bool greenHasFired;

    // ==================== RED - SẤY ĐẠN CHUỖI ====================
    [Header("Red - Sấy đạn chuỗi hướng về Player")]
    public float redFireRate = 0.08f;
    public float redBulletSpeed = 10f;
    public int redBulletDamage = 1;
    public float redSpreadAngle = 10f;
    private float redFireTimer;
    private bool redActive;

    // ==================== YELLOW - ĐẠN 8 HƯỚNG TOÀN MÀN HÌNH ====================
    [Header("Yellow - Đạn 8 hướng đan xen toàn màn hình")]
    public GameObject bulletPrefab;
    public float yellowBulletSpeed = 4f;
    public int yellowBulletDamage = 1;
    public float yellowBurstInterval = 0.35f;
    public int yellowWavesPerCycle = 6;
    public float yellowAngleOffsetPerWave = 15f;
    private float yellowBurstTimer;
    private int yellowWaveCount;
    private float yellowCurrentBaseAngle;
    private bool yellowActive;

    [Header("Contact Damage")]
    public int contactDamage = 1;

    // ==================== THAM CHIẾU ====================
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        stateTimer = stateChangeInterval;
        greenDashTimer = greenDashPrepareTime;

        UpdateStateVisual();
        PlayStateAnimation();
        FlipTowardsPlayer();
    }

    void Update()
    {
        if (player == null) return;

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            CycleState();
            stateTimer = stateChangeInterval;
        }

        // Flip mặt về Player mỗi frame
        FlipTowardsPlayer();

        switch (currentState)
        {
            case BossState.Green:  UpdateGreen();  break;
            case BossState.Red:    UpdateRed();    break;
            case BossState.Yellow: UpdateYellow(); break;
        }
    }

    // ==================== STATE MANAGEMENT ====================

    void CycleState()
    {
        StopGreen();
        StopRed();
        StopYellow();

        currentState = (BossState)(((int)currentState + 1) % 3);
        ResetAllTimers();
        UpdateStateVisual();
        PlayStateAnimation();
    }

    void ResetAllTimers()
    {
        greenIsDashing = false;
        greenDashTimer = greenDashPrepareTime;
        greenHasFired = false;
        redActive = false;
        redFireTimer = 0f;
        yellowActive = false;
        yellowWaveCount = 0;
        yellowBurstTimer = 0f;
        yellowCurrentBaseAngle = Random.Range(0f, 360f);
    }

    void UpdateStateVisual()
    {
        if (sr == null) return;
        sr.color = currentState switch
        {
            BossState.Green  => Color.green,
            BossState.Red    => Color.red,
            BossState.Yellow => Color.yellow,
            _                => Color.white
        };
    }

    /// <summary>
    /// Chạy animation tương ứng với state hiện tại.
    /// Tên animation phải là "Green", "Red", "Yellow" trong Animator.
    /// </summary>
    void PlayStateAnimation()
    {
        if (anim == null) return;
        string animName = currentState.ToString(); // "Green", "Red", "Yellow"
        anim.Play(animName);
    }

    /// <summary>
    /// Flip sprite trái/phải để boss luôn hướng mặt về Player.
    /// Mặc định boss nhìn phải (localScale.x > 0).
    /// Dùng Mathf.Abs để tránh scale bị trôi sau nhiều lần flip.
    /// Chỉ flip trên trục X, không ảnh hưởng hướng đạn.
    /// </summary>
    void FlipTowardsPlayer()
    {
        if (sr == null || player == null) return;

        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);

        if (player.position.x < transform.position.x)
        {
            // Player bên trái → flip sang trái (scale.x âm)
            scale.x = -absX;
        }
        else
        {
            // Player bên phải → mặt mặc định phải (scale.x dương)
            scale.x = absX;
        }

        transform.localScale = scale;
    }

    // ==================== GREEN: DASH → CHẠM TƯỜNG → BẮN 8 HƯỚNG ====================

    void UpdateGreen()
    {
        if (!greenIsDashing && !greenHasFired)
        {
            // Giai đoạn nạp: đứng yên, đếm ngược, luôn hướng về Player
            greenDashTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;

            if (player != null)
                greenDashDirection = (player.position - transform.position).normalized;

            if (greenDashTimer <= 0)
            {
                greenIsDashing = true;
                rb.linearVelocity = greenDashDirection * greenDashSpeed;
            }
        }
        else if (greenIsDashing)
        {
            // Đang dash → timeout 3s nếu không chạm tường
            greenDashTimer += Time.deltaTime;
            if (greenDashTimer >= 3f)
            {
                OnWallHit();
            }
        }
    }

    void OnWallHit()
    {
        if (greenHasFired) return;
        greenIsDashing = false;
        rb.linearVelocity = Vector2.zero;
        greenHasFired = true;

        FireDirectionalBullets(greenBurstBulletCount, greenBulletSpeed, greenBulletDamage);
    }

    void StopGreen()
    {
        greenIsDashing = false;
        greenHasFired = false;
        greenDashTimer = greenDashPrepareTime;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    // ==================== RED: SẤY ĐẠN CHUỖI HƯỚNG VỀ PLAYER ====================

    void UpdateRed()
    {
        if (!redActive)
        {
            redActive = true;
            redFireTimer = 0f;
        }

        redFireTimer -= Time.deltaTime;
        while (redFireTimer <= 0)
        {
            redFireTimer += redFireRate;

            if (bulletPrefab == null || player == null) break;

            // Hướng chính về Player
            Vector2 baseDir = (player.position - transform.position).normalized;

            // Thêm spread ngẫu nhiên để tạo hiệu ứng sấy (rải nhẹ)
            float spread = Random.Range(-redSpreadAngle, redSpreadAngle);
            float angle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg + spread;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BossBullet bossBullet = bullet.GetComponent<BossBullet>();
            if (bossBullet != null)
            {
                bossBullet.Initialize(dir, redBulletSpeed, redBulletDamage);
            }

            // Giới hạn không bắn quá 30 viên/frame
            if (redFireTimer > 0.3f) break;
        }
    }

    void StopRed()
    {
        redActive = false;
    }

    // ==================== YELLOW: ĐẠN 8 HƯỚNG ĐAN XEN ====================

    void UpdateYellow()
    {
        if (!yellowActive)
        {
            yellowActive = true;
            yellowWaveCount = 0;
            yellowBurstTimer = 0f;
            yellowCurrentBaseAngle = Random.Range(0f, 360f);
        }

        yellowBurstTimer -= Time.deltaTime;
        if (yellowBurstTimer <= 0 && yellowWaveCount < yellowWavesPerCycle)
        {
            yellowBurstTimer = yellowBurstInterval;
            yellowWaveCount++;

            float baseAngle = yellowCurrentBaseAngle + (yellowWaveCount * yellowAngleOffsetPerWave);
            Fire8DirectionBullets(baseAngle, yellowBulletSpeed, yellowBulletDamage);
        }
    }

    void StopYellow()
    {
        yellowActive = false;
        yellowWaveCount = 0;
        yellowBurstTimer = 0f;
    }

    // ==================== HÀM BẮN CHUNG ====================

    void Fire8DirectionBullets(float baseAngle, float speed, int damage)
    {
        if (bulletPrefab == null) return;
        for (int i = 0; i < 8; i++)
        {
            float angle = baseAngle + (i * 45f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BossBullet bossBullet = bullet.GetComponent<BossBullet>();
            if (bossBullet != null)
                bossBullet.Initialize(dir, speed, damage);
        }
    }

    void FireDirectionalBullets(int count, float speed, int damage)
    {
        if (bulletPrefab == null) return;
        float angleStep = 360f / count;
        float baseAngle = Random.Range(0f, 360f);
        for (int i = 0; i < count; i++)
        {
            float angle = baseAngle + (angleStep * i);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BossBullet bossBullet = bullet.GetComponent<BossBullet>();
            if (bossBullet != null)
                bossBullet.Initialize(dir, speed, damage);
        }
    }

    // ==================== VA CHẠM ====================

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (greenIsDashing && collision.gameObject.CompareTag("Wall"))
        {
            OnWallHit();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(contactDamage);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (greenIsDashing && !greenHasFired && collision.gameObject.CompareTag("Wall"))
        {
            OnWallHit();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(contactDamage);
        }
    }
}