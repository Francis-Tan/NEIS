using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject player;
    //prob need box to detect player
    private Rigidbody2D rb; //rigidbody movement is to prevent rotatation of travel direction when rotating transform
    private Vector3 directionToPlayer;
    public float moveSpeed;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player"); //instance, not prefab
        rb = GetComponent<Rigidbody2D>();
        directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, directionToPlayer));
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed * Time.fixedDeltaTime);
        //if (transform.position.x == player.x && transform.position.y == player.y) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D other) {   
        //if(other.CompareTag("Player")) { Destroy(gameObject); }
    }
}
