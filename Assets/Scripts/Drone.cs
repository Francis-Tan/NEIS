using System.Collections;
using UnityEngine;

public class Drone : Enemy {
    public GameObject projectile;
    public SpriteRenderer shield;
    public int ammo = 3;
    private bool down = false;
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

        timetillup = downtime;
        enabled = true;
        shield.enabled = true;
        //rb.isKinematic = true;
        gameObject.layer = 0;
        gameObject.tag = "Blocking";
    }
    protected override void unstunned_behaviour() {
        rb.velocity = Vector2.zero;
        if (down) {
            timetillup -= Time.fixedDeltaTime;
            if (timetillup <= 0) {
                //rb.isKinematic = false;
                AudioManager.instance.PlaySound(Sound.drone_activate);
                down = false;
                shield.enabled = true;
                ammo = 3;
                if (currentState == Drone_reactivating) ChangeAnimationState(Drone_hovering);
                timetillup = downtime;
                gameObject.layer = 0;
                gameObject.tag = "Blocking";
            }
        } else if (ammo > 0) {
            directionToPlayer = (player.transform.position - transform.position).normalized;
            deltapos = moveSpeed * Time.fixedDeltaTime * new Vector2(directionToPlayer.x, directionToPlayer.y);
            float distFromPlayer = bc.Distance(player.GetComponent<Collider2D>()).distance;
            if (distFromPlayer > 9) rb.MovePosition(rb.position + deltapos);
            if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
            else if (distFromPlayer <= 13) {
                attack();
                attackCooldown = TimeBtwAttacks;
                ammo--;
            }
        } else {
            if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
            else {
                //rb.isKinematic = true;
                AudioManager.instance.PlaySound(Sound.drone_deactivate);
                down = true;
                shield.enabled = false;
                ChangeAnimationState(Drone_reactivating);
                attackCooldown = TimeBtwAttacks;
                gameObject.layer = 3;
                gameObject.tag = "Enemy";
            }
        }
    }

    protected override void attack() {
        //ChangeAnimationState(Drone_shoot);
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }

    public override void Die() {
        rb.velocity = Vector2.zero;
        hiticon.GetComponent<SpriteRenderer>().enabled = false;
        ChangeAnimationState(Drone_die);
        AudioManager.instance.PlaySound(Sound.drone_die);
        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.35f);
            Destroy(gameObject);
        }
    }
}
