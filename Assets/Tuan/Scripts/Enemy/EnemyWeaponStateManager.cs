using UnityEngine;

public class EnemyWeaponStateManager : MonoBehaviour
{
    private EnemyWeaponBaseState currentState;
    
    [SerializeField] public EnemyPistolState pistolState = new EnemyPistolState();

    [Header("Target")]
    public Transform targetPlayer;

    [Header("Weapon Objects")]
    public GameObject pistolObject;

    [HideInInspector] public SpriteRenderer currentWeaponSR;
    [HideInInspector] public Transform currentMuzzle;
    
    [Header("Muzzles")]
    public Transform pistolMuzzle;

    void Start()
    {
        if (pistolObject != null) pistolObject.SetActive(false);

        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) targetPlayer = playerObj.transform;
        }

        SwitchState(pistolState);
    }

    // void Update()
    // {
    //     if (targetPlayer == null) return;

    //     HandleGunRotation();

    //     if (currentState != null)
    //     {
    //         currentState.UpdateState(this);
    //     }
    // }

    void Update()
    {
        if (targetPlayer == null) return;

        // 1. Quái đang quay mặt về bên phải hay bên trái? 
        bool isFacingRight = transform.parent.localScale.x > 0;
        
        // 2. Player đang đứng ở bên phải hay bên trái của Quái?
        bool isPlayerToTheRight = (targetPlayer.position.x - transform.position.x) > 0;

        // 3. Quái chỉ "nhìn thấy" Player nếu cả 2 hướng này khớp nhau
        bool canSeePlayer = (isFacingRight && isPlayerToTheRight) || (!isFacingRight && !isPlayerToTheRight);

        if (canSeePlayer)
        {
            // KẺ ĐỊCH TRONG TẦM NHÌN: Xoay súng bám theo mục tiêu
            HandleGunRotation();
            
            // Cho phép State hoạt động (đếm ngược thời gian và xả đạn)
            if (currentState != null)
            {
                currentState.UpdateState(this);
            }
        }
        else
        {
            // QUAY LƯNG VỚI KẺ ĐỊCH: Hạ súng xuống vị trí nghỉ, ngưng bắn
            IdleGunRotation();
        }
    }

    private void IdleGunRotation()
    {
        // Khi quay lưng, súng trở về góc 0 độ so với thân hình của quái (luôn chĩa thẳng ra trước mặt)
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        
        // Trả lại hình dáng súng bình thường
        if (currentWeaponSR != null)
        {
            currentWeaponSR.flipY = false;
        }
    }

    private void HandleGunRotation()
    {
        // 1. Luôn lấy hướng nhìn chuẩn trong không gian thực (World Space)
        Vector2 lookDirection = targetPlayer.position - transform.position;
        float worldAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // 2. Kiểm tra xem nhân vật cha (Ghost/Player) có đang bị lật trục X hay không
        float localAngle = worldAngle;
        bool isFlippedByParent = transform.parent != null && transform.parent.localScale.x < 0;

        if (isFlippedByParent)
        {
            // Thuật toán bù trừ: Lật ngược lại góc xoay
            localAngle = 180f - worldAngle;
        }

        transform.localRotation = Quaternion.Euler(0, 0, localAngle);

        // Truyền trạng thái lật xuống hàm xử lý hình ảnh
        HandleFlip(worldAngle, isFlippedByParent);
    }

    private void HandleFlip(float worldAngle, bool isFlippedByParent)
    {
        // Mặc định súng sẽ lật Y nếu đang bắn về bên trái
        bool flip = worldAngle > 90 || worldAngle < -90;

        // QUAN TRỌNG: Nếu cha đã lật, bản thân súng đã bị lật theo. 
        // Ta phải đảo ngược lại điều kiện flipY để súng không bị lộn ngược bụng lên trời.
        if (isFlippedByParent)
        {
            flip = !flip;
        }

        if (currentWeaponSR != null)
        {
            currentWeaponSR.flipY = flip;
        }
    }

    public void SwitchState(EnemyWeaponBaseState state)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        
        currentState = state;         
        currentState.EnterState(this); 
    }
}

// using UnityEngine;

// public class EnemyWeaponStateManager : MonoBehaviour
// {
//     private EnemyWeaponBaseState currentState;
    
//     [SerializeField] public EnemyPistolState pistolState = new EnemyPistolState();

//     [Header("Target")]
//     public Transform targetPlayer;

//     [Header("Weapon Objects")]
//     public GameObject pistolObject;

//     [HideInInspector] public SpriteRenderer currentWeaponSR;
//     [HideInInspector] public Transform currentMuzzle;
    
//     [Header("Muzzles")]
//     public Transform pistolMuzzle;

//     void Start()
//     {
//         if (pistolObject != null) pistolObject.SetActive(false);

//         if (targetPlayer == null)
//         {
//             GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//             if (playerObj != null) targetPlayer = playerObj.transform;
//         }

//         SwitchState(pistolState);
//     }

//     void Update()
//     {
//         if (targetPlayer == null) return;

//         bool isFacingRight = transform.parent.localScale.x < 0; 
//         bool isPlayerToTheRight = (targetPlayer.position.x - transform.position.x) > 0;
//         bool canSeePlayer = (isFacingRight && isPlayerToTheRight) || (!isFacingRight && !isPlayerToTheRight);

//         if (canSeePlayer)
//         {
//             HandleGunRotation();
            
//             if (currentState != null)
//             {
//                 currentState.UpdateState(this);
//             }
//         }
//         else
//         {
//             IdleGunRotation();
//         }
//     }

//     private void HandleGunRotation()
//     {
//         Vector2 lookDirection = targetPlayer.position - transform.position;
//         float worldAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

//         float localAngle = worldAngle;
//         bool isFlippedByParent = transform.parent != null && transform.parent.localScale.x < 0;

//         if (isFlippedByParent)
//         {
//             localAngle = 180f - worldAngle;
//         }

//         transform.localRotation = Quaternion.Euler(0, 0, localAngle);
//         HandleFlip(worldAngle, isFlippedByParent);
//     }

//     private void HandleFlip(float worldAngle, bool isFlippedByParent)
//     {
//         bool flip = worldAngle > 90 || worldAngle < -90;

//         if (isFlippedByParent)
//         {
//             flip = !flip;
//         }

//         if (currentWeaponSR != null)
//         {
//             currentWeaponSR.flipY = flip;
//         }
//     }

//     private void IdleGunRotation()
//     {
//         transform.localRotation = Quaternion.Euler(0, 0, 0);
//         if (currentWeaponSR != null)
//         {
//             currentWeaponSR.flipY = false;
//         }
//     }

//     public void SwitchState(EnemyWeaponBaseState state)
//     {
//         if (currentState != null)
//         {
//             currentState.ExitState(this);
//         }
        
//         currentState = state;         
//         currentState.EnterState(this); 
//     }
// }