using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb; //rigidbody movement better for collision
    private BoxCollider2D bc; //to detect whether it will collide with player - might replace with raycast box
    private Vector3 directionToPlayer;
    private Vector2 deltapos; //the change in position
    public float moveSpeed;
    public float startTimeBtwShots;
    
    private float timeTillNextShot;
    public GameObject projectile;

    private void Start() {
         timeTillNextShot = startTimeBtwShots;
         rb = GetComponent<Rigidbody2D>();
         bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime;
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        }
        float angle = Vector2.SignedAngle(Vector2.right, directionToPlayer);
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (timeTillNextShot > 0) {
            timeTillNextShot -= Time.fixedDeltaTime;
        } else {
            GameObject bullet = Instantiate(projectile, transform.position + directionToPlayer, Quaternion.identity);
            Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            timeTillNextShot = startTimeBtwShots;
        }
    }
}
