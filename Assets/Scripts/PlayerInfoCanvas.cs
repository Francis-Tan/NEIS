using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    public static Renderer[] renderers;
    private void Awake()
    {
        if (instance != null) {
            Destroy(gameObject); return;
        }
        instance = gameObject;
        renderers = GetComponentsInChildren<Renderer>();
        DontDestroyOnLoad(instance);
    }
}
