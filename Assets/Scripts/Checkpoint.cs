using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static GameObject instance;
    public bool inTutorial = false;
    private int floornum;
    private void Awake() {
        instance = gameObject;
        floornum = SceneManager.GetActiveScene().buildIndex - 1;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null) {
            AudioManager.instance.PlaySound(Sound.enter_checkpoint);

            if (!inTutorial) CheckPointManager.UpdateCheckpoint(
                floornum,
                player.health,
                player.currentmana);
            
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
