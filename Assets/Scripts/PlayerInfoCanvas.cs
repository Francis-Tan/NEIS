using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    private void Awake()
    {
        if (instance != null && instance != gameObject) {
            Debug.Log("Using new canvas");
            Destroy(instance);
        }
        instance = gameObject;
        DontDestroyOnLoad(instance);
    }
}
