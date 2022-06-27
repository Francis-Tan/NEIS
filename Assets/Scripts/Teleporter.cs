using System.Collections;
using UnityEngine;

public class Teleporter : Enemy {
    public int dmg = 3;
    private float teleportRadius;
    private Vector2 pBoxSize;
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

        mana = 2;
        teleportRadius = (bc.size.magnitude + player.GetComponent<BoxCollider2D>().size.magnitude) / 2;
        pBoxSize = player.GetComponent<BoxCollider2D>().size + 2 * bc.size;
        gameObject.layer = 3;
        enabled = true;
    }

    protected override void unstunned_behaviour() {
        if (disappearTimer > 0) { disappearTimer -= Time.fixedDeltaTime; }
        else if (!sr.enabled) { appear(); }
        else if (appearTimer > 0) { attacking_behaviour(); appearTimer -= Time.fixedDeltaTime; }
        else {
            canAttack = false;
            rb.velocity = Vector2.zero;
            disappear();
            disappearTimer = timeTillDisappear;
            appearTimer = timeTillAppear;
        }
    }

    private void disappear() {
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
            //this is supposed to be transform.position, however it leads to annoying ganging-up situations
            sr.enabled = true;
            hiticon.GetComponent<SpriteRenderer>().enabled = true;
            ChangeAnimationState(Assassin_reappear);
            StartCoroutine(reappear());
            IEnumerator reappear() {
                yield return new WaitForSeconds(0.4f);
                bc.enabled = true;
                if (currentState != Assassin_stab) ChangeAnimationState(Assassin_idle);
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
        }
        else if (attackCooldown > 0) {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else if (canAttack) {
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