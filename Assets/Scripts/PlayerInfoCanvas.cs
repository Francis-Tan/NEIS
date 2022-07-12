using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    public static Renderer[] renderers;
    private void Awake()
    {
        if (instance != null && instance != gameObject) {
            Debug.Log("Deleting old canvas found");
            Destroy(instance);
        }
        instance = gameObject;
        renderers = GetComponentsInChildren<Renderer>();
        DontDestroyOnLoad(instance);
    }
}
