using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int health = 2;
    public GameObject player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //to detect whether it will collide with player - might replace with raycast box
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //the change in position
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;

    protected void MoveAndFacePlayer(float moveSpeed) {
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
        float angle = Vector2.SignedAngle(Vector2.right, directionToPlayer);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    protected abstract void attack();
    public abstract int Die();
}
