using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int DamageDeal ;

    

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Movement victimMovement = collision.gameObject.GetComponent<Movement>();
            if (victimMovement != null)
            {
                victimMovement.TakeDamage(DamageDeal);
            }   
        }
    }
    
}
