using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Skill_Icon : MonoBehaviour {
    private Image countdownring;
    public Image icon;
    private TMP_Text costtxt;
    public int skillcost;
    public Sprite notready;
    public Sprite ready;
    public float cooldown = 1f;
    private float timer;

    private void Start() {
        countdownring = GetComponent<Image>();
        costtxt = GetComponentInChildren<TMP_Text>();
        costtxt.text = "" + skillcost;
        initialize();
    }
    public void initialize() {
        countdownring.fillAmount = 1;
        timer = cooldown;
    } 
    public void FixedUpdate() { //why not update
        if (timer < cooldown) {
            timer += Time.fixedDeltaTime;
            countdownring.fillAmount = timer / cooldown;
        } else {
            countdownring.color = Color.white;
        }
    }

    public void pressed(bool activating) {
        costtxt.color = activating ? Color.magenta : Color.white;
    }

    public void reset() {
        countdownring.fillAmount = 0;
        countdownring.color = Color.blue;
        timer = 0;
    }

    public void updatesprite(int mana) {
        icon.sprite = mana < skillcost ? notready : ready;
    }

    public bool isready() {
        return timer >= cooldown;
    }
}
