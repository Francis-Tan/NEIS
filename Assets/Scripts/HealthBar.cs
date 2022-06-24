using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour {
    private Image healthBar;
    private TMP_Text healthText;
    static float health, maxHealth;
    float lerpspeed;

    private void Start() {
        healthBar = GetComponent<Image>();
        healthText = GetComponentInChildren<TMP_Text>();
        maxHealth = Player.GetInstance().GetComponent<Player>().health;
        health = maxHealth;
    }

    private void FixedUpdate() {
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