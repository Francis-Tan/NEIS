using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    public static Renderer[] renderers;
    public bool inTutorial;
    private void Awake()
    {
        if (instance != null) {
            Destroy(gameObject); 
            return;
        }
        instance = gameObject;
        renderers = GetComponentsInChildren<Renderer>();
        if (!inTutorial) DontDestroyOnLoad(instance);
    }
}
