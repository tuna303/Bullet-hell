using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
   
    public float moveSpeed = 5f;


    public float rollSpeed = 12f;      
    public float rollDuration = 0.4f;  
    public float rollCooldown = 1f;   

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool isFacingRight = true;
    

   public bool isRolling { get; private set; } = false;
    private Vector2 rollDirection;
    private float rollTimeCounter;
    private float rollCooldownCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Quản lý thời gian Hồi chiêu lộn nhào
        if (rollCooldownCounter > 0)
        {
            rollCooldownCounter -= Time.deltaTime;
        }

        // 2. KIỂM TRA TRẠNG THÁI: Đang lộn hay Đang chạy?
        if (isRolling)
        {
            // --- KỊCH BẢN ĐANG LỘN ---
            rollTimeCounter -= Time.deltaTime;

            if (rollTimeCounter <= 0f)
            {
                isRolling = false; // Hết thời gian lộn -> Mở khóa trạng thái
            }
        }
        else
        {
          
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;

            // Xử lý Animation chạy
            bool isMoving = moveInput.magnitude > 0;
            animator.SetBool("isMoving", isMoving);

            // Lật mặt nhân vật
            if (moveInput.x != 0)
            {
                CheckFlip(moveInput.x);
            }

            // Lắng nghe phím Lộn nhào (Space hoặc Chuột phải)
            if (Input.GetKeyDown(KeyCode.Space) && rollCooldownCounter <= 0f)
            {
                StartRoll();
            }
        }
    }

    void FixedUpdate()
    {
        // 3. Áp dụng Vật lý dựa theo Trạng thái
        if (isRolling)
        {
            // Phóng đi theo hướng cuộn với tốc độ cao
            rb.linearVelocity = rollDirection * rollSpeed;
        }
        else
        {
            // Chạy bộ bình thường
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    private void StartRoll()
    {
        // Khóa phím, bật trạng thái Lộn
        isRolling = true;
        rollTimeCounter = rollDuration;
        rollCooldownCounter = rollCooldown;

        // Xác định hướng lộn (Roll Direction)
        if (moveInput.magnitude > 0)
        {
            rollDirection = moveInput; // Lộn theo hướng đang bấm
        }
        else
        {
            // Nếu đang đứng im, lộn về phía trước mặt
            rollDirection = isFacingRight ? Vector2.right : Vector2.left; 
        }

        // Kích hoạt cò súng Animation lộn nhào
        animator.SetTrigger("doRoll");
    }

    private void CheckFlip(float xInput)
    {
        if (xInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (xInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}