using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    public GameObject projectile;
    public int mana = 3;

    private void Start() {
         rb = GetComponent<Rigidbody2D>();
         bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime;
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        } 
        if (attackCooldown > 0) {    
            attackCooldown -= Time.fixedDeltaTime;
        } else {
            attack();
            attackCooldown = TimeBtwAttacks;
        }
        float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    protected override void attack() { 
        Instantiate(projectile, transform.position + directionToPlayer, transform.rotation);
    }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
