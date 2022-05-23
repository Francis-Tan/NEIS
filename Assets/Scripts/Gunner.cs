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
        GameObject bullet = Instantiate(projectile, transform.position + directionToPlayer, transform.rotation);
    }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
