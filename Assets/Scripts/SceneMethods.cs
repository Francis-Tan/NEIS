using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneMethods : MonoBehaviour {
    /** 
     * For each button, We could also set renderer.material.color to colors 
     * in the methods OnMouseEnter() OnMouseExit() OnMouseClick()
     */
    public static Vector2 MainLevelPos = new Vector2(-12.7f, 6.73f);
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
        Player.GetInstance()
            .Spawn(new Vector2(-0.8f, -1.8f), 50, 0, 0, false);
        PlayerInfo.ShowBarsOnly();
        SceneManager.LoadSceneAsync(1);
    }

    public static void LoadCheckPointLevel(int FloorNumber, CheckPointManager.PlayerData playerData) {
        Player.EnableStunSR(false);
        AudioManager.instance.PlayBGM(Sound.BGM_MainLevels);
        Player.GetInstance().Spawn(SceneMethods.MainLevelPos,
            playerData.hp, playerData.mana, 2);
        Player.GetInstance().StartCoroutine(LoadNextLevel());
        IEnumerator LoadNextLevel() {
            yield return new WaitForSeconds(0f);
            SceneManager.LoadSceneAsync(FloorNumber + 1);
            Player.EnableStunSR(true); //must be under waitforseconds to be hidden
            PlayerInfo.SetVisibleAll(true);
        }
    }

    public void QuitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
