using UnityEngine;
using System.Collections;
public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootContinuously());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ShootContinuously()
    {
    while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
            }
    }
    private void Shoot()
    {
        
        
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
    }
}
