using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMethods : MonoBehaviour {
    /** 
     * For each button, We could also set renderer.material.color to colors 
     * in the methods OnMouseEnter() OnMouseExit() OnMouseClick()
     */

    public static void GoToMenu() {
        Player.SetVisible(false);
        PlayerInfo.SetVisibleAll(false);
        AudioManager.instance.PlayBGM(Sound.BGM_MainMenu);
        SceneManager.LoadScene(0);
    }

    public static void GoToLevelSelect() {
        Player.SetVisible(false);
        PlayerInfo.SetVisibleAll(false);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void PlayTutorial() {
        SceneManager.LoadScene(1);
        Player.GetInstance().GetComponent<Player>()
            .Spawn(new Vector2(-0.8f, -1.8f), 50, 0, 0, false);
        PlayerInfo.ShowBarsOnly();
    }

    public void QuitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
