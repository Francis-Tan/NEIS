using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ManaBar : MonoBehaviour {
    private Image manaBar;
    private TMP_Text manaText;
    static float mana, maxmana;
    float lerpspeed;

    private void Start() {
        manaBar = GetComponent<Image>();
        manaText = GetComponentInChildren<TMP_Text>();
        maxmana = Player.GetInstance().GetComponent<Player>().maxmana;
        mana = 0;
    }

    private void FixedUpdate() {
        manaText.text = mana + "/" + maxmana;
        lerpspeed = 10 * Time.fixedDeltaTime;

        //updates the manabar fill level at speed lerpspeed
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, mana / maxmana, lerpspeed);

        //color starts at cyan and becomes bluer as health falls
        manaBar.color = Color.Lerp(Color.blue, Color.cyan, mana / maxmana);
    }
    public static void setmana(float amt) {
        mana = amt;
    }
}