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
                rb.isKinematic = false;
                isdown = false;
                ammo = 3;
                if (currentState == Drone_reactivating) ChangeAnimationState(Drone_hovering);
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
            if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
            else {
                attack();
                attackCooldown = TimeBtwAttacks;
                ammo--;
            }
        }
        else {
            if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
            else {
                rb.isKinematic = true;
                isdown = true;
                ChangeAnimationState(Drone_reactivating);
                attackCooldown = TimeBtwAttacks;
                gameObject.layer = 3;
            }
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
