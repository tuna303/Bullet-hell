using UnityEngine;

public class SignInteract : MonoBehaviour
{
    public GameObject textBoxUI;
    public GameObject signBoard;
    private bool isPlayerInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (textBoxUI != null)
        {
            textBoxUI.SetActive(false);
        }
         if (signBoard != null)
        {
            signBoard.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Khi người chơi đứng trong vùng và nhấn E, toggle textBoxUI
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (textBoxUI != null)
            {
                bool isActive = textBoxUI.activeSelf;
                textBoxUI.SetActive(!isActive);
            }
            if (textBoxUI != null)
            {
                bool isActive = signBoard.activeSelf;
                signBoard.SetActive(!isActive);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            // Hiện sign board khi người chơi vào vùng
            if (signBoard != null)
            {
                signBoard.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // Ẩn sign board khi người chơi rời vùng
            if (signBoard != null)
            {
                signBoard.SetActive(false);
            }
            
            // Ẩn text khi người chơi rời vùng
            if (textBoxUI != null)
            {
                textBoxUI.SetActive(false);
            }
        }
    }
}
