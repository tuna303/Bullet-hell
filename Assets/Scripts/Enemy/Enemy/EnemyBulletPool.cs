using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
   
    public static EnemyBulletPool Instance { get; private set; }
    public GameObject enemyBulletPrefab;

    // Bể chứa đạn
     public ObjectPool<GameObject> pool;

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Khởi tạo Object Pool
        pool = new ObjectPool<GameObject>(
            CreateBullet, 
            OnTakeBullet, 
            OnReturnBullet, 
            OnDestroyBullet, 
            true, 20, 100); // Mặc định tạo 20 viên, tối đa 100 viên
    }

    private GameObject CreateBullet()
    {
        return Instantiate(enemyBulletPrefab);
    }

    private void OnTakeBullet(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void OnDestroyBullet(GameObject bullet)
    {
        DestroyImmediate(bullet);
    }
}