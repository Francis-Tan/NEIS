using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static PauseMenu instance;
    private static CanvasGroup canvasGroup;
    public static bool paused;

    private void Awake() {
        if (instance != null) {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        instance = GetComponent<PauseMenu>();
        canvasGroup = GetComponent<CanvasGroup>();
        paused = false;
    }

    public static void Open() {
        Time.timeScale = 0;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        paused = true;
        Narrator.HideOnPause();
    }

    public static void Close() {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        paused = false;
        Narrator.ShowOnUnpause();
    }

    public static void GoToLevelSelect() {
        SceneMethods.GoToLevelSelect();
        Time.timeScale = 1;
        paused = false;
    }

    public static void GoToMenu() {
        SceneMethods.GoToMenu();
        Time.timeScale = 1;
        paused = false;
        PlayerInfo.instance.gunIcon.show();
        PlayerInfo.instance.stunIcon.show();
        PlayerInfo.ResizePlayerInfoCollider(-219.8962f, 353.9455f);
        Player.GetInstance().skillLevel = 2;
    }
}
