using UnityEngine;
using System.Collections.Generic;
public class DungeonGenerator : MonoBehaviour
{
    // thuoc tinh phong
    public enum RoomType {Combat, Utility}
    
    // cac loai phong
    public RoomInfo startRoom;
    public RoomInfo bossRoom;
    public RoomInfo[] combatRooms;
    public RoomInfo[] utilityRooms;

    // so luong phong moi loai
    public int minCombatRooms = 4;
    public int maxCombatRooms = 7;
    public int minUtilityRooms = 2;
    public int maxUtilityRooms = 4;

    // player
    public GameObject playerPrefab;

    // Danh sach luu tru tat ca cac phong da sinh ra de co the reset
    private List<GameObject> spawnedRooms = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDungeon();
    }
    void GenerateDungeon()
    {
       List<RoomType> middleRooms = new List<RoomType>();
       
       int combatCount = Random.Range(minCombatRooms, maxCombatRooms + 1);
       for (int i = 0; i < combatCount; i++)
        {
            middleRooms.Add(RoomType.Combat);
        }

        int utilityCount = Random.Range(minUtilityRooms, maxUtilityRooms + 1);
        for (int i = 0; i < utilityCount; i++)
        {
            middleRooms.Add(RoomType.Utility);
        }

        ShuffleList(middleRooms);
        FixAdjacentUtilities(middleRooms);

        RoomInfo startRoomInstance = Instantiate(startRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(startRoomInstance.gameObject);
        RoomInfo currentRoom = startRoomInstance;
        foreach(RoomType type in middleRooms)
        {
            RoomInfo roomSpawn = GetRandomPrefab(type);
            currentRoom = SpawnAndAlignRoom(roomSpawn, currentRoom);
            spawnedRooms.Add(currentRoom.gameObject);
        }
        RoomInfo bossRoomInstance = SpawnAndAlignRoom(bossRoom, currentRoom);
        spawnedRooms.Add(bossRoomInstance.gameObject);

        // Spawn player ngay sau exit cua start room (vi start room khong co entrance)
        SpawnPlayer(startRoomInstance);
        
    }

    /// <summary>
    /// Reset toan bo level: xoa tat ca phong da spawn, tao lai dungeon moi, hoi sinh player.
    /// </summary>
    public void ResetLevel()
    {
        // Xoa tat ca phong da spawn
        foreach (GameObject room in spawnedRooms)
        {
            if (room != null)
            {
                Destroy(room);
            }
        }
        spawnedRooms.Clear();

        // Tao lai dungeon moi
        GenerateDungeon();

        // Hoi sinh player: bat lai gameObject, reset mau, dat vi tri
        if (playerPrefab != null)
        {
            playerPrefab.SetActive(true);
            PlayerHealth playerHealth = playerPrefab.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }
        }
    }

    private RoomInfo SpawnAndAlignRoom(RoomInfo prefabSpawn, RoomInfo previousRoom)
    {
        RoomInfo newRoom = Instantiate(prefabSpawn);
        // Cong thuc can chinh: dat entrance cua phong moi trung voi exit cua phong truoc
        // offset = khoang cach tu tam phong moi den entrance cua no
        Vector3 offset = newRoom.entrance.position - newRoom.transform.position;

        // Vi tri phong moi = vi tri exit phong truoc - offset
        // Nhu vay entrance cua phong moi se nam dung tai vi tri exit cua phong truoc
        newRoom.transform.position = previousRoom.exit.position - offset;

        return newRoom;
    }

    private RoomInfo GetRandomPrefab(RoomType type)
    {
        if(type == RoomType.Combat) return combatRooms[Random.Range(0, combatRooms.Length)];
        else return utilityRooms[Random.Range(0, utilityRooms.Length)];
    }

    private void SpawnPlayer(RoomInfo startRoomInstance)
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("DungeonGenerator: Chua gan Player Prefab trong Inspector!");
            return;
        }
        // Di chuyen player da co san trong scene den vi tri phong start
        playerPrefab.transform.position = startRoomInstance.transform.position;
    }

    private void FixAdjacentUtilities(List<RoomType> rooms)
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            if (rooms[i] == RoomType.Utility && rooms[i + 1] == RoomType.Utility)
            {
                for (int j = i + 2; j < rooms.Count; j++)
                {
                    if (rooms[j] == RoomType.Combat)
                    {
                        RoomType temp = rooms[i + 1];
                        rooms[i + 1] = rooms[j];
                        rooms[j] = temp;
                        break;
                    }
                }
                if (rooms[i + 1] == RoomType.Utility)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (rooms[j] == RoomType.Combat)
                        {
                            RoomType temp = rooms[i];
                            rooms[i] = rooms[j];
                            rooms[j] = temp;
                            break;
                        }
                    }
                }
            }
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
