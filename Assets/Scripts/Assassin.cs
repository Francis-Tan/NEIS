using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb; //rigidbody movement better for collision
    private BoxCollider2D bc; //to detect whether it will collide with player - might replace with raycast box
    private Vector3 directionToPlayer;
    private Vector2 deltapos; //the change in position
    private Color suitcolor; //to change visibility
    public float moveSpeed, appearSpeed; //toggle rate of movement and visibility
    public int dmg;
    public float startTimeBtwSwings;
    private float timeTillNextSwing;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        suitcolor = GetComponent<SpriteRenderer>().material.color;
        suitcolor.a = 0;
        GetComponent<SpriteRenderer>().material.color = suitcolor;
        timeTillNextSwing = startTimeBtwSwings;
    }

    private void FixedUpdate() { //consider lateupdate if it could be useful
        suitcolor.a += appearSpeed * Time.fixedDeltaTime;
        GetComponent<SpriteRenderer>().material.color = suitcolor;

        directionToPlayer = (player.transform.position - transform.position).normalized;
        deltapos = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime;
        if (bc.Distance(player.GetComponent<Collider2D>()).distance > deltapos.magnitude) {
            rb.MovePosition(rb.position + deltapos);
        } else if (timeTillNextSwing > 0) {    
            timeTillNextSwing -= Time.fixedDeltaTime;
        } else {
            player.GetComponent<Player>().takeDamage(dmg);
            timeTillNextSwing = startTimeBtwSwings;
        }
        
        float angle = Vector2.SignedAngle(Vector2.right, directionToPlayer);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
