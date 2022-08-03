using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour {
    public static LoadLevel instance;
    public Vector2 spawnPosition;
    public int enemycount;
    //should be able to cache this
    private void Awake() {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        instance = GetComponent<LoadLevel>();
    }

    public void takenoteof(Enemy enemy) {
        enemy.OnEnemyDeath += UpdateLift;
    }

    private void UpdateLift(object sender, EventArgs e) {
        if (--enemycount == 0) enable();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) enable();
    }

    public void enable() {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() != null) {
            Player.GetInstance().transform.position = spawnPosition;
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex == 2) {
                SceneMethods.GoToMenu();
            } else if (nextSceneIndex == SceneManager.sceneCountInBuildSettings - 1) {
                SceneMethods.GoToLevelSelect();
            } else {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}
