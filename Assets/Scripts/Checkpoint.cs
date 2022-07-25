using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static GameObject instance;
    public CheckPointButton checkPointButton;
    private void Awake() {
        instance = gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null) {
            AudioManager.instance.PlaySound(Sound.enter_checkpoint);
            ButtonMethods.checkpointIndex = SceneManager.GetActiveScene().buildIndex;
            ButtonMethods.savedHealth = player.health;
            ButtonMethods.savedMana = player.currentmana;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
