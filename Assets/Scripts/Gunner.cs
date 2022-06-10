using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    public gbullet_pooler gbullet_pooler;
    public float shooting_time = 3f;
    public float reload_time = 1f;

    const string
        Gunner_idle = "Gunner_idle",
        Gunner_aiming = "Gunner_aiming",
        Gunner_shoot = "Gunner_shoot",
        Gunner_reloadidle = "Gunner_reloadidle",
        Gunner_reloadmove = "Gunner_reloadmove", //to remove/replace
        Gunner_hit = "Gunner_hit",
        Gunner_die = "Gunner_die";

    private void Start() 
    {
        mana = 3;
        player = Player.GetInstance();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if (shooting_time > 0)
        {
            //ChangeAnimationState(Gunner_aiming);
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
        else if (reload_time > 0) 
        {
            //ChangeAnimationState(Gunner_reloadidle);
            reload_time -= Time.fixedDeltaTime;
            float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        } 
        else
        {
            //ChangeAnimationState(Gunner_idle or Gunner_reloaddone);
            attackCooldown = TimeBtwAttacks;
            shooting_time = 3f;
            reload_time = 1f;
        }
    }

    protected override void attack() 
    {
        //ChangeAnimationState(Gunner_shoot);
        gbullet_pooler.FireBullet();
        
    }

    protected override void playhitanimation()
    {
        ChangeAnimationState(Gunner_hit);
    }
    public override void Die() 
    {
        ChangeAnimationState(Gunner_die);
        Destroy(gameObject);
        gbullet_pooler.Die();
    }
}
