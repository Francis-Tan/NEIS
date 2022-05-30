using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    public GameObject projectile;
    public int mana = 2;
    private int ammo = 3;
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
        Drone_hit = "Drone_hit",
        Drone_die = "Drone_die";

    private void Start() {
        bc.enabled = false;
        timetillup = downtime;
    }
    private void FixedUpdate() {
        if (isdown) {
            timetillup -= Time.fixedDeltaTime;
            if (timetillup <= 0) {
                isdown = false;
                ChangeAnimationState(Drone_hovering);
                timetillup = downtime;
                bc.enabled = false;
            }
        } else if (attackCooldown > 0) {    
            attackCooldown -= Time.fixedDeltaTime;
        } else if (ammo > 0) {
            attack();
            attackCooldown = TimeBtwAttacks;
            ammo--;
        } else {    
            isdown = true;
            ChangeAnimationState(Drone_reactivating);
            attackCooldown = TimeBtwAttacks;
            ammo = 3;
            bc.enabled = true;
        }
    }

    protected override void attack() { 
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }

    public override void takeDamage() { 
        if (isdown) base.takeDamage();
    }

    public override int Die() {
        if (isdown) {
            //Instantiate()
            Destroy(gameObject);
            return mana;
        }
        return 0;
    }
}
