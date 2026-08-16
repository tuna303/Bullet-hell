using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager1 : MonoBehaviour
{
   
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
