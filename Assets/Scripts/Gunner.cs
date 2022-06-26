using UnityEngine;

public class Gunner : Enemy {
    public gbullet_pooler gbullet_pooler;
    public float shooting_time = 3f;
    public float reload_time = 1f;
    private Vector3 hiticonpos, stuniconpos;
    private Quaternion stuniconrot;

    const string
        Gunner_idle = "Gunner_idle",
        Gunner_aiming = "Gunner_aiming",
        Gunner_shoot = "Gunner_shoot",
        Gunner_reloadidle = "Gunner_reloadidle",
        Gunner_reloadmove = "Gunner_reloadmove", //to remove/replace
        Gunner_die = "Gunner_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;

        hiticonpos = hiticon.transform.position;
        stuniconpos = stunicon.transform.position;
        stuniconrot = stunicon.transform.rotation;
        mana = 3;
        gameObject.layer = 3;
        enabled = true;
    }

    protected override void unstunned_behaviour() {
        if (shooting_time > 0) {
            //ChangeAnimationState(Gunner_aiming);
            shooting_time -= Time.fixedDeltaTime;
            if (attackCooldown > 0) {
                attackCooldown -= Time.fixedDeltaTime;
            }
            else {
                attack();
                attackCooldown = TimeBtwAttacks;
            }
            float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (reload_time > 0) {
            //ChangeAnimationState(Gunner_reloadidle);
            reload_time -= Time.fixedDeltaTime;
            float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else {
            //ChangeAnimationState(Gunner_idle or Gunner_reloaddone);
            attackCooldown = TimeBtwAttacks;
            shooting_time = 3f;
            reload_time = 1f;
        }
        hiticon.transform.position = hiticonpos;
        stunicon.transform.position = stuniconpos;
        stunicon.transform.rotation = stuniconrot;
    }

    protected override void attack() {
        //ChangeAnimationState(Gunner_shoot);
        gbullet_pooler.FireBullet();
    }

    public override void Die() {
        ChangeAnimationState(Gunner_die);
        Destroy(gameObject);
        gbullet_pooler.Die();
    }
}
