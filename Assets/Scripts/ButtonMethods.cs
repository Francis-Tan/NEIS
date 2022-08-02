using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethods : MonoBehaviour {
    /** 
     * For each button, We could also set renderer.material.color to colors 
     * in the methods OnMouseEnter() OnMouseExit() OnMouseClick()
     */

    /**public void GoToMenu() {
        SceneManager.LoadScene(0);
    }*/

    public void GoToLevelSelect() {
        CheckPointManager.UpdateCheckpoint(1, 50, 0); //put this in player awake if carrying around everywhere
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void PlayTutorial() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
