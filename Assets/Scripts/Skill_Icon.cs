using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Skill_Icon : MonoBehaviour {
    public int skillcost;
    public Image timerRing;
    public Image icon;
    public Sprite notready;
    public Sprite ready;
    public float cooldown = 1f;
    private float timer;

    private void Start() {
        Initialize();
    }
    public void Initialize() {
        timerRing.fillAmount = 0;
        timer = 0;
    } 
    public void FixedUpdate() { //why not update
        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
            timerRing.fillAmount = timer / cooldown;
        }
    }

    public void pressed(bool activating) {
        icon.sprite = activating ? ready : notready;
    }

    public void reset() {
        timerRing.fillAmount = 1;
        timer = cooldown;
    }

    public bool isready() {
        return timer <= 0;
    }
}
