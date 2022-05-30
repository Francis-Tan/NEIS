using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    public Transform firepoint;
    public GameObject projectile;
    public int mana = 3;
    public float shooting_time = 3f;
    public float reload_time = 1f;

    const string
        Gunner_idle = "Gunner_idle",
        Gunner_move = "Gunner_move",
        Gunner_shoot = "Gunner_shoot",
        Gunner_reloadidle = "Gunner_reloadidle",
        Gunner_reloadmove = "Gunner_reloadmove",
        Gunner_hit = "Gunner_hit",
        Gunner_die = "Gunner_die";

    private void Start() {
         rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (shooting_time > 0)
        {
            shooting_time -= Time.fixedDeltaTime;
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.fixedDeltaTime;
            }
            else
            {
                attack();
                attackCooldown = TimeBtwAttacks;
            }
            float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (reload_time > 0) {
            reload_time -= Time.fixedDeltaTime;
            directionToPlayer = (player.transform.position - transform.position).normalized;
            deltapos = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime;
            if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude)
            {
                rb.MovePosition(rb.position - deltapos);
            }
            float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        } else
        {
            attackCooldown = TimeBtwAttacks;
            shooting_time = 3f;
            reload_time = 1f;
        }
    }

    protected override void attack() { 
        Instantiate(projectile, firepoint.position, firepoint.rotation);
    }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
