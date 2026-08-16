using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnPoint
{
    public Transform spawnPoint;
    public GameObject enemyPrefab;
}

public class RoomInfo : MonoBehaviour
{
    [Header("Cửa vào / ra")]
    public Transform entrance;
    public Transform exit;
    public GameObject doorBlockerEnterance; // Object chắn cửa, sẽ SetActive(true) khi khóa phòng
    public GameObject doorBlockerExit;

    [Header("Kẻ thù cố định")]
    public EnemySpawnPoint[] enemySpawnPoints;

    private int aliveEnemyCount;
    private bool hasSpawned = false;
    private bool roomLocked = false;

    /// <summary>
    /// Spawn quái cố định tại các vị trí đã được gán sẵn trong Inspector.
    /// Mỗi spawnPoint luôn spawn đúng loại enemyPrefab tương ứng.
    /// </summary>
    public void SpawnEnemies()
    {
        if (hasSpawned) return;
        if (enemySpawnPoints == null || enemySpawnPoints.Length == 0)
        {
            Debug.LogWarning($"RoomInfo [{gameObject.name}]: Không có enemySpawnPoints nào được gán.");
            return;
        }

        foreach (EnemySpawnPoint esp in enemySpawnPoints)
        {
            if (esp.spawnPoint == null || esp.enemyPrefab == null)
            {
                Debug.LogWarning($"RoomInfo [{gameObject.name}]: SpawnPoint hoặc EnemyPrefab bị thiếu, bỏ qua.");
                continue;
            }

            GameObject enemy = Instantiate(esp.enemyPrefab, esp.spawnPoint.position, esp.spawnPoint.rotation);
            aliveEnemyCount++;

            // Đăng ký callback: khi enemy/boss chết, nó sẽ báo về đây
            // Ưu tiên BossHealth trước, nếu không có thì tìm EnemyHealth
            BossHealth bossHealth = enemy.GetComponentInChildren<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.roomOwner = this;
            }
            else
            {
                EnemyHealth enemyHealth = enemy.GetComponentInChildren<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.roomOwner = this;
                }
                else
                {
                    Debug.LogError($"RoomInfo [{gameObject.name}]: Prefab '{esp.enemyPrefab.name}' không có EnemyHealth hoặc BossHealth!");
                }
            }
        }

        hasSpawned = true;
    }

    /// <summary>
    /// Enemy gọi hàm này khi nó chết.
    /// </summary>
    public void NotifyEnemyDied()
    {
        aliveEnemyCount--;
        if (aliveEnemyCount <= 0 && roomLocked)
        {
            UnlockRoom();
        }
    }

    /// <summary>
    /// Được gọi từ RoomTrigger khi Player đi vào trigger zone.
    /// Spawn quái (nếu chưa spawn) rồi khóa cửa.
    /// </summary>
    public void TriggerRoom()
    {
        if (roomLocked) return;

        if (!hasSpawned)
        {
            SpawnEnemies();
        }

        // Nếu không có enemy nào được spawn, không khóa cửa
        if (aliveEnemyCount <= 0 && hasSpawned)
        {
            return; // Phòng trống, không cần khóa
        }

        roomLocked = true;
        if (doorBlockerEnterance != null)
        {
            doorBlockerEnterance.SetActive(true);
        }
        if (doorBlockerExit != null)
        {
            doorBlockerExit.SetActive(true);
        }
    }

    private void UnlockRoom()
    {
        roomLocked = false;
        if (doorBlockerEnterance != null)
        {
            doorBlockerEnterance.SetActive(false);
        }
        if (doorBlockerExit != null)
        {
            doorBlockerExit.SetActive(false);
        }
        Debug.Log($"RoomInfo [{gameObject.name}]: Tất cả quái đã chết, mở cửa!");
    }
}