using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public float EnemyHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        EnemyHealth = Mathf.Clamp(EnemyHealth, 0, 3);
        if (EnemyHealth == 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy died: " + gameObject.name);
        }
    }
    
}
