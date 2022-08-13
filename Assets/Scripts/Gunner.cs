using System.Collections;
using UnityEngine;

public class Gunner : Enemy {
    public gbullet_pooler gbullet_pooler;
    public float shooting_time = 3f;
    public float reload_time = 1f;
    private Vector3 hiticonpos, stuniconpos;
    private Quaternion stuniconrot;

    const string
        Gunner_idle = "Gunner_idle",
        Gunner_shoot = "Gunner_shoot",
        Gunner_reloadidle = "Gunner_reloadidle",
        Gunner_die = "Gunner_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;
        gameObject.layer = 3;
        enabled = true;
    }

    protected override void default_behaviour() {
        if (shooting_time > 0) {
            shooting_time -= Time.fixedDeltaTime;
            if (attackCooldown > 0) {
                attackCooldown -= Time.fixedDeltaTime;
            } else {
                attack();
                attackCooldown = TimeBtwAttacks;
            }
        } else if (reload_time > 0) {
            if (enabled) {
                ChangeAnimationState(Gunner_reloadidle);
            }
            reload_time -= Time.fixedDeltaTime;
        } else {
            if (enabled) {
                ChangeAnimationState(Gunner_idle);
            }
            attackCooldown = TimeBtwAttacks;
            shooting_time = 3f;
            reload_time = 1f;
        }
    }

    protected override void attack() {
        if (enabled) {
            ChangeAnimationState(Gunner_shoot);
        }
        AudioManager.instance.PlaySound(Sound.gunner_shoot);
        gbullet_pooler.FireBullet();
    }

    public override void Die() {
        enabled = false;
        hiticon.GetComponent<SpriteRenderer>().enabled = false;
        AudioManager.instance.PlaySound(Sound.gunner_die);
        ChangeAnimationState(Gunner_die);
        StartCoroutine(selfDestruct());
        IEnumerator selfDestruct() {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
        gbullet_pooler.Die();
    }
}