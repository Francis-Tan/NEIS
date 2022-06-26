using System.Collections;
using UnityEngine;

public class Drone : Enemy {
    public GameObject projectile;
    public int ammo = 3;
    private bool isdown = false;
    public float downtime;
    private float timetillup;

    const string
        Drone_hovering = "Drone_hovering",
        Drone_move = "Drone_move",
        Drone_shoot = "Drone_shoot",
        Drone_deactivating = "Drone_deactivating",
        Drone_recharging = "Drone_recharging",
        Drone_reactivating = "Drone_reactivating",
        Drone_die = "Drone_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;

        mana = 2;
        timetillup = downtime;
        enabled = true;
    }
    protected override void unstunned_behaviour() {
        if (isdown) {
            timetillup -= Time.fixedDeltaTime;
            if (timetillup <= 0) {
                isdown = false;
                ChangeAnimationState(Drone_hovering);
                timetillup = downtime;
                gameObject.layer = 9;
            }
        }
        else if (ammo > 0) {
            directionToPlayer = (player.transform.position - transform.position).normalized;
            deltapos = moveSpeed * Time.fixedDeltaTime * new Vector2(directionToPlayer.x, directionToPlayer.y);
            float distFromPlayer = bc.Distance(player.GetComponent<Collider2D>()).distance;
            if (distFromPlayer > 9) {
                rb.MovePosition(rb.position + deltapos);
            }
            if (distFromPlayer <= 12 && attackCooldown <= 0) {
                attack();
                attackCooldown = TimeBtwAttacks;
                ammo--;
            }
            attackCooldown -= Time.fixedDeltaTime;
        } else {
            isdown = true;
            ChangeAnimationState(Drone_reactivating);
            attackCooldown = TimeBtwAttacks;
            ammo = 3;
            gameObject.layer = 3;
        }
    }

    protected override void attack() {
        //ChangeAnimationState(Drone_shoot);
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }

    public override void Die() {
        ChangeAnimationState(Drone_die);
        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.35f);
            Destroy(gameObject);
        }
    }
}
