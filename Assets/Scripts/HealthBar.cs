using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour {
    public Image healthBar;
    private TMP_Text healthText;
    static float health, maxHealth;
    float lerpspeed;

    private void Start() {
        healthText = GetComponentInChildren<TMP_Text>();
        maxHealth = Player.GetInstance().GetComponent<Player>().health;
        health = maxHealth;
    }

    private void FixedUpdate() { 
        //should update only when health changes
        healthText.text = health + "/" + maxHealth;
        lerpspeed = 10 * Time.fixedDeltaTime;

        //updates the healthbar fill level at speed lerpspeed
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpspeed);

        healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
    }
    public static void sethealth(float amt) {
        health = amt;
    }
}