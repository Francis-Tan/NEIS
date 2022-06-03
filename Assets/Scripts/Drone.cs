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
        Drone_floorhit = "Drone_floorhit",
        Drone_die = "Drone_die";

    private void Start() 
    {
        player = Player.GetInstance();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        bc.enabled = false;
        timetillup = downtime;
    }
    private void FixedUpdate() 
    {
        if (isdown)
        {
            timetillup -= Time.fixedDeltaTime;
            if (timetillup <= 0)
            {
                isdown = false;
                ChangeAnimationState(Drone_hovering);
                timetillup = downtime;
                bc.enabled = false;
            }
        }
        else if (attackCooldown > 0)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else if (ammo > 0)
        {
            attack();
            attackCooldown = TimeBtwAttacks;
            ammo--;
        }
        else
        {
            isdown = true;
            ChangeAnimationState(Drone_reactivating);
            attackCooldown = TimeBtwAttacks;
            ammo = 3;
            bc.enabled = true;
        }
    }

    protected override void attack() 
    {
        //ChangeAnimationState(Drone_shoot);
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }

    protected override void playhitanimation()
    {
        ChangeAnimationState(Drone_floorhit);
    }

    public override int Die() 
    {
        if (isdown) 
        {
            ChangeAnimationState(Drone_die);
            Destroy(gameObject);
            return mana;
        }
        return 0;
    }
}
