using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethods : MonoBehaviour {
    /** 
     * For each button, We could also set renderer.material.color to colors 
     * in the methods OnMouseEnter() OnMouseExit() OnMouseClick()
     */
    public static int checkpointIndex = 2, savedHealth = 50, savedMana = 0;

    public void PlayTutorial() {
        SceneManager.LoadScene(1);
    }

    public void PlayGame() {
        SceneManager.LoadScene(2);
    }

    public void Retry() {
        SceneManager.LoadScene(checkpointIndex);
        Player.GetInstance().GetComponent<Player>().spawn(new Vector2(-10.65f, 6.73f), savedHealth, savedMana);
        PlayerInfoCanvas.instance.GetComponent<Canvas>().enabled = true;
    }

    public void QuitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
