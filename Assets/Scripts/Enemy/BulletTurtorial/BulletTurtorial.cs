using UnityEngine;

public class BulletTurtorial : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 respawnPosition;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        // Dùng MovePosition để di chuyển + vẫn detect trigger
        rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            Transform playerTransform = collision.GetComponent<Transform>();
            PlayerController playerScript = collision.GetComponent<PlayerController>();
            if (playerTransform != null && playerScript != null)
            {
                if(playerScript.isRolling == false)
                {
                    playerTransform.position = respawnPosition;
                    Destroy(gameObject);
                    
                }
    
                else
                {
                    return;
                }
            

            }
           
            
            
        }
        else if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
            Debug.Log("ưalled");
        }
       
        
    }
    
  
}
