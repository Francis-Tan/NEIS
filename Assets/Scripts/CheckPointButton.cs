using UnityEngine;
using UnityEngine.UI; //for Button class

public class CheckPointButton : MonoBehaviour
{
    private Button button;
    private int CheckPointIndex, savedHP, savedMana;
    
    public void UpdateCheckPoint(int hp, int mana) {
        savedHP = hp;
        savedMana = mana;
        //activate button if inactive
    }

    public void OnMouseClick() {
        /**SceneManager.LoadScene(CheckPointIndex);
        Player.GetInstance().GetComponent<Player>().spawn(new Vector2(-10.65f, 6.73f), savedHP, savedMana);
        PlayerInfoCanvas.instance.GetComponent<Canvas>().enabled = true*/
    }
}
