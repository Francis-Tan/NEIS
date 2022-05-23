using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb; //rigidbody movement is to avoid moving in a rotated transform after rotation
    private Vector2 directionToPlayer;
    public float moveSpeed = 16;
    public int dmg = 1;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //instance, not prefab
        rb = GetComponent<Rigidbody2D>();
        directionToPlayer = (player.transform.position - transform.position).normalized;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) { 
        GameObject runinto = other.gameObject;
        if (runinto.CompareTag("Player")) { 
            runinto.GetComponent<Player>().takeDamage(dmg); 
            //Instantiate(bulletexplode, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
        if (runinto.CompareTag("Blocking")) { 
            //Instantiate(bulletexplode, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }
}
