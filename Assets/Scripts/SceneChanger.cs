using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);
    
}
