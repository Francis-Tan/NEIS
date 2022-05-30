using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    private SpriteRenderer sr; //to change visibility
    public int dmg = 3;
    public int mana = 2;

    const string
        Assassin_idle = "Assassin_idle",
        Assassin_disappear = "Assassin_disappear",
        Assassin_reappear = "Assassin_reappear",
        Assassin_move = "Assassin_move",
        Assassin_stab = "Assassin_stab",
        Assassin_hit = "Assassin_hit",
        Assassin_die = "Assassin_die";

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>(); //.material.color;
        //suitcolor.a = 0;
        //GetComponent<SpriteRenderer>().material.color = suitcolor;
    }

    private void FixedUpdate() { //consider lateupdate if it could be useful
        ChangeAnimationState(Assassin_idle);
        //suitcolor.a += appearSpeed * Time.fixedDeltaTime;
        //GetComponent<SpriteRenderer>().material.color = suitcolor;
        
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime;
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        } else if (attackCooldown > 0) {    
            attackCooldown -= Time.fixedDeltaTime;
        } else {
            attack();
            attackCooldown = TimeBtwAttacks;
        }
        sr.flipX = directionToPlayer.x >= 0;
    }

    protected override void attack() {
        ChangeAnimationState(Assassin_stab);
        player.GetComponent<Player>().takeDamage(dmg); 
    }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
