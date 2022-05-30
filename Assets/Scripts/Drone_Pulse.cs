using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Pulse : MonoBehaviour
{
    public int dmg = 3;
    public float detonate_time;

    private Animator animator;
    private string currentState;
    const string
        DP_idle = "DP_idle",
        DP_explode = "DP_explode";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {
        detonate_time -= Time.fixedDeltaTime;
        if (detonate_time <= 0) {
            Collider2D player = Physics2D.OverlapCircle(transform.position, 7f, 256);
            if (player != null) {
                player.gameObject.GetComponent<Player>().takeDamage(dmg);
            }
            Destroy(gameObject);
        }
    }
}
