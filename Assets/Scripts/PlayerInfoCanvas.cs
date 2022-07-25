using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    public static Renderer[] renderers;
    public bool inTutorial;
    private void Awake()
    {
        if (instance != null) {
            Destroy(SceneManager.GetActiveScene().buildIndex == 1 ? instance : gameObject); 
            return;
        }
        instance = gameObject;
        renderers = GetComponentsInChildren<Renderer>();
        if (!inTutorial) DontDestroyOnLoad(instance);
    }
}
