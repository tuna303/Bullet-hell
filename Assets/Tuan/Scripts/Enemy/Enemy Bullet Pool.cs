using UnityEngine;
using System.Collections.Generic;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance;

    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private List<EnemyBullet> pool = new List<EnemyBullet>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            EnemyBullet bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            pool.Add(bullet);
        }
    }

    public EnemyBullet GetBullet()
    {
        foreach (EnemyBullet bullet in pool)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        EnemyBullet newBullet = Instantiate(bulletPrefab, transform);
        pool.Add(newBullet);
        newBullet.gameObject.SetActive(true);
        return newBullet;
    }
}