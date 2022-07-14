using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static GameObject instance;
    private void Awake() {
        if (ButtonMethods.checkpointIndex == SceneManager.GetActiveScene().buildIndex) {
            Destroy(gameObject);
        }
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
