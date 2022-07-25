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
        CheckPointManager.UpdateCheckpoint(2, 50, 0);
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
