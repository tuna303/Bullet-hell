using UnityEngine;
using UnityEngine.SceneManagement;
public class Turtorial : MonoBehaviour
{
    public int sceneToLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem vật thể chạm vào có tag là "Player" hay không
        if (other.CompareTag("Player"))
        {
         
            
            // Thực hiện chuyển cảnh sang scene có index là 3
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
