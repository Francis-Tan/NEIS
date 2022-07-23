using System.Collections;
using UnityEngine;

public class Assassin : Enemy {
    public int dmg = 3;

    const string
        Assassin_idle = "Assassin_idle",
        Assassin_disappear = "Assassin_disappear",
        Assassin_reappear = "Assassin_reappear",
        Assassin_move = "Assassin_move",
        Assassin_stab = "Assassin_stab",
        Assassin_die = "Assassin_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;

        gameObject.layer = 3;
        enabled = true;
    }

    protected override void unstunned_behaviour() {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = moveSpeed * Time.fixedDeltaTime * new Vector2(directionToPlayer.x, directionToPlayer.y);
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        }
        else if (attackCooldown > 0) {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else {
            attack();
            attackCooldown = TimeBtwAttacks;
        }
        sr.flipX = directionToPlayer.x >= 0;
    }

    protected override void attack() {
        ChangeAnimationState(Assassin_stab);
        player.GetComponent<Player>().takeDamage(dmg);
        StartCoroutine(wait());
        IEnumerator wait() { 
            yield return new WaitForSeconds(0.15f);
            if (currentState == Assassin_stab) { ChangeAnimationState(Assassin_idle); }
        }
    }

    public override void Die() {
        enabled = false;
        Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;
        c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        ChangeAnimationState(Assassin_die);
        StartCoroutine(selfdestruct());
        IEnumerator selfdestruct() {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
