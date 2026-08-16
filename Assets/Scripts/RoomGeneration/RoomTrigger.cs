using UnityEngine;

/// <summary>
/// Gắn script này vào GameObject cảm biến (có Collider2D IsTrigger) 
/// đặt ở lối vào phòng. Khi Player chạm vào, nó sẽ gọi TriggerRoom() 
/// trên RoomInfo tương ứng để khóa cửa và spawn quái.
/// </summary>
public class RoomTrigger : MonoBehaviour
{
    [Tooltip("Kéo RoomInfo của phòng này vào đây.")]
    public RoomInfo targetRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetRoom != null)
        {
            targetRoom.TriggerRoom();
        }
    }
}