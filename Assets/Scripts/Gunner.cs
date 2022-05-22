using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    public float moveSpeed = 0;
    public GameObject projectile;
    public int mana = 3;

    private void Start() {
         rb = GetComponent<Rigidbody2D>();
         bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() { MoveAndFacePlayer(moveSpeed); }

    protected override void attack() { 
        GameObject bullet = Instantiate(projectile, transform.position + directionToPlayer, Quaternion.identity);
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
