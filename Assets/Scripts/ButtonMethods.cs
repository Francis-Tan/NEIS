using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethods : MonoBehaviour {
    /** 
     * For each button, We could also set renderer.material.color to colors 
     * in the methods OnMouseEnter() OnMouseExit() OnMouseClick()
     */
    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void Retry() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
