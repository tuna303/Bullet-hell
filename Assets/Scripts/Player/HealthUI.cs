using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thư viện bắt buộc để can thiệp vào UI

public class HealthUI : MonoBehaviour
{
    [Header("Cấu hình UI")]
    public GameObject heartPrefab;     // Mẫu 1 trái tim
    public Transform heartsContainer;  // Cái hộp chứa các trái tim (Sắp xếp hàng ngang)
    public Sprite fullHeartSprite;     // Hình tim đầy
    public Sprite emptyHeartSprite;    // Hình tim rỗng

    // Danh sách quản lý các trái tim đang hiển thị trên màn hình
    private List<Image> heartImages = new List<Image>();

    // Bật máy nghe đài khi UI xuất hiện
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHeartsUI;
    }

    // Tắt máy nghe đài khi UI bị hủy (Để chống lỗi tràn bộ nhớ Memory Leak)
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHeartsUI;
    }

    // Hàm này sẽ tự động chạy mỗi khi PlayerHealth gọi OnHealthChanged?.Invoke
    private void UpdateHeartsUI(int currentHealth, int maxHealth)
    {
        // 1. SCALE UP: Nếu maxHealth tăng (Vd: từ 3 lên 4), đẻ thêm trái tim mới nhét vào danh sách
        while (heartImages.Count < maxHealth)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            heartImages.Add(newHeart.GetComponent<Image>());
        }

        // 2. HIỂN THỊ: Chạy vòng lặp để cập nhật hình ảnh
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < maxHealth)
            {
                // Bật trái tim lên (phòng khi trước đó bị ẩn)
                heartImages[i].gameObject.SetActive(true);
                
                // Logic cốt lõi: Nếu thứ tự tim nhỏ hơn máu hiện tại -> Tim Đầy. Ngược lại -> Tim Rỗng.
                heartImages[i].sprite = (i < currentHealth) ? fullHeartSprite : emptyHeartSprite;
            }
            else
            {
                // Giấu đi nếu số trái tim trong danh sách lỡ nhiều hơn maxHealth
                heartImages[i].gameObject.SetActive(false); 
            }
        }
    }
}