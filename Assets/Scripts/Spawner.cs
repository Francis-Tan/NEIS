using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private Enemy[] enemies;
    private LoadLevel loadlevel;
    private void Start() {
        loadlevel = LoadLevel.instance;
        loadlevel.enemycount += enemies.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            Destroy(Checkpoint.instance);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartBattle();
        }
    }

    private void StartBattle() {
        foreach (Enemy enemy in enemies) {
            loadlevel.takenoteof(enemy);
            enemy.Spawn();
        }
        Destroy(gameObject);
    }
}
