using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private Enemy[] enemies;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartBattle();
        }
    }

    private void StartBattle() {
        LoadLevel loadlevel = LoadLevel.instance;
        loadlevel.enemycount = enemies.Length;
        foreach (Enemy enemy in enemies) {
            loadlevel.takenoteof(enemy);
            enemy.Spawn();
        }
    }
}
