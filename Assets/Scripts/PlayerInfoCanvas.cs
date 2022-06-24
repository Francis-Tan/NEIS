using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    public static GameObject instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = gameObject;
    }
}
