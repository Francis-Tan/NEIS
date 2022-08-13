using System.Collections;
using UnityEngine;

public class Assassin : Enemy {
    public int dmg = 3;
    private float teleportRadius;
    //private Vector2 pBoxSize;
    private float timeTillDisappear = 1f;
    private float timeTillAppear = 1f;
    private float disappearTimer;
    private float appearTimer;
    private bool canAttack;

    const string
        Assassin_idle = "Assassin_idle",
        Assassin_disappear = "Assassin_disappear",
        Assassin_reappear = "Assassin_reappear",
        Assassin_stab = "Assassin_stab",
        Assassin_die = "Assassin_die";

    public override void Spawn() {
        Color c = sr.material.color;
        c.a = 1;
        sr.material.color = c;
        teleportRadius = (bc.size.magnitude + player.GetComponent<BoxCollider2D>().size.magnitude) / 2;
        //pBoxSize = player.GetComponent<BoxCollider2D>().size + 2 * bc.size;
        gameObject.layer = 3;
        enabled = true;
    }

    protected override void default_behaviour() {
        if (disappearTimer > 0) { 
            disappearTimer -= Time.fixedDeltaTime; 
        } else if (!sr.enabled) { 
            appear(); 
        } else if (appearTimer > 0) { 
            attacking_behaviour(); 
            appearTimer -= Time.fixedDeltaTime; 
        } else {
            canAttack = false;
            rb.velocity = Vector2.zero;
            disappear();
            disappearTimer = timeTillDisappear;
            appearTimer = timeTillAppear;
        }
    }

    private void disappear() {
        AudioManager.instance.PlaySound(Sound.assassin_disappear);
        bc.enabled = false;
        hiticon.GetComponent<SpriteRenderer>().enabled = false;
        ChangeAnimationState(Assassin_disappear);
        StartCoroutine(disappear());
        IEnumerator disappear() {
            yield return new WaitForSeconds(0.3f);
            sr.enabled = false;
        }
    }

    private void appear() {
        float angle = Random.Range(0, 2 * Mathf.PI);
        transform.position = player.transform.position
                + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * teleportRadius;
        if (Mathf.Abs(transform.position.x) < 21 && Mathf.Abs(transform.position.y) < 14 &&
            Physics2D.OverlapBox(player.transform.position, bc.size, 0, 1) == null) {
            //if location within level boundary (should update those) and there are 
            //no colliders (nothing is on layer 1) overlapping with a box around the player, appear there
            AudioManager.instance.PlaySound(Sound.assassin_appear);
            sr.enabled = true;
            hiticon.GetComponent<SpriteRenderer>().enabled = true;
            ChangeAnimationState(Assassin_reappear);
            StartCoroutine(reappear());
            IEnumerator reappear() {
                yield return new WaitForSeconds(0.4f);
                bc.enabled = true;
                ChangeAnimationState(Assassin_idle);
                yield return new WaitForSeconds(0.125f);
                canAttack = true;
            }
        }
    }

    private void attacking_behaviour() {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = moveSpeed * Time.fixedDeltaTime * new Vector2(directionToPlayer.x, directionToPlayer.y);
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        } else if (attackCooldown > 0) {
            attackCooldown -= Time.fixedDeltaTime;
        } else if (canAttack) {
            attack();
            attackCooldown = TimeBtwAttacks;
        }
        sr.flipX = directionToPlayer.x >= 0;
    }

    protected override void attack() {
        AudioManager.instance.PlaySound(Sound.assassin_stab);
        ChangeAnimationState(Assassin_stab);
        player.GetComponent<Player>().takeDamage(dmg);
        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.15f);
            if (currentState == Assassin_stab) { ChangeAnimationState(Assassin_idle); }
            //check makes it smoother (changes can be sudden otherwise, idk why) and ensures death anim plays
        }
    }

    public override void Die() {
        enabled = false;
        rb.velocity = Vector2.zero;
        Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;
        c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        AudioManager.instance.PlaySound(Sound.assassin_die);
        ChangeAnimationState(Assassin_die);
        StartCoroutine(selfdestruct());
        IEnumerator selfdestruct() {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}