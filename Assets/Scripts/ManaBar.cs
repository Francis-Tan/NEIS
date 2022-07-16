using UnityEngine;
using UnityEngine.UI;
public class ManaBar : MonoBehaviour {
    public static ManaBar instance;
    public Image[] bars;
    private int currentBar = -1;
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject); 
            return;
        }
        instance = this;
    }

    public void updateBars(int newMana) {
        --newMana;
        while (currentBar < newMana) bars[++currentBar].color = Color.cyan;
        while (currentBar > newMana) bars[currentBar--].color = Color.black;
    }
}