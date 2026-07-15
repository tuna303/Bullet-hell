using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 30;

    private List<Bullet> pool = new List<Bullet>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);

            bullet.gameObject.SetActive(false);

            pool.Add(bullet);
        }
    }

    public Bullet GetBullet()
    {
        foreach (Bullet bullet in pool)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);

                return bullet;
            }
        }

        return null;
    }
}
