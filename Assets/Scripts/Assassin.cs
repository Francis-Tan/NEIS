using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    private SpriteRenderer sr; //change sprite direction
    public int dmg = 3;

    const string
        Assassin_idle = "Assassin_idle",
        Assassin_disappear = "Assassin_disappear",
        Assassin_reappear = "Assassin_reappear",
        Assassin_move = "Assassin_move",
        Assassin_stab = "Assassin_stab",
        Assassin_hit = "Assassin_hit",
        Assassin_die = "Assassin_die";

    private void Start() 
    {
        mana = 2;
        player = Player.GetInstance();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void unstunned_behaviour() 
    {
        ChangeAnimationState(Assassin_idle);
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = moveSpeed * Time.fixedDeltaTime * new Vector2(directionToPlayer.x, directionToPlayer.y);
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude)
        {
            rb.MovePosition(rb.position + deltapos);
        }
        else if (attackCooldown > 0)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else
        {
            attack();
            attackCooldown = TimeBtwAttacks;
        }
        sr.flipX = directionToPlayer.x >= 0;
    }

    protected override void attack() 
    {
        ChangeAnimationState(Assassin_stab);
        player.GetComponent<Player>().takeDamage(dmg); 
    }

    protected override void playhitanimation()
    {
        ChangeAnimationState(Assassin_hit);
    }

    public override void Die() 
    {
        enabled = false;
        Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;
        c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        ChangeAnimationState(Assassin_die);
        StartCoroutine(selfdestruct());
    }

    IEnumerator selfdestruct()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
