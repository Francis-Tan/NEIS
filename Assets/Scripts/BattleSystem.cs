using UnityEngine;

public class BattleSystem : MonoBehaviour {
    //try to merge the script and object with collidertrigger
    private enum State {
        Idle,
        Active,
    }

    private State state;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private ColliderTrigger colliderTrigger;

    private void Awake() {
        state = State.Idle;
        colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
    }

    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e) {
        if (state == State.Idle) {
            StartBattle();
            //colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
            //no need since we disable the collider but good to keep in mind
        }
    }

    private void StartBattle() {
        state = State.Active;
        LoadLevel loadlevel = LoadLevel.instance;
        loadlevel.enemycount = enemies.Length;
        foreach (Enemy enemy in enemies) {
            loadlevel.takenoteof(enemy);
            enemy.Spawn();
        }
    }
}
