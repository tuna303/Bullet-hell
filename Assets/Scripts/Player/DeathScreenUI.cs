using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Quan ly man hinh "You Died" khi player chet.
/// Gan script nay vao mot GameObject chua UI death screen (Canvas/Panel).
/// </summary>
public class DeathScreenUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathScreenPanel;  // Panel chua toan bo death screen
    public Button retryButton;           // Nut Retry

    private void Start()
    {
        // An death screen khi bat dau
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);

        // Gan su kien cho nut Retry
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryClicked);
    }

    private void OnEnable()
    {
        // Lang nghe su kien player chet
        PlayerHealth.OnPlayerDied += ShowDeathScreen;
    }

    private void OnDisable()
    {
        // Huy lang nghe khi script bi tat/huy
        PlayerHealth.OnPlayerDied -= ShowDeathScreen;
    }

    /// <summary>
    /// Duoc goi khi player chet (tu PlayerHealth.OnPlayerDied event)
    /// </summary>
    private void ShowDeathScreen()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(true);
    }

    /// <summary>
    /// Xu ly khi nguoi choi nhan nut Retry.
    /// Reload toan bo scene hien tai (hoat dong ca trong Editor lan Build).
    /// </summary>
    private void OnRetryClicked()
    {
        // Reload scene bang buildIndex - dam bao hoat dong ca khi build
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
