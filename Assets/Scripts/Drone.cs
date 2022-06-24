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
        Drone_floorhit = "Drone_floorhit",
        Drone_die = "Drone_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;

        mana = 2;
        player = Player.GetInstance();
        animator = GetComponent<Animator>();
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
        else if (attackCooldown > 0) {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else if (ammo > 0) {
            attack();
            attackCooldown = TimeBtwAttacks;
            ammo--;
        }
        else {
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

    protected override void playhitanimation() {
        ChangeAnimationState(Drone_floorhit);
    }

    public override void Die() {
        ChangeAnimationState(Drone_die);
        Destroy(gameObject);
    }
}
